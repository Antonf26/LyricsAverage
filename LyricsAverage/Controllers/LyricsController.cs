using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using LyricsAverage.Configuration;
using LyricsAverage.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;

namespace LyricsAverage.Controllers
{
    public class LyricsController : Controller
    {
        private readonly ILyricsCounter _lyricsCounter;
        private readonly IOptions<LyricsAverageConfig> _config;

        public LyricsController(ILyricsCounter lyricsCounter, IOptions<LyricsAverageConfig> config)
        {
            _lyricsCounter = lyricsCounter ?? throw new ArgumentNullException(nameof(lyricsCounter));
            _config = config ?? throw new ArgumentNullException(nameof(config));
        }

        public async Task<IActionResult> Artist(string artistName)
        {
            var cts = new CancellationTokenSource();
            cts.CancelAfter(TimeSpan.FromSeconds(_config.Value.RequestTimeoutSeconds));
            return View(await _lyricsCounter.GetLyricsAverages(artistName, cts.Token));
        }

    }
}