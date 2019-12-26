using System.Threading.Tasks;
using LyricsAverage.Models;

namespace LyricsAverage.Services
{
    public interface ILyricsRetriever
    {
        public Task<SongLyrics> GetLyrics(string artist, string song);

    }
}
