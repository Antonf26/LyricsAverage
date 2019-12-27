using System.Threading;
using System.Threading.Tasks;
using LyricsAverage.Models;

namespace LyricsAverage.Services
{
    public interface ILyricsCounter
    {
        public Task<AverageLyricsResponse> GetLyricsAverages(string artist, CancellationToken ct);
    }
}
