using BudgetManager.Models.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.IdentityModel.Tokens;
using System.Runtime.ConstrainedExecution;

namespace BudgetManager.Services
{
    public class UserStore : IUserStore<User>, IUserEmailStore<User>,
        IUserPasswordStore<User>
    {
        private readonly IUserRepository _userRepository;
        public UserStore(IUserRepository userRepository)
        {
            _userRepository = userRepository;
        }
        public async Task<IdentityResult> CreateAsync(User user, CancellationToken cancellationToken)
        {
            user.Id = await _userRepository.Create(user);
            return IdentityResult.Success;
        }

        public Task<IdentityResult> DeleteAsync(User user, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }

        public void Dispose()
        {
           
        }

        public async Task<User?> FindByEmailAsync(string normalizedEmail, CancellationToken cancellationToken)
        {
            return await _userRepository.GetByEmail(normalizedEmail);
        }

        public Task<User?> FindByIdAsync(string userId, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }

        public async Task<User?> FindByNameAsync(string normalizedUserName, CancellationToken cancellationToken)
        {
            return await _userRepository.GetByEmail(normalizedUserName);
        }

        public Task<string?> GetEmailAsync(User user, CancellationToken cancellationToken)
        {
            return Task.FromResult(!user.Email.IsNullOrEmpty() ? user.Email: null);
        }

        public Task<bool> GetEmailConfirmedAsync(User user, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }

        public Task<string?> GetNormalizedEmailAsync(User user, CancellationToken cancellationToken)
        {
            return Task.FromResult(!user.StandardEmail.IsNullOrEmpty() ? user.StandardEmail : null);
        }

        public Task<string?> GetNormalizedUserNameAsync(User user, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }

        public Task<string?> GetPasswordHashAsync(User user, CancellationToken cancellationToken)
        {
            return Task.FromResult(!user.PasswordHash.IsNullOrEmpty() ? user.PasswordHash : null);
        }

        public Task<string> GetUserIdAsync(User user, CancellationToken cancellationToken)
        {
            return Task.FromResult(user.Id.ToString());
        }

        public Task<string?> GetUserNameAsync(User user, CancellationToken cancellationToken)
        {
            return Task.FromResult(!user.Email.IsNullOrEmpty() ? user.Email: null);
        }

        public Task<bool> HasPasswordAsync(User user, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }

        public Task SetEmailAsync(User user, string? email, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }

        public Task SetEmailConfirmedAsync(User user, bool confirmed, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }

        public Task SetNormalizedEmailAsync(User user, string? normalizedEmail, CancellationToken cancellationToken)
        {
            user.StandardEmail = normalizedEmail is not null ? normalizedEmail : string.Empty;
            return Task.CompletedTask;
        }

        public Task SetNormalizedUserNameAsync(User user, string? normalizedName, CancellationToken cancellationToken)
        {
            return Task.CompletedTask;
        }

        public Task SetPasswordHashAsync(User user, string? passwordHash, CancellationToken cancellationToken)
        {
            user.PasswordHash = passwordHash is not null ? passwordHash : string.Empty;
            return Task.CompletedTask;
        }

        public Task SetUserNameAsync(User user, string? userName, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }

        public async Task<IdentityResult> UpdateAsync(User user, CancellationToken cancellationToken)
        {
            await _userRepository.Update(user);
            return IdentityResult.Success;
        }
    }
}
