using Logic.Interfaces;
using System.Security.Cryptography;

namespace Logic.Services
{
    public class PasswordHasher : IPasswordHasher
    {
        private const int SaltSize = 16; // 128 bit
        private const int KeySize = 32; // 256 bit
        private const int Iterations = 100_000;

        public string HashPassword(string password)
        {
            if (string.IsNullOrWhiteSpace(password))
                throw new ArgumentException("Password cannot be empty.");

            using var algorithm = new Rfc2898DeriveBytes(
                password,
                SaltSize,
                Iterations,
                HashAlgorithmName.SHA256);

            var salt = Convert.ToBase64String(algorithm.Salt);
            var key = Convert.ToBase64String(algorithm.GetBytes(KeySize));

            // Format: iterations.salt.hash
            return $"{Iterations}.{salt}.{key}";
        }

        public bool VerifyPassword(string password, string passwordHash)
        {
            if (string.IsNullOrWhiteSpace(passwordHash))
                return false;

            var parts = passwordHash.Split('.');
            if (parts.Length != 3)
                return false;

            var iterations = int.Parse(parts[0]);
            var salt = Convert.FromBase64String(parts[1]);
            var expectedKey = Convert.FromBase64String(parts[2]);

            using var algorithm = new Rfc2898DeriveBytes(
                password,
                salt,
                iterations,
                HashAlgorithmName.SHA256);

            var actualKey = algorithm.GetBytes(KeySize);

            // Constant-time comparison (important)
            return CryptographicOperations.FixedTimeEquals(actualKey, expectedKey);
        }
    }
}
