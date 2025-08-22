namespace BudgetManager.Models.Dtos
{
    public class TransactionReportDetail
    {
        public DateTime InitialDate { get; set; }
        public DateTime FinalDate { get; set; }
        public IEnumerable<TransactionByDate> TransactionsByDate { get; set; } = Enumerable.Empty<TransactionByDate>();
        public decimal TotalIncomeBalance => 
            TransactionsByDate
            .Sum(transactionByDate => transactionByDate.IncomeBalance);
        public decimal TotalOutcomeBalance =>
            TransactionsByDate
            .Sum(transactionByDate => transactionByDate.OutcomeBalance);

        public decimal TotalBalance => TotalIncomeBalance - TotalOutcomeBalance;

    }
}
