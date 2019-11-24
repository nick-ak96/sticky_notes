using System.Threading;
using System.Threading.Tasks;
using api.Models;

namespace api.Repositories
{
    public interface IUserRepository
    {
        Task<long> CreateUserAsync(User user, CancellationToken cancellationToken);
        Task<User> GetUserAsync(long userId, CancellationToken cancellationToken);
        Task<User> GetUserByUsernameAsync(string username, CancellationToken cancellationToken);
        Task<int> UpdateUserAsync(User user, CancellationToken cancellationToken);
        Task<int> DeleteUserAsync(long userId, CancellationToken cancellationToken);
    }
}
