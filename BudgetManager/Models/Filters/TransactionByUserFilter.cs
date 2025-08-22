namespace BudgetManager.Models.Filters
{
    public class TransactionByUserFilter
    {
        public int UserId { get; set; }
        public DateTime InitialDate { get; set; }
        public DateTime FinalDate { get; set; }
    }
}
