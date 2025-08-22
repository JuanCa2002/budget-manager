namespace BudgetManager.Models.ViewModels
{
    public class PaginationResponseViewModel : PaginationViewModel
    {
        public int Total { get; set; }
        public int TotalPages => (int)Math.Ceiling((double)Total / RowsPerPage);
        public string? BaseURL { get; set; } = string.Empty;
    }
}
