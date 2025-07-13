using FluentValidation.TestHelper;
using UrlShortener.API.Validators;
using UrlShortener.API.Controllers;
using Xunit;

namespace UrlShortener.API.Tests.Validators
{
    public class ShortenUrlRequestValidatorTests
    {
        private readonly ShortenUrlRequestValidator _validator = new ShortenUrlRequestValidator();

        [Theory]
        [InlineData("http://example.com")]
        [InlineData("https://example.com/path?query=1")]
        public void OriginalUrl_ValidUrls_Pass(string url)
        {
            var model = new ShortenUrlRequest { OriginalUrl = url };
            var result = _validator.TestValidate(model);
            result.ShouldNotHaveValidationErrorFor(x => x.OriginalUrl);
        }

        [Theory]
        [InlineData("")]
        [InlineData("ftp://example.com")]
        [InlineData("not_a_url")] 
        public void OriginalUrl_InvalidUrls_Fail(string url)
        {
            var model = new ShortenUrlRequest { OriginalUrl = url };
            var result = _validator.TestValidate(model);
            result.ShouldHaveValidationErrorFor(x => x.OriginalUrl);
        }

        [Theory]
        [InlineData("my-alias_123")]
        [InlineData(null)]
        public void CustomAlias_Valid_Pass(string? alias)
        {
            var model = new ShortenUrlRequest { OriginalUrl = "https://ok.com", CustomAlias = alias };
            var result = _validator.TestValidate(model);
            result.ShouldNotHaveValidationErrorFor(x => x.CustomAlias);
        }

        [Theory]
        [InlineData("with space")] 
        [InlineData("!@#bad")] 
        [InlineData("thisaliasiswaytoolongtobeacceptedbythesystem1234567890")] 
        public void CustomAlias_Invalid_Fail(string alias)
        {
            var model = new ShortenUrlRequest { OriginalUrl = "https://ok.com", CustomAlias = alias };
            var result = _validator.TestValidate(model);
            result.ShouldHaveValidationErrorFor(x => x.CustomAlias);
        }
    }
}
