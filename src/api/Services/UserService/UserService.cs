using System;
using System.Threading;
using System.Threading.Tasks;
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

        public async Task<UserResponse> CreateUserAsync(UserCreate userCreate, CancellationToken cancellationToken)
        {
            var existingUser = await _userRepository.GetUserByUsernameAsync(userCreate.Username, cancellationToken);
            if (existingUser != null)
                throw new ConflictException("An account with the same username already exists");
            var user = new User(userCreate);
            (user.PasswordHash, user.PasswordSalt) = _passwordService.GetPasswordHash(userCreate.Password);
            var userId = await _userRepository.CreateUserAsync(user, cancellationToken);
            return await this.GetUserAsync(userId, cancellationToken);
        }

        public async Task<UserResponse> UpdateUserAsync(long userId, UserUpdate patch, CancellationToken cancellationToken)
        {
            var user = await this.GetUserAsync(userId, cancellationToken);
            if (user == null)
                throw new NotFoundException($"User {userId} was not found");

            int result;
            if (!string.IsNullOrWhiteSpace(patch.Password))
            {
                var fullUser = await _userRepository.GetUserByUsernameAsync(user.Username, cancellationToken);
                (fullUser.PasswordHash, fullUser.PasswordSalt) = _passwordService.GetPasswordHash(patch.Password);
                fullUser.Update(patch);
                result = await _userRepository.UpdateUserAsync(fullUser, cancellationToken);
            }
            else
            {
                user.Update(patch);
                result = await _userRepository.UpdateUserAsync(user, cancellationToken);
            }
            if (result < 1)
                throw new Exception($"Error updating user {userId}");

            return await this.GetUserAsync(userId, cancellationToken);
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

        public async Task<UserResponse> GetUserAsync(long userId, CancellationToken cancellationToken)
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
                return _tokenProoviderService.IssueToken(user.Id, credentials.RememberMe);
            }
            return null;
        }
    }
}
