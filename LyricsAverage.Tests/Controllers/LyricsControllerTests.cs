using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using LyricsAverage.Configuration;
using LyricsAverage.Controllers;
using LyricsAverage.Models;
using LyricsAverage.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Moq;
using NUnit.Framework;

namespace LyricsAverage.Tests.Controllers
{
    public class LyricsControllerTests
    {
        private LyricsController _sut;
        private LyricsAverageConfig _config;
        private Mock<IOptions<LyricsAverageConfig>> _optionsMock;
        private Mock<ILyricsCounter> _lyricsCounterMock;

        [SetUp]
        public void Init()
        {
            _lyricsCounterMock = new Mock<ILyricsCounter>();
            _config = new LyricsAverageConfig() {RequestTimeoutSeconds = 30};
            _optionsMock = new Mock<IOptions<LyricsAverageConfig>>();
            _optionsMock.Setup(om => om.Value).Returns(_config);
            _sut = new LyricsController(_lyricsCounterMock.Object, _optionsMock.Object);
        }


        [Test]
        public void LyricsController_PassedNullLyricsCounter_ThrowsNullArgumentException()
        {
            Assert.Throws<ArgumentNullException>(() => _ = new LyricsController(null, _optionsMock.Object));
        }

        [Test]
        public void LyricsController_PassedNullConfigOptions_ThrowsNullArgumentException()
        {
            Assert.Throws<ArgumentNullException>(() => _ = new LyricsController(_lyricsCounterMock.Object, null));
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
