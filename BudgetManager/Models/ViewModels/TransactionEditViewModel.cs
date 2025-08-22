namespace BudgetManager.Models.ViewModels
{
    public class TransactionEditViewModel: TransactionCreateViewModel
    {
        public decimal PreviousAmount { get; set; }
        public int PreviousAccountId { get; set; }
        public string? UrlCallBack { get; set; }
    }
}
