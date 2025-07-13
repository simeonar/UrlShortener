using System.Threading.Tasks;
using Xunit;
using Moq;
using UrlShortener.API.Controllers;
using UrlShortener.Core.Services;
using Microsoft.AspNetCore.Mvc;

namespace UrlShortener.API.Tests.Controllers
{
    public class QRControllerTests
    {
        [Fact]
        public async Task GetQr_ReturnsPngFileResult_WhenFormatIsPng()
        {
            // Arrange
            var cacheMock = new Mock<IQRCodeCache>();
            cacheMock.Setup(x => x.GetOrAddAsync(It.IsAny<string>(), It.IsAny<Func<Task<byte[]>>>()))
                .ReturnsAsync(new byte[] { 1, 2, 3 });
            var controller = new QRController(cacheMock.Object);

            // Act
            var result = await controller.GetQr("abc123", 200, "png");

            // Assert
            var fileResult = Assert.IsType<FileContentResult>(result);
            Assert.Equal("image/png", fileResult.ContentType);
            Assert.Equal(new byte[] { 1, 2, 3 }, fileResult.FileContents);
        }

        [Fact]
        public async Task GetQr_ReturnsSvgFileResult_WhenFormatIsSvg()
        {
            // Arrange
            var cacheMock = new Mock<IQRCodeCache>();
            cacheMock.Setup(x => x.GetOrAddAsync(It.IsAny<string>(), It.IsAny<Func<Task<byte[]>>>()))
                .ReturnsAsync(new byte[] { 10, 20, 30 });
            var controller = new QRController(cacheMock.Object);

            // Act
            var result = await controller.GetQr("abc123", 200, "svg");

            // Assert
            var fileResult = Assert.IsType<FileContentResult>(result);
            Assert.Equal("image/svg+xml", fileResult.ContentType);
            Assert.Equal(new byte[] { 10, 20, 30 }, fileResult.FileContents);
        }
    }
}
