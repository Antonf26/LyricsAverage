using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using LyricsAverage.Extensions;

namespace LyricsAverage.Models
{
    public class SongLyrics : IComparable<SongLyrics>
    {
        public SongLyrics(string title, string lyrics)
        {
            Title = title;
            Lyrics = lyrics;
            WordCount = Lyrics.GetWordCount();
        }

        public string Title { get; }
        public string Lyrics { get; }
        public WordCountDetails WordCount { get;  }
        public int CompareTo(SongLyrics other)
        {
            return WordCount.WordCount.CompareTo(other.WordCount.WordCount);
        }

    }
}
