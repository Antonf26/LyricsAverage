using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LyricsAverage.Models
{
    public class ArtistSongTitles
    {
        public string Artist { get; set; }
        public IEnumerable<string> SongTitles { get; set; }
    }
}
