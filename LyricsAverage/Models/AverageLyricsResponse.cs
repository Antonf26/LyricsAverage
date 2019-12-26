using System;
using System.Collections.Generic;
using System.Linq;

namespace LyricsAverage.Models
{
    public class AverageLyricsResponse
    {
        public AverageLyricsResponse(IEnumerable<WordCountDetails> wordCounts)
        {
            SongsAnalysed = wordCounts.Count();
            AverageWords = Math.Round(wordCounts.Average(wc => wc.WordCount), 2);
            AverageDistinctWords = Math.Round(wordCounts.Average(wc => wc.DistinctWordCount), 2);
        }

        public int SongsAnalysed { get; set; }
        public double AverageWords { get; set; }
        public double AverageDistinctWords { get; set; }
    }
}
