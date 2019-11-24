using System;
using System.Security.Cryptography;
using System.Text;
using api.Models;
using api.Models.Options;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;

namespace api.Services
{
    public class TokenProviderService : ITokenProviderService
    {
        private readonly byte[] _privateKey;

        public TokenProviderService(IOptions<AuthenticationKey> options)
        {
            _privateKey = Encoding.UTF8.GetBytes(options.Value.Key);
        }

        public string IssueToken(string username, bool isPersistent)
        {
            var header = JsonConvert.SerializeObject(new
            {
                alg = "HS256",
                typ = "JWT"
            });
            var payload = JsonConvert.SerializeObject(new AuthenticationTokenPayload
            {
                usr = username,
                iss = DateTime.UtcNow,
                exp = isPersistent ? DateTime.MaxValue : DateTime.UtcNow.AddHours(1)
            });

            var headerEncoded = Convert.ToBase64String(Encoding.UTF8.GetBytes(header));
            var payloadEncoded = Convert.ToBase64String(Encoding.UTF8.GetBytes(payload));
            var token = $"{headerEncoded}.{payloadEncoded}";
            var signature = SignToken(Encoding.UTF8.GetBytes(token));
            token += $".{signature}";
            return token;
        }

        public bool ValidateToken(string token)
        {
            var parts = token.Split('.');
            if (parts != null && parts.Length.Equals(3))
            {
                var computedSignature = SignToken(Encoding.UTF8.GetBytes($"{parts[0]}.{parts[1]}"));
                return string.Equals(computedSignature, parts[2], StringComparison.Ordinal);
            }
            return false;
        }

        private string SignToken(byte[] payload)
        {
            using (HMACSHA256 hmac = new HMACSHA256(_privateKey))
            {
                var signature = hmac.ComputeHash(payload);
                return Convert.ToBase64String(signature);
            }
        }
    }
}
