using System.ComponentModel.DataAnnotations;

namespace api.Models
{
    public class OrganizationCreate
    {
		[Required]
        public string Name { get; set; }

        public string ProfilePicture { get; set; }
    }
}
