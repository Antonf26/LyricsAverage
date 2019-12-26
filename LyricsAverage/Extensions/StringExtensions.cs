using System;
using System.Linq;
using LyricsAverage.Models;

namespace LyricsAverage.Extensions
{
    public static class StringExtensions
    {
        public static WordCountDetails GetWordCount(this string input)
        {
            char[] delimiters = new char[] { ' ', '\r', '\n' };

            var words = input.Split(delimiters, StringSplitOptions.RemoveEmptyEntries);
            return new WordCountDetails
            {
                WordCount = words.Length,
                DistinctWordCount = words.Distinct().Count()
            };
        }
    }
}
