using System.Threading.Tasks;
using Xunit;
using Moq;
using Microsoft.AspNetCore.Mvc;
using UrlShortener.API.Controllers;
using UrlShortener.Core.Repositories;
using UrlShortener.Core.Entities;

namespace UrlShortener.API.Tests.Controllers
{
    public class StatsControllerTests
    {
        [Fact]
        public async Task GetStats_ReturnsOkWithStats()
        {
            var urlRepo = new Mock<IShortenedUrlRepository>();
            urlRepo.Setup(x => x.GetByShortCodeAsync("abc")).ReturnsAsync(new ShortenedUrl {
                ShortCode = "abc", ClicksCount = 5, CreatedAt = new System.DateTime(2025,7,10)
            });
            var clickRepo = new Mock<IClickStatisticRepository>();
            var controller = new StatsController(clickRepo.Object, urlRepo.Object);

            var result = await controller.GetStats("abc");
            var ok = Assert.IsType<OkObjectResult>(result);
            Assert.Contains("totalClicks", ok.Value.ToString());
        }

        [Fact]
        public async Task GetTop_ReturnsCountriesAndBrowsers()
        {
            var urlRepo = new Mock<IShortenedUrlRepository>();
            var clickRepo = new Mock<IClickStatisticRepository>();
            var controller = new StatsController(clickRepo.Object, urlRepo.Object);

            var result = await controller.GetTop("abc");
            var ok = Assert.IsType<OkObjectResult>(result);
            Assert.Contains("countries", ok.Value.ToString());
            Assert.Contains("browsers", ok.Value.ToString());
        }

        [Fact]
        public async Task GetSources_ReturnsSources()
        {
            var urlRepo = new Mock<IShortenedUrlRepository>();
            var clickRepo = new Mock<IClickStatisticRepository>();
            var controller = new StatsController(clickRepo.Object, urlRepo.Object);

            var result = await controller.GetSources("abc");
            var ok = Assert.IsType<OkObjectResult>(result);
            Assert.Contains("google.com", ok.Value.ToString());
        }
    }
}
