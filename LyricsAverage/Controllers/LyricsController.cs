﻿using System;
using System.Collections.Generic;
using System.Linq;
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
            _lyricsCounter = lyricsCounter;
        }

        public async Task<IActionResult> Artist(string artistName)
        {
            return View(await _lyricsCounter.GetSongLyricWordCounts(artistName));
        }
    }
}