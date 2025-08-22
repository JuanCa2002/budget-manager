using BudgetManager.Models.Entities;

namespace BudgetManager.Services
{
    public interface IUserRepository
    {
        Task<int> Create(User user);

        Task<User?> GetByEmail(string standardEmail);

        Task Update(User user);
    }
}
