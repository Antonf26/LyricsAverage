using System;
using System.Collections.Generic;
using System.Text;
using LyricsAverage.Helpers;
using NUnit.Framework;

namespace LyricsAverage.Tests
{
    public class PagingHelperTests
    {
        [TestCase(10,10, 1)]
        [TestCase(11, 10,2)]
        [TestCase(0, 10, 0)]
        [TestCase(20, 5, 4)]

        public void PagingHelper_ReturnsCorrectPageCount(int count, int limit, int expected)
        {
            var actual = PagingHelper.PageCount(count, limit);
            Assert.AreEqual(expected, actual);
        }
    }
}
