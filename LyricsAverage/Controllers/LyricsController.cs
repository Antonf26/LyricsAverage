using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using LyricsAverage.Services;
using Microsoft.AspNetCore.Mvc;

namespace LyricsAverage.Controllers
{
    public class LyricsController : Controller
    {
        private readonly ILyricsCounter _lyricsCounter;

        public LyricsController(ILyricsCounter lyricsCounter)
        {
            _lyricsCounter = lyricsCounter ?? throw new ArgumentNullException(nameof(_lyricsCounter));
        }

        public async Task<IActionResult> Artist(string artistName)
        {
            var cts = new CancellationTokenSource();
            cts.CancelAfter(TimeSpan.FromSeconds(30));
            return View(await _lyricsCounter.GetLyricsAverages(artistName, cts.Token));
        }

    }
}