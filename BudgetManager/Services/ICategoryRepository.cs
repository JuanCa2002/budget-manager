using BudgetManager.Models.Entities;
using BudgetManager.Models.Enums;
using BudgetManager.Models.Filters;

namespace BudgetManager.Services
{
    public interface ICategoryRepository
    {
        Task Create(Category category);

        Task<Category?> GetById(int id, int userId);

        Task<IEnumerable<Category>> GetAll(int userId, PaginationFilter paginationFilter);

        Task<IEnumerable<Category>> GetAllByTransactionType(int userId, TransactionType transactionType);

        Task Update(Category category);

        Task Delete(int id, int userId);

        Task<int> Count(int userId);
    }
}
