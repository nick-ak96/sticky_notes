using System;

namespace api.Models
{
    public class AuthenticationTokenPayload
    {
        public string usr { get; set; }
        public DateTime iss { get; set; }
        public DateTime exp { get; set; }
    }
}
