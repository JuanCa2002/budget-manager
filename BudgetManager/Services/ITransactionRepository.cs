using BudgetManager.Models.Dtos;
using BudgetManager.Models.Entities;
using BudgetManager.Models.Filters;

namespace BudgetManager.Services
{
    public interface ITransactionRepository
    {
        Task Save(Transaction transaction);

        Task Update(Transaction transaction, decimal previousAmount, int previousAccountId);

        Task<Transaction?> GetById(int id, int userId);

        Task Delete(int id, int userId);

        Task<IEnumerable<Transaction>> GetByAccount(TransactionsByAccountFilter filter);

        Task<IEnumerable<Transaction>> GetByUser(TransactionByUserFilter filter);

        Task<IEnumerable<TransactionByWeek>> GetByWeek(TransactionByUserFilter filter);

        Task<IEnumerable<TransactionByMonth>> GetByMonth(int userId, int year);
    }
}
