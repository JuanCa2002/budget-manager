using BudgetManager.Models.Dtos;

namespace BudgetManager.Models.ViewModels
{
    public class WeeklyReportViewModel
    {
        public decimal IncomeBalance => TransactionsByWeek.Sum(x => x.IncomeBalance);
        public decimal OutcomeBalance => TransactionsByWeek.Sum(x => x.OutcomeBalance);
        public decimal TotalBalance => IncomeBalance - OutcomeBalance;
        public DateTime ReferenceDate { get; set; }
        public IEnumerable<TransactionByWeek> TransactionsByWeek { get; set; } = Enumerable.Empty<TransactionByWeek>();
    }
}
