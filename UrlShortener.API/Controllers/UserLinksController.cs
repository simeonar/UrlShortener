using Microsoft.AspNetCore.Mvc;
using UrlShortener.Core.Repositories;

namespace UrlShortener.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class UserLinksController : ControllerBase
    {
        private readonly IShortenedUrlRepository _urlRepository;
        private readonly IUserRepository _userRepository;
        public UserLinksController(IShortenedUrlRepository urlRepository, IUserRepository userRepository)
        {
            _urlRepository = urlRepository;
            _userRepository = userRepository;
        }

        [HttpGet]
        [HttpGet]
        public async Task<IActionResult> GetMyLinks([FromHeader(Name = "X-Api-Key")] string apiKey)
        {
            var user = _userRepository.GetByApiKey(apiKey);
            if (user == null)
                return Unauthorized();
            var allLinks = await _urlRepository.GetAllAsync();
            var links = allLinks.Where(u => u.OwnerUserName == user.UserName);
            return Ok(links);
        }
        [HttpDelete("{shortCode}")]
        public async Task<IActionResult> Delete(string shortCode, [FromHeader(Name = "X-Api-Key")] string apiKey)
        {
            var user = _userRepository.GetByApiKey(apiKey);
            if (user == null)
                return Unauthorized();
            var allLinks = await _urlRepository.GetAllAsync();
            var link = allLinks.FirstOrDefault(u => u.ShortCode == shortCode && u.OwnerUserName == user.UserName);
            if (link == null)
                return NotFound();
            await _urlRepository.DeleteAsync(link.Id);
            return NoContent();
        }
    }
}
