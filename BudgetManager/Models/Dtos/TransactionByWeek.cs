using BudgetManager.Models.Enums;

namespace BudgetManager.Models.Dtos
{
    public class TransactionByWeek
    {
        public int Week { get; set; }
        public decimal Amount { get; set; }
        public TransactionType TransactionTypeId { get; set; }
        public decimal IncomeBalance{ get; set; }
        public decimal OutcomeBalance { get; set; }
        public DateTime InitialDate { get; set; }
        public DateTime FinalDate { get; set; }
    }
}
