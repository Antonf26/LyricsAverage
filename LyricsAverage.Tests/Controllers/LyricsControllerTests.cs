using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using LyricsAverage.Controllers;
using LyricsAverage.Models;
using LyricsAverage.Services;
using Microsoft.AspNetCore.Mvc;
using Moq;
using NUnit.Framework;

namespace LyricsAverage.Tests.Controllers
{
    public class LyricsControllerTests
    {
        private LyricsController _sut;
        private Mock<ILyricsCounter> _lyricsCounterMock;

        [SetUp]
        public void Init()
        {
            _lyricsCounterMock = new Mock<ILyricsCounter>();
            _sut = new LyricsController(_lyricsCounterMock.Object);
        }


        [Test]
        public void LyricsController_PassedNullLyricsCounter_ThrowsNullArgumentException()
        {
            Assert.Throws<ArgumentNullException>(() => _ = new LyricsController(null));
        }

        [Test]
        public async Task LyricsController_Artist_CallsLyricsCounter()
        {
            var artistName = "Madonna";
            _ = await _sut.Artist(artistName);
            _lyricsCounterMock.Verify(lc => lc.GetLyricsAverages(artistName, It.IsAny<CancellationToken>()), Times.Once);
        }

        [Test]
        public async Task LyricsController_GetsResult_ReturnsViewWithModelReturned()
        {
            var artistName = "Madonna";
            var response = new AverageLyricsResponse();
            _lyricsCounterMock.Setup(lc => lc.GetLyricsAverages(artistName, It.IsAny<CancellationToken>()))
                .Returns(Task.FromResult(response));
            var result = await _sut.Artist(artistName) as ViewResult;
            Assert.NotNull(result);
            Assert.AreEqual(response, result.Model);
        }
    }
}
