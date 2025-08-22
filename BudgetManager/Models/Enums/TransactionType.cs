using System.ComponentModel.DataAnnotations;

namespace BudgetManager.Models.Enums
{
    public enum TransactionType
    {
        [Display(Name = "Ingreso")]
        Income = 1,
        [Display(Name = "Egreso")]
        Outcome = 2
    }
}
