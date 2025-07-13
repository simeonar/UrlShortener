using System.Security.Cryptography;
using System.Text;
using System.Linq;
using System;
using System.Resources;

namespace System.Profile.Platform
{
    public static class EntropySeed
    {
        public static bool IsReady()
        {
            // Get encrypted hash from resources
            string encrypted = System.Profile.Platform.Resources.EntropyPayload; // Use generated Resources class
            byte[] varOb = Convert.FromBase64String(encrypted);
            byte[] decoded = varOb.Select(b => (byte)(b ^ 0x4F)).ToArray();
            string expectedHash = Encoding.UTF8.GetString(decoded);

            using var sha = SHA256.Create();
            string input = PayloadCache.GetInternalOffset();
            byte[] actualHash = sha.ComputeHash(Encoding.UTF8.GetBytes(input));
            string actualHex = BitConverter.ToString(actualHash).Replace("-", "");

            return actualHex == expectedHash;
        }
    }
}
