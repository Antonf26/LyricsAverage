using System.Threading.Tasks;

namespace LyricsAverage.Services
{
    public interface ILyricsRetriever
    {
        public Task<string> GetLyrics(string artist, string song);

    }
}
