using System;

namespace api.Models
{
    public class AuthenticationTokenPayload
    {
        public long usr { get; set; }
        public DateTime iss { get; set; }
        public DateTime exp { get; set; }
    }
}
