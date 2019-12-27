using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;

namespace LyricsAverage.Models
{
    public class AverageLyricsResponse
    {
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
