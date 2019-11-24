namespace api.Services
{
    public interface ITokenProviderService
    {
        bool ValidateToken(string token);

        string IssueToken(string username, bool isPersistent);
    }
}
