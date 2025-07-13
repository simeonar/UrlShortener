using System.Threading.Tasks;
using Xunit;
using Moq;
using UrlShortener.Core.Entities;
using UrlShortener.Core.Services;

namespace UrlShortener.Core.Tests.Services
{
    public class ShortCodeUniquenessTests
    {
        [Fact]
        public async Task GenerateShortCodeAsync_EnsuresUniqueness()
        {
            var uniquenessChecker = new Mock<IShortCodeUniquenessChecker>();
            // Первый код не уникален, второй уникален
            uniquenessChecker.SetupSequence(x => x.IsShortCodeUniqueAsync(It.IsAny<string>()))
                .ReturnsAsync(false)
                .ReturnsAsync(true);
            var generator = new ShortCodeGenerator(uniquenessChecker.Object);
            var entity = new ShortenedUrl { Id = System.Guid.NewGuid() };
            var code = await generator.GenerateShortCodeAsync(entity);
            Assert.False(string.IsNullOrWhiteSpace(code));
            uniquenessChecker.Verify(x => x.IsShortCodeUniqueAsync(It.IsAny<string>()), Times.AtLeast(2));
        }
    }
}
