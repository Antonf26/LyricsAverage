using System.Net.Http;
using System.Text.Json;
using System.Threading.Tasks;
using LyricsAverage.Models;

namespace LyricsAverage.Services
{
    public class OvhLyricsRetriever : ILyricsRetriever
    {
        private readonly HttpClient _httpClient;

        public OvhLyricsRetriever(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task<SongLyrics> GetLyrics(string artist, string song)
        {
            var uri = $"{artist}/{song}";
            var response = await _httpClient.GetAsync(uri);

            if (!response.IsSuccessStatusCode) return null;
            await using var responseStream = await response.Content.ReadAsStreamAsync();
            var lyricsResponse = await JsonSerializer.DeserializeAsync<LyricsResponse>(responseStream, new JsonSerializerOptions { PropertyNameCaseInsensitive = true });
            return new SongLyrics(song, lyricsResponse.Lyrics);

        }
    }
}
