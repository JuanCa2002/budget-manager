using BudgetManager.Models.Entities;

namespace BudgetManager.Services
{
    public interface IAccountRepository
    {
        Task Save(Account account);

        Task<IEnumerable<Account>> GetAll(int userId);

        Task Update(Account account);

        Task<Account?> GetById(int id, int userId);

        Task Delete(int id);
    }
}
