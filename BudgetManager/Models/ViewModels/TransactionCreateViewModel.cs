using BudgetManager.Models.Entities;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace BudgetManager.Models.ViewModels
{
    public class TransactionCreateViewModel: Transaction
    {
        public IEnumerable<SelectListItem> Accounts { get; set; } = [];
        public IEnumerable<SelectListItem> Categories { get; set; } = [];
    }
}
