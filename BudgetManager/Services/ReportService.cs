using BudgetManager.Models.Dtos;
using BudgetManager.Models.Entities;
using BudgetManager.Models.Filters;

namespace BudgetManager.Services
{
    public class ReportService : IReportService
    {
        private readonly ITransactionRepository _transactionRepository;
        private readonly HttpContext? httpContext;
        public ReportService(ITransactionRepository transactionRepository,
            IHttpContextAccessor httpContextAccessor)
        {
            _transactionRepository = transactionRepository;
            httpContext = httpContextAccessor.HttpContext;
        }
        public async Task<TransactionReportDetail> GetReportDetailByAccount
            (int userId, int accountId, int month, 
            int year, dynamic ViewBag)
        {
            (DateTime initialDate, DateTime finalDate) = GenerateInitialAndFinalDate(month, year);

            TransactionsByAccountFilter filter = new()
            {
                UserId = userId,
                AccountId = accountId,
                InitialDate = initialDate,
                FinalDate = finalDate
            };
            var transactions = await _transactionRepository.GetByAccount(filter);

            TransactionReportDetail model = GenerateTransactionDetailReport(initialDate, finalDate, transactions);

            AsignViewBagValues(ViewBag, initialDate);

            return model;
        }

        public async Task<TransactionReportDetail> GetReportDetailByUser
            (int userId, int month, int year, dynamic ViewBag)
        {
            (DateTime initialDate, DateTime finalDate) = GenerateInitialAndFinalDate(month, year);

            var filter = new TransactionByUserFilter()
            {
                UserId = userId,
                InitialDate = initialDate,
                FinalDate = finalDate
            };

            var transactions = await _transactionRepository.GetByUser(filter);

            TransactionReportDetail model = GenerateTransactionDetailReport(initialDate, finalDate, transactions);

            AsignViewBagValues(ViewBag, initialDate);

            return model;

        }

        public async Task<IEnumerable<TransactionByWeek>> GetReportDetailByWeek(int userId, int month, int year, dynamic ViewBag)
        {
            (DateTime initialDate, DateTime finalDate) = GenerateInitialAndFinalDate(month, year);

            var filter = new TransactionByUserFilter()
            {
                UserId = userId,
                InitialDate = initialDate,
                FinalDate = finalDate
            };

            AsignViewBagValues(ViewBag, initialDate);
            
            var transactions = await _transactionRepository.GetByWeek(filter);
            return transactions;
        }

        // Auxiliar Methods
        private static (DateTime initialDate, DateTime finalDate) GenerateInitialAndFinalDate(int month, int year)
        {
            DateTime initialDate;
            DateTime finalDate;

            if (month <= 0 || month > 12 || year <= 1900)
            {
                var today = DateTime.Today;
                initialDate = new DateTime(today.Year, today.Month, 1);
            }
            else
            {
                initialDate = new DateTime(year, month, 1);
            }

            finalDate = initialDate.AddMonths(1).AddDays(-1);

            return (initialDate, finalDate);
        }

        private static TransactionReportDetail GenerateTransactionDetailReport(DateTime initialDate, DateTime finalDate, IEnumerable<Transaction> transactions)
        {
            var model = new TransactionReportDetail();

            var transactionsByDate = transactions
                .OrderByDescending(x => x.TransactionDate)
                .GroupBy(x => x.TransactionDate)
                .Select(group => new TransactionByDate()
                {
                    TransactionDate = group.Key,
                    Transactions = group.AsEnumerable()
                });

            model.TransactionsByDate = transactionsByDate;
            model.InitialDate = initialDate;
            model.FinalDate = finalDate;
            return model;
        }

        private void AsignViewBagValues(dynamic ViewBag, DateTime initialDate)
        {
            ViewBag.PreviousMonth = initialDate.AddMonths(-1).Month;
            ViewBag.PreviousYear = initialDate.AddMonths(-1).Year;
            ViewBag.NextMonth = initialDate.AddMonths(1).Month;
            ViewBag.NextYear = initialDate.AddMonths(1).Year;
            ViewBag.UrlCallBack = httpContext!.Request.Path + httpContext.Request.QueryString;
        }
    }
}
