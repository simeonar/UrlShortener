using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using UrlShortener.Core.Entities;
using UrlShortener.Core.Repositories;

namespace UrlShortener.API.Controllers
{
    [ApiController]
    [Route("api/urls")]
    public class UrlsQueryController : ControllerBase
    {
        private readonly IShortenedUrlRepository _repository;

        public UrlsQueryController(IShortenedUrlRepository repository)
        {
            _repository = repository;
        }

        // Removed duplicate GetByShortCode to avoid route conflict
    }
}
