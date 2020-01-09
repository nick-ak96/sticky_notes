using System;

namespace api.Models
{
    public class User
    {
        public User() { }

        public User(UserCreate userDetails)
        {
            Username = userDetails.Username;
            Name = userDetails.Name;
            Surname = userDetails.Surname;
            Language = userDetails.Language;
            ProfilePicture = userDetails.ProfilePicture;
            InsertDate = DateTime.UtcNow;
            LastModified = DateTime.UtcNow;
        }

        public void Update(UserUpdate patch)
        {
            Language = patch.Language ?? Language;
            Name = patch.Name ?? Name;
            Surname = patch.Surname ?? Surname;
            ProfilePicture = patch.ProfilePicture ?? ProfilePicture;
            LastModified = DateTime.UtcNow;
        }

        public long Id { get; set; }

        public string Language { get; set; }

        public string Username { get; }

        public string Name { get; set; }

        public string Surname { get; set; }

        public string ProfilePicture { get; set; }

        public string PasswordHash { get; set; }

        public string PasswordSalt { get; set; }

        public DateTime InsertDate { get; set; }

        public DateTime LastModified { get; set; }
    }
}
