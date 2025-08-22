using BudgetManager.Models.Dtos;

namespace BudgetManager.Services
{
    public interface IReportService
    {
        Task<TransactionReportDetail> GetReportDetailByAccount
            (int userId, int accountId, int month, int year, dynamic ViewBag);

        Task<TransactionReportDetail> GetReportDetailByUser
            (int userId, int month, int year, dynamic ViewBag);

        Task<IEnumerable<TransactionByWeek>> GetReportDetailByWeek
            (int userId, int month, int year, dynamic ViewBag);
    }
}
