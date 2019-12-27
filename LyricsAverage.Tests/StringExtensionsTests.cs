using LyricsAverage.Extensions;
using NUnit.Framework;

namespace LyricsAverage.Tests
{
    public class StringExtensionsTests
    {
        
        [TestCase("Two words", 2)]
        [TestCase("Oneword", 1)]
        [TestCase("", 0)]
        [TestCase("One Two \n Three \r Four", 4)]
        public void WordCount_ShouldReturnCorrectNumberOfWords(string text, int expectedCount)
        {
            var actual = text.GetWordCount();
            Assert.AreEqual(expectedCount, actual.WordCount);
        }

        [TestCase("Two words", 2)]
        [TestCase("Oneword", 1)]
        [TestCase("", 0)]
        [TestCase("One Two \n Three \r Four", 4)]
        [TestCase("Boom Boom", 1)]
        [TestCase("La la LA", 1)]
        public void WordCount_ShouldReturnCorrectNumberOfDistinctWords(string text, int expectedCount)
        {
            var actual = text.GetWordCount();
            Assert.AreEqual(expectedCount, actual.DistinctWordCount);
        }

    }
}