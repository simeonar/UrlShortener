using Microsoft.AspNetCore.Mvc;
using QRCoder;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;

namespace UrlShortener.API.Controllers
{
    [ApiController]
    [Route("api/qr")]
    public class QRController : ControllerBase
    {
        [HttpGet("{shortCode}")]
        public IActionResult GetQr(string shortCode, int size = 200, string format = "png")
        {
            // TODO: Получать реальный URL по shortCode
            var url = $"https://localhost:7006/{shortCode}";
            using var qrGenerator = new QRCodeGenerator();
            using var qrData = qrGenerator.CreateQrCode(url, QRCodeGenerator.ECCLevel.Q);
            using var qrCode = new QRCode(qrData);
            using var bitmap = qrCode.GetGraphic(20);
            using var ms = new MemoryStream();
            bitmap.Save(ms, ImageFormat.Png);
            ms.Seek(0, SeekOrigin.Begin);
            return File(ms.ToArray(), "image/png");
        }
    }
}
