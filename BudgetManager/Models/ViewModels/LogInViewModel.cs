using System.ComponentModel.DataAnnotations;

namespace BudgetManager.Models.ViewModels
{
    public class LogInViewModel
    {
        [Required(ErrorMessage = "El campo {0} es requerido")]
        [Display(Name = "Correo Electronico")]
        [EmailAddress(ErrorMessage = "El campo debe ser un correo electronico valido")]
        public string Email { get; set; } = string.Empty;

        [Required(ErrorMessage = "El campo {0} es requerido")]
        [Display(Name = "Contraseña")]
        [DataType(DataType.Password)]
        public string Password { get; set; } = string.Empty;

        [Required(ErrorMessage = "El campo {0} es requerido")]
        [Display(Name = "Recordarme")]
        public bool RememberMe { get; set; }
    }
}
