using System.Collections.Generic;
using LyricsAverage.Models;

namespace LyricsAverage.Services
{
    public interface ISongRetriever
    {
        public ArtistSongTitles ArtistSongTitles(string artistName);
    }
}
