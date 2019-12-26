using System.Collections.Generic;

namespace LyricsAverage.Services
{
    public interface ISongRetriever
    {
        public IEnumerable<string> ArtistSongTitles(string artistName);
    }
}
