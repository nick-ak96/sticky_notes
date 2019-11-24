using System.Threading;
using System.Threading.Tasks;
using api.Controllers.Models;
using api.Models;

namespace api.Services
{
    public interface IUserService
    {
        Task<string> Login(UserCredentials credentials, CancellationToken cancellationToken);

        Task<User> CreateUserAsync(UserDetails userDetails, CancellationToken cancellationToken);

        Task<User> GetUserAsync(long userId, CancellationToken cancellationToken);

        Task<User> UpdateUserAsync(User user, CancellationToken cancellationToken);

        Task DeleteUserAsync(long userId, CancellationToken cancellationToken);
    }
}
