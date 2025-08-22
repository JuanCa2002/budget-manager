using BudgetManager.Models.Entities;

namespace BudgetManager.Models.ViewModels
{
    public class IndexAccountViewModel
    {
        public string AccountType { get; set; } = string.Empty;
        public IEnumerable<Account> Accounts { get; set; } = [];

        public decimal Balance => Accounts.Sum(x => x.Balance);
    }
}
