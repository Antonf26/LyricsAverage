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

        public async Task<string> GetLyrics(string artist, string song)
        {
            var uri = $"{artist}/{song}";
            var response = await _httpClient.GetAsync(uri);

            if (response.IsSuccessStatusCode)
            {
                await using var responseStream = await response.Content.ReadAsStreamAsync();
                var lyricsResponse = await JsonSerializer.DeserializeAsync<LyricsResponse>(responseStream, new JsonSerializerOptions { PropertyNameCaseInsensitive = true });
                return lyricsResponse.Lyrics;
            }

            return null;
        }
    }
}
