namespace BudgetManager.Models.Filters
{
    public class TransactionsByAccountFilter: TransactionByUserFilter
    {
        public int AccountId { get; set; }
    }
}
