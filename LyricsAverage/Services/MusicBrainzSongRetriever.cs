using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text.Json;
using System.Threading.Tasks;
using LyricsAverage.Helpers;
using LyricsAverage.Models;

namespace LyricsAverage.Services
{
    public class MusicBrainzSongRetriever : ISongRetriever
    {
        private readonly HttpClient _httpClient;
        private readonly int _limit = 100;

        public MusicBrainzSongRetriever(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public ArtistSongTitles ArtistSongTitles(string artistName)
        {
            
            var artist = GetArtistId(artistName).Result;
            return new ArtistSongTitles
            {
                Artist = artist.Name,
                SongTitles = GetSongTitles(artist.Id).Result
            };
        }

         private async Task<Artist> GetArtistId(string artistName)
         {
             var uri = $"artist/?query=artist:{artistName}";
             var response = await _httpClient.GetAsync(uri);

            if (response.IsSuccessStatusCode)
            {
                await using var responseStream = await response.Content.ReadAsStreamAsync();
                var artistsResponse = await JsonSerializer.DeserializeAsync<ArtistQueryResponse>(responseStream, new JsonSerializerOptions{PropertyNameCaseInsensitive = true});
                return artistsResponse.Artists.FirstOrDefault();
            }

            return null;
         }

         private async Task<IEnumerable<string>> GetSongTitles(string artistId)
         {
            List<string> songNames = new List<string>();
            var worksResponse = await MakeWorksRequest(artistId, _limit, 0);
            
            StoreSongNames(worksResponse);

            var pageCount = PagingHelper.PageCount(worksResponse.WorkCount, _limit);
            for (var i = 1; i < pageCount; i++)
            {
                worksResponse = await MakeWorksRequest(artistId, _limit, _limit * i - 1);
                if(worksResponse is null)
                {
                    continue;
                }
                StoreSongNames(worksResponse);
            }

            return songNames;

            void StoreSongNames(WorksResponse response)
            {
                songNames.AddRange(response.Works.Where(s => s.Type == "Song").Select(s => s.Title));
            }
         }

         private async Task<WorksResponse> MakeWorksRequest(string artistId, int limit, int offset)
         {
             var uri = $"work?artist={artistId}&limit={limit}&offset={offset}";

             var response = await _httpClient.GetAsync(uri);

             if (response.IsSuccessStatusCode)
             {
                 await using var responseStream = await response.Content.ReadAsStreamAsync();
                 return await JsonSerializer.DeserializeAsync<WorksResponse>(responseStream,
                     new JsonSerializerOptions { PropertyNamingPolicy = JsonNamingPolicy.CamelCase });
             }
             return null;
         }
    }
}
