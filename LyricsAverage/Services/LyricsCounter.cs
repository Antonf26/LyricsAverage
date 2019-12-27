using System;
using System.Collections.Generic;
using LyricsAverage.Models;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace LyricsAverage.Services
{
    public class LyricsCounter : ILyricsCounter
    {
        private readonly ISongRetriever _songRetriever;
        private readonly ILyricsRetriever _lyricsRetriever;

        public LyricsCounter(ISongRetriever songRetriever, ILyricsRetriever lyricsRetriever)
        {
            _songRetriever = songRetriever ?? throw new ArgumentNullException(nameof(songRetriever));
            _lyricsRetriever = lyricsRetriever ?? throw  new ArgumentNullException(nameof(lyricsRetriever));
        }

        public async Task<AverageLyricsResponse> GetLyricsAverages(string artist, CancellationToken ct)
        {
            var artistSongTitles = _songRetriever.ArtistSongTitles(artist);

            var tasks = artistSongTitles.SongTitles.Select(song => _lyricsRetriever.GetLyrics(artist, song)).ToList();
            List<SongLyrics> lyricsRetrieved = new List<SongLyrics>();
            while (tasks.Any() && !ct.IsCancellationRequested)
            {
                var finishedTask = await Task.WhenAny(tasks);
                tasks.Remove(finishedTask);
                if (!finishedTask.IsFaulted)
                {
                    lyricsRetrieved.Add(await finishedTask);
                }
            }
            return BuildResponse(lyricsRetrieved, artistSongTitles.Artist);
        }



        private AverageLyricsResponse BuildResponse(IEnumerable<SongLyrics> songLyrics, string artistName)
        {
            var result = new AverageLyricsResponse {Artist = artistName};
            var lyrics = songLyrics.Where(l => l != null).ToList();
            
            if (lyrics.Any())
            {
                result.SongsAnalysed = lyrics.Count();
                result.AverageWords = Math.Round(lyrics.Average(wc => wc.WordCount.WordCount), 2);
                result.AverageDistinctWords = Math.Round(lyrics.Average(wc => wc.WordCount.DistinctWordCount), 2);
                result.SongWithMostWords = lyrics.Max();
                result.SongWithFewestWords = lyrics.Min();
            }

            return result;
        }
    }
}
