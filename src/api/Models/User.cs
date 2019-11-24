using System;
using api.Controllers.Models;

namespace api.Models
{
    public class User
    {
        public User() { }

        public User(UserDetails userDetails)
        {
            Username = userDetails.Username;
            Name = userDetails.Name;
            Surname = userDetails.Surname;
            InsertDate = DateTime.UtcNow;
            LastModified = DateTime.UtcNow;
        }

        public long Id { get; set; }

        public string Username { get; }

        public string Name { get; set; }

        public string Surname { get; set; }

        public string PasswordHash { get; set; }

        public string PasswordSalt { get; set; }

        public DateTime InsertDate { get; set; }

        public DateTime LastModified { get; set; }
    }
}
