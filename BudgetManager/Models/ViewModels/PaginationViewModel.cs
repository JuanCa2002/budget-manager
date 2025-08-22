namespace BudgetManager.Models.ViewModels
{
    public class PaginationViewModel
    {
        public int Page { get; set; } = 1;
        public int RowsPerPage { get; set; } = 10;
    }
}
