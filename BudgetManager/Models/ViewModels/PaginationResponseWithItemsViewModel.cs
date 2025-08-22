namespace BudgetManager.Models.ViewModels
{
    public class PaginationResponseWithItemsViewModel<T>: PaginationResponseViewModel
    {
        public IEnumerable<T> Rows { get; set; } = Enumerable.Empty<T>();
    }
}
