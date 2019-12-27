using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LyricsAverage.Configuration
{
    public interface ILyricsAverageConfig
    {
        public int RequestTimeoutSeconds { get; set; }
    }
}
