using System.Threading.Tasks;
using LyricsAverage.Models;
using LyricsAverage.Services;
using Microsoft.AspNetCore.Mvc;

namespace LyricsAverage.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ArtistController : ControllerBase
    {
        private readonly ILyricsCounter _lyricsCounter;

        public ArtistController(ILyricsCounter lyricsCounter)
        {
            _lyricsCounter = lyricsCounter;
        }

        [Route("{name}")]
        [HttpGet]
        public async Task<AverageLyricsResponse> Get(string name)
        {
            return await _lyricsCounter.GetSongLyricWordCounts(name);
        }
    
}
}