using System;
using System.Security.Cryptography;

namespace BloodBank.Api.Security
{
    public static class ResetToken
    {
        public static (string RawToken, byte[] Hash, byte[] Salt) Create()
        {
            // Generate 32-byte token -> Base64Url string for URL
            var tokenBytes = RandomNumberGenerator.GetBytes(32);
            var rawToken = Base64UrlEncode(tokenBytes);

            // Salt to harden the hash (optional but recommended)
            var salt = RandomNumberGenerator.GetBytes(16);

            // Hash = SHA256(token || salt)
            using var sha = SHA256.Create();
            var combined = new byte[tokenBytes.Length + salt.Length];
            Buffer.BlockCopy(tokenBytes, 0, combined, 0, tokenBytes.Length);
            Buffer.BlockCopy(salt, 0, combined, tokenBytes.Length, salt.Length);
            var hash = sha.ComputeHash(combined);

            return (rawToken, hash, salt);
        }

        public static byte[] HashFromRaw(string rawToken, byte[] salt)
        {
            var tokenBytes = Base64UrlDecode(rawToken);
            using var sha = SHA256.Create();
            var combined = new byte[tokenBytes.Length + salt.Length];
            Buffer.BlockCopy(tokenBytes, 0, combined, 0, tokenBytes.Length);
            Buffer.BlockCopy(salt, 0, combined, tokenBytes.Length, salt.Length);
            return sha.ComputeHash(combined);
        }

        // --- Helpers: RFC 4648 base64url without padding ---
        private static string Base64UrlEncode(byte[] input)
            => Convert.ToBase64String(input).Replace('+','-').Replace('/','_').TrimEnd('=');
        private static byte[] Base64UrlDecode(string input)
        {
            string s = input.Replace('-', '+').Replace('_', '/');
            switch (s.Length % 4) { case 2: s += "=="; break; case 3: s += "="; break; }
            return Convert.FromBase64String(s);
        }
    }
}