using System.Security.Cryptography;

namespace GamingNProgramming.Common
{
    public class PasswordGenerator
    {
        public byte[] Salt { get; set; }
        private void GenerateSalt()
        {
            var saltBytes = new byte[64];

            using (var provider = new RNGCryptoServiceProvider())
            {
                provider.GetNonZeroBytes(saltBytes);
            }

            this.Salt = saltBytes;
        }

        private static string HashPassword(string password, byte[] salt, int nIterations, int nHash)
        {
            using (var rfc2898DeriveBytes = new Rfc2898DeriveBytes(password, salt, nIterations))
            {
                return Convert.ToBase64String(rfc2898DeriveBytes.GetBytes(nHash));
            }
        }

        public string GenerateHashedPassword(string password)
        {
            GenerateSalt();
            return HashPassword(password, this.Salt, 10101, 64);
        }

        public string GenerateHashedPassword(string password, byte[] salt)
        {
            return HashPassword(password, salt, 10101, 64);
        }
    }
}