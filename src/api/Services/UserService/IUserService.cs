using System.Threading;
using System.Threading.Tasks;
using api.Models;

namespace api.Services
{
    public interface IUserService
    {
        Task<string> Login(UserCredentials credentials, CancellationToken cancellationToken);

        Task<UserResponse> CreateUserAsync(UserCreate userCreate, CancellationToken cancellationToken);

        Task<UserResponse> GetUserAsync(long userId, CancellationToken cancellationToken);

        Task<UserResponse> UpdateUserAsync(long userId, UserUpdate patch, CancellationToken cancellationToken);

        Task DeleteUserAsync(long userId, CancellationToken cancellationToken);
    }
}
