using Microsoft.AspNetCore.Identity;

namespace BudgetManager.Models.Dtos
{
    public class ErrorMessageIdentity: IdentityErrorDescriber
    {
        public override IdentityError PasswordTooShort(int length) { return new IdentityError { Code = nameof(PasswordTooShort), Description = $"La contraseña deben tener un largo mínimo de {length} caracteres." }; }
    }
}
