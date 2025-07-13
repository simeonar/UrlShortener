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
            Console.WriteLine($"[DEBUG] Encrypted: '{encrypted}' (Length: {encrypted?.Length})");
            byte[] obfuscated = Convert.FromBase64String(encrypted);
            byte[] decoded = obfuscated.Select(b => (byte)(b ^ 0x4F)).ToArray();
            string expectedHash = Encoding.UTF8.GetString(decoded);

            using var sha = SHA256.Create();
            string input = PayloadCache.GetInternalOffset();
            byte[] actualHash = sha.ComputeHash(Encoding.UTF8.GetBytes(input));
            string actualHex = BitConverter.ToString(actualHash).Replace("-", "");

            return actualHex == expectedHash;
        }
    }
}
