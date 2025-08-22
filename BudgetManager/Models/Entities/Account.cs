using BudgetManager.Validations;
using System.ComponentModel.DataAnnotations;

namespace BudgetManager.Models.Entities
{
    public class Account
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "El campo {0} es requerido")]
        [StringLength(maximumLength: 50, MinimumLength = 3, ErrorMessage = "El campo {0} debe estar entre {2} y {1}")]
        [FirstCapitalLetter]
        [Display(Name = "Nombre")]
        public string Name { get; set; } = string.Empty;

        [Display(Name = "Tipo Cuenta")]
        public int AccountTypeId { get; set; }

        public decimal Balance { get; set; }

        [StringLength(maximumLength:1000, ErrorMessage = "El campo {0} debe tener maximo {1} caracteres")]
        [Display(Name = "Descripción")]
        public string? Description { get; set; } = null;

        public string AccountType { get; set; } = string.Empty;
    }
}
