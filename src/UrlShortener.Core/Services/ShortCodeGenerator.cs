using System;
using System.Threading.Tasks;
using UrlShortener.Core.Entities;

namespace UrlShortener.Core.Services
{
    /// <summary>
    /// Default implementation of IShortCodeGenerator using Base62 and uniqueness check.
    /// </summary>
    public class ShortCodeGenerator : IShortCodeGenerator
    {
        private readonly IShortCodeUniquenessChecker _uniquenessChecker;
        private readonly Random _random = new();
        private const int DefaultLength = 6;
        private const int MaxAttempts = 10;

        public ShortCodeGenerator(IShortCodeUniquenessChecker uniquenessChecker)
        {
            _uniquenessChecker = uniquenessChecker;
        }

        public async Task<string> GenerateShortCodeAsync(ShortenedUrl entity)
        {
            // Try to use entity ID for deterministic code, fallback to random
            for (int attempt = 0; attempt < MaxAttempts; attempt++)
            {
                string code = attempt == 0 && entity.Id != Guid.Empty
                    ? Base62Encoder.Encode(BitConverter.ToInt64(entity.Id.ToByteArray(), 0)).PadLeft(DefaultLength, '0')
                    : GenerateRandomCode(DefaultLength);
                if (await _uniquenessChecker.IsShortCodeUniqueAsync(code))
                    return code;
            }
            throw new InvalidOperationException("Failed to generate unique short code after several attempts.");
        }

        private string GenerateRandomCode(int length)
        {
            var buffer = new byte[8];
            _random.NextBytes(buffer);
            long value = Math.Abs(BitConverter.ToInt64(buffer, 0));
            string code = Base62Encoder.Encode(value);
            return code.Length >= length ? code[..length] : code.PadLeft(length, '0');
        }
    }

    /// <summary>
    /// Abstraction for checking short code uniqueness (to be implemented in infrastructure/data layer).
    /// </summary>
    public interface IShortCodeUniquenessChecker
    {
        Task<bool> IsShortCodeUniqueAsync(string code);
    }
}
