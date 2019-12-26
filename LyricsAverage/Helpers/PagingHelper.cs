using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LyricsAverage.Helpers
{
    public class PagingHelper
    {
        public static int PageCount(int count, int limit)
        {
            return count % limit != 0
                ? count / limit + 1
                : count / limit;
        }

    }
}
