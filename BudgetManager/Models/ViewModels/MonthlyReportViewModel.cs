using BudgetManager.Models.Dtos;

namespace BudgetManager.Models.ViewModels
{
    public class MonthlyReportViewModel
    {
        public IEnumerable<TransactionByMonth> TransactionsByMonth { get; set; } = Enumerable.Empty<TransactionByMonth>();
        public decimal IncomeBalance => TransactionsByMonth.Sum(x => x.IncomeBalance);
        public decimal OutcomeBalance => TransactionsByMonth.Sum(x => x.OutcomeBalance);
        public decimal TotalBalance => IncomeBalance - OutcomeBalance;
        public int Year { get; set; }
    }
}
