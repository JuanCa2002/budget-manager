using BudgetManager.Models.Entities;
using BudgetManager.Models.Enums;

namespace BudgetManager.Models.Dtos
{
    public class TransactionByDate
    {
        public DateTime TransactionDate { get; set; }
        public IEnumerable<Transaction> Transactions { get; set; }  = Enumerable.Empty<Transaction>();
        public decimal IncomeBalance => 
            Transactions
            .Where(transaction => transaction.TransactionTypeId.Equals(TransactionType.Income))
            .Sum(transaction => transaction.Amount);
        public decimal OutcomeBalance => 
            Transactions
            .Where(transaction => transaction.TransactionTypeId.Equals(TransactionType.Outcome))
            .Sum(transaction => transaction.Amount);

    }
}
