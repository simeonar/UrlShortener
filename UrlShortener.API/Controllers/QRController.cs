namespace UrlShortener.API.Controllers
{
    using System.Drawing;
    using System.Drawing.Imaging;
    using System.IO;
    using System.Threading.Tasks;
    using Microsoft.AspNetCore.Mvc;
    using QRCoder;
    using UrlShortener.Core.Services;

    /// <summary>
    /// Controller for generating and caching QR codes for short URLs.
    /// </summary>
    [ApiController]
    [Route("api/qr")]
    public class QRController : ControllerBase
    {
        private readonly IQRCodeCache qrCodeCache;

        /// <summary>
        /// Initializes a new instance of the <see cref="QRController"/> class.
        /// </summary>
        /// <param name="qrCodeCache">QR code cache service.</param>
        public QRController(IQRCodeCache qrCodeCache)
        {
            this.qrCodeCache = qrCodeCache;
        }

        /// <summary>
        /// Returns a QR code for the given short code.
        /// </summary>
        /// <param name="shortCode">Short URL code.</param>
        /// <param name="size">Size of the QR code.</param>
        /// <param name="format">Format: png or svg.</param>
        /// <returns>QR code image file.</returns>
        [HttpGet("{shortCode}")]
        public async Task<IActionResult> GetQr(string shortCode, int size = 200, string format = "png")
        {
            var url = $"https://localhost:7006/{shortCode}";
            var cacheKey = $"{shortCode}:{size}:{format.ToLower()}";
            if (format.ToLower() == "svg")
            {
                var svgBytes = await this.qrCodeCache.GetOrAddAsync(cacheKey, async () =>
                {
                    await Task.Yield();
                    using var qrGenerator = new QRCodeGenerator();
                    using var qrData = qrGenerator.CreateQrCode(url, QRCodeGenerator.ECCLevel.Q);
                    var svgQr = new SvgQRCode(qrData);
                    var svg = svgQr.GetGraphic(size);
                    return System.Text.Encoding.UTF8.GetBytes(svg);
                });
                return this.File(svgBytes, "image/svg+xml");
            }
            else
            {
                var pngBytes = await this.qrCodeCache.GetOrAddAsync(cacheKey, async () =>
                {
                    await Task.Yield();
                    using var qrGenerator = new QRCodeGenerator();
                    using var qrData = qrGenerator.CreateQrCode(url, QRCodeGenerator.ECCLevel.Q);
                    using var qrCode = new BitmapByteQRCode(qrData);
                    return qrCode.GetGraphic(size / 10 > 0 ? size / 10 : 20);
                });
                return this.File(pngBytes, "image/png");
            }
        }
    }
}
