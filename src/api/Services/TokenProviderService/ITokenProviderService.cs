namespace api.Services
{
    public interface ITokenProviderService
    {
        long? ValidateToken(string token);

        string IssueToken(long userId, bool isPersistent);
    }
}
