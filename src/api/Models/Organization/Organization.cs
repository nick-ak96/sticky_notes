using System;

namespace api.Models
{
    public class Organization
    {
        public Organization()
        {
        }

        public Organization(OrganizationCreate org, long creator)
        {
            Name = org.Name;
            ProfilePicture = org.ProfilePicture;
            CreatedBy = creator;
            InsertDate = DateTime.UtcNow;
            LastModified = DateTime.UtcNow;
        }

        public void Update(OrganizationUpdate patch)
        {
            Name = patch.Name ?? Name;
            ProfilePicture = patch.ProfilePicture ?? ProfilePicture;
        }

        public long Id { get; set; }

        public string Name { get; set; }

        public string ProfilePicture { get; set; }

        public long CreatedBy { get; set; }

        public DateTime InsertDate { get; set; }

        public DateTime LastModified { get; set; }
    }
}
