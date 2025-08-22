using System.ComponentModel.DataAnnotations;

namespace BudgetManager.Models.ViewModels
{
    public class ForgotPasswordViewModel
    {
        [Required(ErrorMessage = "El campo {0} es requerido")]
        [EmailAddress(ErrorMessage = "El campo debe ser un correo electronico valido")]
        [Display(Name = "Correo Electronico")]
        public string Email { get; set; } = string.Empty;
    }
}
