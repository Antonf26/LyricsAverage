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
            var artistSongTitles = _songRetriever.ArtistSongTitles(artist);

            var tasks = artistSongTitles.SongTitles.Select(song => _lyricsRetriever.GetLyrics(artist, song)).ToList();

            var allLyrics = await Task.WhenAll(tasks);
            var wordCounts = allLyrics.Where(l => l != null).ToList();
            return new AverageLyricsResponse(wordCounts, artistSongTitles.Artist);
        }

    }
}
