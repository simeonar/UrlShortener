using FluentValidation;
using UrlShortener.API.Controllers;
using System.Text.RegularExpressions;

namespace UrlShortener.API.Validators
{
    public class ShortenUrlRequestValidator : AbstractValidator<ShortenUrlRequest>
    {
        public ShortenUrlRequestValidator()
        {
            RuleFor(x => x.OriginalUrl)
                .NotEmpty()
                .MaximumLength(2048)
                .Must(BeAValidUrl).WithMessage("Invalid URL format. Only http/https allowed.");

            RuleFor(x => x.CustomAlias)
                .MaximumLength(32)
                .Matches("^[a-zA-Z0-9_-]*$").WithMessage("Custom alias can contain only letters, digits, '-', '_' (no spaces)");
        }

        private bool BeAValidUrl(string url)
        {
            if (string.IsNullOrWhiteSpace(url)) return false;
            if (!Uri.TryCreate(url, UriKind.Absolute, out var uri)) return false;
            return uri.Scheme == Uri.UriSchemeHttp || uri.Scheme == Uri.UriSchemeHttps;
        }
    }
}
