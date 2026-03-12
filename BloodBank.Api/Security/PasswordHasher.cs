using System;
using System.Security.Cryptography;
using System.Text;


namespace BloodBank.Api.Security;

public static class PasswordHasher
    {
        // Tune to your latency budget: 310_000+ up to ~600_000 (OWASP guidance as of 2025/26).
        private const int DefaultIterations = 310_000; // consider increasing if acceptable
        private const int SaltSizeBytes = 32;
        private const int KeySizeBytes  = 32; // 256-bit

        public static string HashPassword(string password, int? iterationsOverride = null)
        {
            if (string.IsNullOrWhiteSpace(password))
                throw new ArgumentException("Password cannot be empty.", nameof(password));

            var iterations = iterationsOverride ?? DefaultIterations;
            var salt = RandomNumberGenerator.GetBytes(SaltSizeBytes);

            // .NET one-shot PBKDF2 API (forward compatible with .NET 10 guidance)
            var hash = Rfc2898DeriveBytes.Pbkdf2(
                password: password,
                salt: salt,
                iterations: iterations,
                hashAlgorithm: HashAlgorithmName.SHA256,
                outputLength: KeySizeBytes); // 32 bytes

            return $"PBKDF2-SHA256${iterations}${Convert.ToBase64String(salt)}${Convert.ToBase64String(hash)}";
        }

        public static bool Verify(string password, string stored)
        {
            // Expected: algo$iter$salt$hash
            var parts = stored?.Split('$', StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries);
            if (parts is null || parts.Length != 4) return false;
            if (!parts[0].Equals("PBKDF2-SHA256", StringComparison.OrdinalIgnoreCase)) return false;
            if (!int.TryParse(parts[1], out var iterations)) return false;

            var salt = Convert.FromBase64String(parts[2]);
            var expectedHash = Convert.FromBase64String(parts[3]);

            var actualHash = Rfc2898DeriveBytes.Pbkdf2(
                password,
                salt,
                iterations,
                HashAlgorithmName.SHA256,
                expectedHash.Length);

            // constant-time comparison
            return CryptographicOperations.FixedTimeEquals(actualHash, expectedHash);
        }
    }
