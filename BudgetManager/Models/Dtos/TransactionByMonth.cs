using BudgetManager.Models.Enums;

namespace BudgetManager.Models.Dtos
{
    public class TransactionByMonth
    {
        public int Month { get; set; }
        public DateTime ReferenceDate { get; set; }
        public decimal Amount { get; set; }
        public decimal IncomeBalance { get; set; }
        public decimal OutcomeBalance { get; set; }
        public TransactionType TransactionTypeId { get; set; }
    }
}
