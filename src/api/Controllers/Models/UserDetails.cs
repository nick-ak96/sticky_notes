namespace api.Controllers.Models
{
    public class UserDetails : UserCredentials
    {
        public UserDetails()
        {
        }

        public string Name { get; set; }
        public string Surname { get; set; }
    }
}
