using Microsoft.AspNetCore.Mvc;
using QRCoder;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Threading.Tasks;
using UrlShortener.Core.Services;

namespace UrlShortener.API.Controllers
{
    [ApiController]
    [Route("api/qr")]
    public class QRController : ControllerBase
    {
        private readonly IQRCodeCache _qrCodeCache;
        public QRController(IQRCodeCache qrCodeCache)
        {
            _qrCodeCache = qrCodeCache;
        }

        [HttpGet("{shortCode}")]
        public async Task<IActionResult> GetQr(string shortCode, int size = 200, string format = "png")
        {
            var url = $"https://localhost:7006/{shortCode}";
            var cacheKey = $"{shortCode}:{size}:{format.ToLower()}";
            if (format.ToLower() == "svg")
            {
                var svgBytes = await _qrCodeCache.GetOrAddAsync(cacheKey, async () =>
                {
                    using var qrGenerator = new QRCodeGenerator();
                    using var qrData = qrGenerator.CreateQrCode(url, QRCodeGenerator.ECCLevel.Q);
                    var svgQr = new SvgQRCode(qrData);
                    var svg = svgQr.GetGraphic(size);
                    return System.Text.Encoding.UTF8.GetBytes(svg);
                });
                return File(svgBytes, "image/svg+xml");
            }
            else
            {
                var pngBytes = await _qrCodeCache.GetOrAddAsync(cacheKey, async () =>
                {
                    using var qrGenerator = new QRCodeGenerator();
                    using var qrData = qrGenerator.CreateQrCode(url, QRCodeGenerator.ECCLevel.Q);
                    using var qrCode = new QRCode(qrData);
                    using var bitmap = qrCode.GetGraphic(size / 10 > 0 ? size / 10 : 20);
                    using var ms = new MemoryStream();
                    bitmap.Save(ms, ImageFormat.Png);
                    return ms.ToArray();
                });
                return File(pngBytes, "image/png");
            }
        }
    }
}
