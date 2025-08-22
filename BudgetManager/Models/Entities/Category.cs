using BudgetManager.Models.Enums;
using System.ComponentModel.DataAnnotations;

namespace BudgetManager.Models.Entities
{
    public class Category
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "El campo {0} es requerido")]
        [StringLength(maximumLength: 50, MinimumLength = 3, ErrorMessage = "El campo {0} debe estar entre {2} y {1} ")]
        [Display(Name = "Nombre")]
        public string Name { get; set; } = string.Empty;

        [Display(Name = "Tipo Transacción")]
        public TransactionType TransactionTypeId { get; set; }
        public int userId { get; set; }
    }
}
