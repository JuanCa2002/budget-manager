using BudgetManager.Models.Entities;

namespace BudgetManager.Services
{
    public interface IAccountTypesRepository
    {
        Task Save(AccountType accountType);
        Task<bool> ExistByNameAndUser(string name, int userId, int id = 0);
        Task<IEnumerable<AccountType>> GetAll(int userId);
        Task Update(AccountType accountType);
        Task<AccountType?> GetById(int id, int userId);
        Task DeleteById(int id, int userId);
        Task Sort(IEnumerable<AccountType> accountTypes);
    }
}
