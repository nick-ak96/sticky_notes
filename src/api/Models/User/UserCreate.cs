using System.ComponentModel.DataAnnotations;

namespace api.Models
{
    public class UserCreate : UserCredentials
    {
        public UserCreate()
        {
        }

        [Required]
        public string Name { get; set; }

        [Required]
        public string Surname { get; set; }

        [Required]
        public string Language { get; set; }

        public string ProfilePicture { get; set; }
    }
}
