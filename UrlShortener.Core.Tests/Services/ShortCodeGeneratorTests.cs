using System.Threading.Tasks;
using Xunit;
using Moq;
using UrlShortener.Core.Entities;
using UrlShortener.Core.Services;

namespace UrlShortener.Core.Tests.Services
{
    public class ShortCodeGeneratorTests
    {
        [Fact]
        public async Task GenerateShortCodeAsync_Base62Encoding_UniqueCodeReturned()
        {
            // Arrange
            var uniquenessChecker = new Mock<IShortCodeUniquenessChecker>();
            uniquenessChecker.Setup(x => x.IsShortCodeUniqueAsync(It.IsAny<string>())).ReturnsAsync(true);
            var generator = new ShortCodeGenerator(uniquenessChecker.Object);
            var entity = new ShortenedUrl { Id = Guid.NewGuid() };

            // Act
            var code = await generator.GenerateShortCodeAsync(entity);

            // Assert
            Assert.False(string.IsNullOrWhiteSpace(code));
            Assert.True(code.Length >= 6); // DefaultLength
        }

        [Fact]
        public async Task GenerateShortCodeAsync_CustomAlias_ReturnsAliasIfUnique()
        {
            // Arrange
            var uniquenessChecker = new Mock<IShortCodeUniquenessChecker>();
            uniquenessChecker.Setup(x => x.IsShortCodeUniqueAsync("custom123")).ReturnsAsync(true);
            var generator = new ShortCodeGenerator(uniquenessChecker.Object);
            var entity = new ShortenedUrl { Id = Guid.NewGuid(), CustomAlias = "custom123" };

            // Act
            var code = await generator.GenerateShortCodeAsync(entity);

            // Assert
            Assert.Equal("custom123", code);
        }

        [Fact]
        public async Task GenerateShortCodeAsync_CustomAlias_NotUnique_Throws()
        {
            // Arrange
            var uniquenessChecker = new Mock<IShortCodeUniquenessChecker>();
            uniquenessChecker.Setup(x => x.IsShortCodeUniqueAsync("taken")).ReturnsAsync(false);
            var generator = new ShortCodeGenerator(uniquenessChecker.Object);
            var entity = new ShortenedUrl { Id = Guid.NewGuid(), CustomAlias = "taken" };

            // Act & Assert
            await Assert.ThrowsAsync<InvalidOperationException>(() => generator.GenerateShortCodeAsync(entity));
        }
    }
}
