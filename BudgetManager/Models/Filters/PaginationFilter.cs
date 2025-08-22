namespace BudgetManager.Models.Filters
{
    public class PaginationFilter
    {
        public int Page { get; set; } = 1;
        private int rowsPerPage = 10;
        private readonly int maxRowsPerPage = 50;

        public int RowsPerPage
        {
            get 
            {
                return rowsPerPage;
            }
            set
            {
                rowsPerPage = (value > maxRowsPerPage) ? maxRowsPerPage : value;
            } 
        }

        public int Skip => rowsPerPage * (Page -1);
    }
}
