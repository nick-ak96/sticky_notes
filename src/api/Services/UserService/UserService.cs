using System;
using System.Threading;
using System.Threading.Tasks;
using api.Controllers.Models;
using api.Models;
using api.Models.Exceptions;
using api.Repositories;

namespace api.Services
{
    public class UserService : IUserService
    {
        private readonly IUserRepository _userRepository;
        private readonly IPasswordService _passwordService;
        private readonly ITokenProviderService _tokenProoviderService;

        public UserService(IUserRepository userRepository, IPasswordService passwordService, ITokenProviderService tokenProoviderService)
        {
            _userRepository = userRepository;
            _passwordService = passwordService;
            _tokenProoviderService = tokenProoviderService;
        }

        public async Task<User> CreateUserAsync(UserDetails userDetails, CancellationToken cancellationToken)
        {
            var existingUser = await _userRepository.GetUserByUsernameAsync(userDetails.Username, cancellationToken);
            if (existingUser != null)
                throw new ConflictException("An account with the same username already exists");
            var user = new User(userDetails);
            (string passwordHash, string salt) = _passwordService.GetPasswordHash(userDetails.Password);
            user.PasswordHash = passwordHash;
            user.PasswordSalt = salt;
            var userId = await _userRepository.CreateUserAsync(user, cancellationToken);
            return await this.GetUserAsync(userId, cancellationToken);
        }

        public async Task<User> UpdateUserAsync(User user, CancellationToken cancellationToken)
        {
            var userToUpdate = await this.GetUserAsync(user.Id, cancellationToken);
            if (userToUpdate == null)
                throw new NotFoundException($"User {user.Id} was not found");

            var result = await _userRepository.UpdateUserAsync(user, cancellationToken);
            if (result < 1)
                throw new Exception($"Error updating user {user.Id}");

            var newUser = await this.GetUserAsync(user.Id, cancellationToken);
            return newUser;
        }

        public async Task DeleteUserAsync(long userId, CancellationToken cancellationToken)
        {
            var user = await this.GetUserAsync(userId, cancellationToken);
            if (user == null)
                throw new NotFoundException($"User {userId} was not found");

            var result = await _userRepository.DeleteUserAsync(userId, cancellationToken);
            if (result < 1)
                throw new Exception($"Error deleting user {userId}");
        }

        public async Task<User> GetUserAsync(long userId, CancellationToken cancellationToken)
        {
            var user = await _userRepository.GetUserAsync(userId, cancellationToken);
            if (user == null)
                throw new NotFoundException($"User {userId} was not found");
            return user;
        }

        public async Task<string> Login(UserCredentials credentials, CancellationToken cancellationToken)
        {
            var user = await _userRepository.GetUserByUsernameAsync(credentials.Username, cancellationToken);
            if (user != null && user.PasswordHash.Equals(_passwordService.GetPasswordHash(credentials.Password, user.PasswordSalt)))
            {
                return _tokenProoviderService.IssueToken(user.Username, credentials.RememberMe);
            }
            return null;
        }
    }
}
