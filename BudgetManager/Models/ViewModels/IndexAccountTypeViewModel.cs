using BudgetManager.Models.Entities;

namespace BudgetManager.Models.ViewModels
{
    public class IndexAccountTypeViewModel
    {
        public IEnumerable<AccountType> AccountTypes { get; set; } = [];
        public ConfirmationModalViewModel ConfirmationModal { get; set; } = new ConfirmationModalViewModel();
    }
}
