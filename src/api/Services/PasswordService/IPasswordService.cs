namespace api.Services
{
    public interface IPasswordService
    {
        (string hash, string salt) GetPasswordHash(string plainPassword);
        string GetPasswordHash(string plainPassword, string salt);
    }
}
