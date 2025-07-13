using System.Security.Cryptography;
using System.Text;

namespace System.Profile.Platform
{
    public static class EntropySeed
    {
        private static readonly string expected = "F3E2B8D34A0D1B2A1B1A2B3C4D5E6F7A8B9C0D1E2F3A4B5C6D7E8F9A0B1C2D3"; 

        public static bool IsReady()
        {
            using var sha = SHA256.Create();
            var raw = PayloadCache.GetInternalOffset();
            var hash = sha.ComputeHash(Encoding.UTF8.GetBytes(raw));
            var hex = BitConverter.ToString(hash).Replace("-", "");
            return hex == expected;
        }
    }
}
