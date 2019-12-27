using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LyricsAverage.Models;
using LyricsAverage.Services;
using Moq;
using NUnit.Framework;

namespace LyricsAverage.Tests.Services
{
    public class LyricsCounterTests
    {
        private LyricsCounter _sut;
        private Mock<ISongRetriever> _songRetrieverMock;
        private Mock<ILyricsRetriever> _lyricsRetrieverMock;

        [SetUp]
        public void Init()
        {
            _songRetrieverMock = new Mock<ISongRetriever>();
            _lyricsRetrieverMock = new Mock<ILyricsRetriever>();
            _sut = new LyricsCounter(_songRetrieverMock.Object, _lyricsRetrieverMock.Object);
        }

        [Test]
        public void LyricsCounter_PassedNullSongRetriever_ThrowsNullArgumentException()
        {
            Assert.Throws<ArgumentNullException>(() => _ = new LyricsCounter(null, _lyricsRetrieverMock.Object));
        }

        [Test]
        public void LyricsCounter_PassedNullLyricsRetriever_ThrowsNullArgumentException()
        {
            Assert.Throws<ArgumentNullException>(() => _ = new LyricsCounter(_songRetrieverMock.Object, null));
        }

        [Test]
        public async Task LyricsCounter_GetLyricsAverages_CallsSongRetrieverWithArtistName()
        {
            var artistName = "Michael Jackson";
            _songRetrieverMock.Setup(sr => sr.ArtistSongTitles(artistName)).Returns(new ArtistSongTitles{SongTitles = new List<string>()});

            _ = await _sut.GetLyricsAverages(artistName);

            _songRetrieverMock.Verify(sr => sr.ArtistSongTitles(artistName), Times.Once);
        }

        [Test]
        public async Task LyricsCounter_GetLyricsAverages_CallsLyricsRetrieverForEachSong()
        {
            var artistName = "Gorillaz";
            var songNames = new List<string> {"Feel Good Inc.", "Clint Eastwood", "DARE"};
            var artistSongTitles = new ArtistSongTitles {Artist = artistName, SongTitles = songNames};
            _songRetrieverMock.Setup(sr => sr.ArtistSongTitles(artistName)).Returns(artistSongTitles);

            _ = await _sut.GetLyricsAverages(artistName);

            foreach (var song in songNames)
            {
                _lyricsRetrieverMock.Verify(lr => lr.GetLyrics(artistName, song), Times.Once);
            }
        }

        [Test]
        public async Task LyricsCounter_GetLyricsAverages_ReturnsResponseWithArtistNameFromApi()
        {
            var artistName = "prodigy";
            var fullArtistName = "The Prodigy";
            var artistSongTitles = new ArtistSongTitles { Artist = fullArtistName, SongTitles = new List<string>()};
            _songRetrieverMock.Setup(sr => sr.ArtistSongTitles(artistName)).Returns(artistSongTitles);

            var response = await _sut.GetLyricsAverages(artistName);

            Assert.AreEqual(fullArtistName, response.Artist);
        }

        [Test]
        public async Task LyricsCounter_GetLyricsAverages_NoLyricsFoundReturnsCountsAs0()
        {
            var artistName = "prodigy";
            var fullArtistName = "The Prodigy";
            var artistSongTitles = new ArtistSongTitles { Artist = fullArtistName, SongTitles = new List<string>() };
            _songRetrieverMock.Setup(sr => sr.ArtistSongTitles(artistName)).Returns(artistSongTitles);

            var response = await _sut.GetLyricsAverages(artistName);

            Assert.AreEqual(0, response.SongsAnalysed);
            Assert.AreEqual(0, response.AverageDistinctWords);
            Assert.AreEqual(0, response.AverageWords);
        }

        [Test]
        public async Task LyricsCounter_GetLyricsAverages_ReturnsCorrectNumberOfSongsAnalysed()
        {
            var artistName = "test";
            var songTitles = new List<string> {"song1", "song2", "song3"};

            var artistSongTitles = new ArtistSongTitles { Artist = artistName, SongTitles = songTitles};
            _songRetrieverMock.Setup(sr => sr.ArtistSongTitles(artistName)).Returns(artistSongTitles);
            _lyricsRetrieverMock.Setup(lr => lr.GetLyrics(artistName, "song1"))
                .Returns(Task.FromResult(new SongLyrics("song1", GenerateLyrics(1))));
            _lyricsRetrieverMock.Setup(lr => lr.GetLyrics(artistName, "song2"))
                .Returns(Task.FromResult(new SongLyrics("song2", GenerateLyrics(3))));
            _lyricsRetrieverMock.Setup(lr => lr.GetLyrics(artistName, "song3"))
                .Returns(Task.FromResult<SongLyrics>(null));

            var response = await _sut.GetLyricsAverages(artistName);

            Assert.AreEqual(2, response.SongsAnalysed, "Should return number of songs analysed where lyrics response not null");
        }

        [Test]
        public async Task LyricsCounter_GetLyricsAverages_ReturnsCorrectAverageWordCount()
        {
            var artistName = "test";
            var songTitles = new List<string> { "song1", "song2"};
            var song1Lyrics = GenerateLyrics(4);
            var song2Lyrics = GenerateLyrics(6);

            var artistSongTitles = new ArtistSongTitles { Artist = artistName, SongTitles = songTitles };
            
            _songRetrieverMock.Setup(sr => sr.ArtistSongTitles(artistName)).Returns(artistSongTitles);

            _lyricsRetrieverMock.Setup(lr => lr.GetLyrics(artistName, "song1"))
                .Returns(Task.FromResult(new SongLyrics("song1", song1Lyrics)));

            _lyricsRetrieverMock.Setup(lr => lr.GetLyrics(artistName, "song2"))
                .Returns(Task.FromResult(new SongLyrics("song2", song2Lyrics)));

            var response = await _sut.GetLyricsAverages(artistName);

            Assert.AreEqual(5, response.AverageWords, "Should return average number of total words per song");
        }

        [Test]
        public async Task LyricsCounter_GetLyricsAverages_ReturnsCorrectAverageDistinctWordCount()
        {
            var artistName = "test";
            var songTitles = new List<string> { "song1", "song2" };
            var song1Lyrics = GenerateLyrics(4, 2);
            var song2Lyrics = GenerateLyrics(6, 3);


            var artistSongTitles = new ArtistSongTitles { Artist = artistName, SongTitles = songTitles };
            
            _songRetrieverMock.Setup(sr => sr.ArtistSongTitles(artistName)).Returns(artistSongTitles);
            
            _lyricsRetrieverMock.Setup(lr => lr.GetLyrics(artistName, "song1"))
                .Returns(Task.FromResult(new SongLyrics("song1", song1Lyrics)));
            
            _lyricsRetrieverMock.Setup(lr => lr.GetLyrics(artistName, "song2"))
                .Returns(Task.FromResult(new SongLyrics("song2", song2Lyrics)));

            var response = await _sut.GetLyricsAverages(artistName);

            Assert.AreEqual(2.5, response.AverageDistinctWords, "Should return average number of distinct words per song");
        }

        [Test]
        public async Task LyricsCounter_GetLyricsAverages_ReturnsCorrectSongWithMostWords()
        {
            var artistName = "test";
            var songTitles = new List<string> { "song1", "song2" };
            var song1Lyrics = GenerateLyrics(4, 2);
            var song2Lyrics = GenerateLyrics(6, 3);


            var artistSongTitles = new ArtistSongTitles { Artist = artistName, SongTitles = songTitles };

            _songRetrieverMock.Setup(sr => sr.ArtistSongTitles(artistName)).Returns(artistSongTitles);

            _lyricsRetrieverMock.Setup(lr => lr.GetLyrics(artistName, "song1"))
                .Returns(Task.FromResult(new SongLyrics("song1", song1Lyrics)));

            _lyricsRetrieverMock.Setup(lr => lr.GetLyrics(artistName, "song2"))
                .Returns(Task.FromResult(new SongLyrics("song2", song2Lyrics)));

            var response = await _sut.GetLyricsAverages(artistName);

            Assert.AreEqual("song2", response.SongWithMostWords.Title);
            Assert.AreEqual(6, response.SongWithMostWords.WordCount.WordCount);
        }

        [Test]
        public async Task LyricsCounter_GetLyricsAverages_ReturnsCorrectSongWithFewestWords()
        {
            var artistName = "test";
            var songTitles = new List<string> { "song1", "song2" };
            var song1Lyrics = GenerateLyrics(4, 2);
            var song2Lyrics = GenerateLyrics(6, 3);


            var artistSongTitles = new ArtistSongTitles { Artist = artistName, SongTitles = songTitles };

            _songRetrieverMock.Setup(sr => sr.ArtistSongTitles(artistName)).Returns(artistSongTitles);

            _lyricsRetrieverMock.Setup(lr => lr.GetLyrics(artistName, "song1"))
                .Returns(Task.FromResult(new SongLyrics("song1", song1Lyrics)));

            _lyricsRetrieverMock.Setup(lr => lr.GetLyrics(artistName, "song2"))
                .Returns(Task.FromResult(new SongLyrics("song2", song2Lyrics)));

            var response = await _sut.GetLyricsAverages(artistName);

            Assert.AreEqual("song1", response.SongWithFewestWords.Title);
            Assert.AreEqual(4, response.SongWithFewestWords.WordCount.WordCount);
        }

        private string GenerateLyrics(int totalWords, int distinctWords = 0)
        {
            if (distinctWords > totalWords)
            {
                throw new ArgumentException("Can not have more distinct words than total words");
            }
            List<string> words = new List<string>();
            var word = "word";
            for (var i = 0; i < totalWords; i++)
            {
                if (distinctWords > 0)
                {
                    word = $"word{i}";
                    distinctWords--;
                }
                words.Add(word);
            }
            return String.Join(" ", words);
        }

    }
}
