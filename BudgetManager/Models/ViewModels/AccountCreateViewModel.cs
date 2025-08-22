using BudgetManager.Models.Entities;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace BudgetManager.Models.ViewModels
{
    public class AccountCreateViewModel: Account
    {
        public IEnumerable<SelectListItem> AccountTypes { get; set; } = [];
    }
}
