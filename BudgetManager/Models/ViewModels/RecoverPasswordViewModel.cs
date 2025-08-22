using System.ComponentModel.DataAnnotations;

namespace BudgetManager.Models.ViewModels
{
    public class RecoverPasswordViewModel
    {
        [Required(ErrorMessage = "El campo {0} es requerido")]
        [EmailAddress(ErrorMessage = "El campo debe ser un correo electronico valido.")]
        [Display(Name = "Correo Electronico")]
        public string Email { get; set; } = string.Empty;

        [Required(ErrorMessage = "El campo {0} es requerido")]
        [DataType(DataType.Password)]
        [Display(Name = "Nueva Contraseña")]
        public string Password { get; set; } = string.Empty;
        public string RecoverCode { get; set; } = string.Empty;
    }
}
