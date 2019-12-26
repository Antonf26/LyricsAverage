using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using LyricsAverage.Extensions;
using LyricsAverage.Models;
using static System.String;

namespace LyricsAverage.Services
{
    public class LyricsCounter : ILyricsCounter
    {
        private readonly ISongRetriever _songRetriever;
        private  readonly ILyricsRetriever _lyricsRetriever;

        public LyricsCounter(ISongRetriever songRetriever, ILyricsRetriever lyricsRetriever)
        {
            _songRetriever = songRetriever;
            _lyricsRetriever = lyricsRetriever;
        }

        public async Task<AverageLyricsResponse> GetSongLyricWordCounts(string artist)
        {
            var songs = _songRetriever.ArtistSongTitles(artist);

            var tasks = new List<Task<string>>();

            foreach (var song in songs)
            {
                tasks.Add(_lyricsRetriever.GetLyrics(artist, song));
            }

            var allLyrics = await Task.WhenAll(tasks);
            var wordCounts = allLyrics.Where(l => !IsNullOrEmpty(l)).Select(l => l.GetWordCount());
            return new AverageLyricsResponse(wordCounts);
        }
    }
}
