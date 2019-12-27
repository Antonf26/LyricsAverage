using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;

namespace LyricsAverage.Models
{
    public class AverageLyricsResponse
    {
        public AverageLyricsResponse(IEnumerable<SongLyrics> songLyrics, string artist)
        {
            Artist = artist;
            var lyrics = songLyrics.ToList();
            if (!lyrics.Any()) return;
            SongsAnalysed = lyrics.Count();
            AverageWords = Math.Round(lyrics.Average(wc => wc.WordCount.WordCount), 2);
            AverageDistinctWords = Math.Round(lyrics.Average(wc => wc.WordCount.DistinctWordCount), 2);
            SongWithMostWords = lyrics.Max();
            SongWithFewestWords = lyrics.Min();
        }

        
        public string Artist { get; set; }

        [DisplayName("Songs Analysed")]
        public int SongsAnalysed { get; set; }
        [DisplayName("Average Words")]
        public double AverageWords { get; set; }
        [DisplayName("Average Distinct Words")]
        public double AverageDistinctWords { get; set; }
        [DisplayName("Song with most words")]
        public SongLyrics SongWithMostWords { get; set; }
        [DisplayName("Song with fewest words")]
        public SongLyrics SongWithFewestWords { get; set; }
    }
}
