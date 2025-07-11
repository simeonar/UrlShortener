using System;
using System.Text;

namespace UrlShortener.Core.Services
{
    /// <summary>
    /// Provides Base62 encoding and decoding for short code generation.
    /// </summary>
    public static class Base62Encoder
    {
        private const string Alphabet = "0123456789ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz";
        private const int Base = 62;

        public static string Encode(long value)
        {
            if (value < 0) throw new ArgumentOutOfRangeException(nameof(value), "Value must be non-negative.");
            if (value == 0) return Alphabet[0].ToString();
            var sb = new StringBuilder();
            while (value > 0)
            {
                sb.Insert(0, Alphabet[(int)(value % Base)]);
                value /= Base;
            }
            return sb.ToString();
        }

        public static long Decode(string input)
        {
            if (string.IsNullOrEmpty(input)) throw new ArgumentNullException(nameof(input));
            long result = 0;
            foreach (char c in input)
            {
                int index = Alphabet.IndexOf(c);
                if (index == -1) throw new ArgumentException($"Invalid character '{c}' in Base62 string.");
                result = result * Base + index;
            }
            return result;
        }
    }
}
