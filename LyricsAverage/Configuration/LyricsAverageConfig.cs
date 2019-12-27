using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LyricsAverage.Configuration
{
    public class LyricsAverageConfig : ILyricsAverageConfig
    {
        public int RequestTimeoutSeconds { get; set; }
    }
}
