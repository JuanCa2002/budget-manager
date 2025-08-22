using BudgetManager.Validations;
using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;

namespace BudgetManager.Models.Entities
{
    public class AccountType
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "El campo {0} es requerido")]
        [StringLength(maximumLength: 50, MinimumLength = 3, ErrorMessage = "El campo {0} debe estar entre {2} y {1} caracteres")]
        [Display(Name = "Nombre")]
        [FirstCapitalLetter]
        [Remote(action: "VerifyExistAccount", controller: "AccountTypes", AdditionalFields = nameof(Id))]
        public string Name { get; set; } = string.Empty;
        public int Order { get; set; }
        public int UserId { get; set; }
    }
}
