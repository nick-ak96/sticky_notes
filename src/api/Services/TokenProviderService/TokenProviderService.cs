using System;
using System.Security.Cryptography;
using System.Text;
using api.Models;
using api.Models.Options;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;

namespace api.Services
{
    public class TokenProviderService : ITokenProviderService
    {
        private readonly byte[] _privateKey;
        private readonly ILogger _logger;

        public TokenProviderService(IOptions<AuthenticationKey> options, ILoggerFactory loggerFactory)
        {
            _privateKey = Encoding.UTF8.GetBytes(options.Value.Key);
            _logger = loggerFactory.CreateLogger<TokenProviderService>();
        }

        public string IssueToken(long userId, bool isPersistent)
        {
            var header = JsonConvert.SerializeObject(new
            {
                alg = "HS256",
                typ = "JWT"
            });
            var payload = JsonConvert.SerializeObject(new AuthenticationTokenPayload
            {
                usr = userId,
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

        public long? ValidateToken(string token)
        {
            var parts = token.Split('.');
            if (parts != null && parts.Length.Equals(3))
            {
                var computedSignature = SignToken(Encoding.UTF8.GetBytes($"{parts[0]}.{parts[1]}"));
                if (string.Equals(computedSignature, parts[2], StringComparison.Ordinal))
                {
                    // retrieve token payload
                    var plainPayload = Encoding.UTF8.GetString(Convert.FromBase64String(parts[1]));
                    var tokenPayload = JsonConvert.DeserializeObject<AuthenticationTokenPayload>(plainPayload);
                    if (tokenPayload.exp > DateTime.UtcNow)
                        return tokenPayload.usr;
                    else
                        _logger.LogDebug("Token has expired");
                }
                else
                    _logger.LogDebug("Signatures do not match");
            }
            else
                _logger.LogDebug("Invalid token format");
            return null;
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
