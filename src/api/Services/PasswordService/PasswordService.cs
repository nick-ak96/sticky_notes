using System;
using System.Linq;
using System.Security.Cryptography;
using System.Text;

namespace api.Services
{
    public class PasswordService : IPasswordService
    {
        public PasswordService()
        {
        }

        private byte[] GenerateSalt()
        {
            var salt = new byte[32];
            using (var rand = new RNGCryptoServiceProvider())
            {
                rand.GetNonZeroBytes(salt);
            }
            return salt;
        }

        public (string hash, string salt) GetPasswordHash(string plainPassword)
        {
            var passwordBytes = Encoding.UTF8.GetBytes(plainPassword);
            var saltBytes = this.GenerateSalt();
            var password = passwordBytes.Concat(saltBytes).ToArray();
            using (SHA256 sha = SHA256.Create())
            {
                var hash = sha.ComputeHash(password);
                return (Convert.ToBase64String(hash), Convert.ToBase64String(saltBytes));
            }
        }

        public string GetPasswordHash(string plainPassword, string salt)
        {
            var passwordBytes = Encoding.UTF8.GetBytes(plainPassword);
            var saltBytes = Convert.FromBase64String(salt);
            var password = passwordBytes.Concat(saltBytes).ToArray();
            using (SHA256 sha = SHA256.Create())
            {
                var hash = sha.ComputeHash(password);
                return Convert.ToBase64String(hash);
            }
        }
    }
}
