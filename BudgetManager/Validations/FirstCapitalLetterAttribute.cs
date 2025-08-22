using System.ComponentModel.DataAnnotations;

namespace BudgetManager.Validations
{
    public class FirstCapitalLetterAttribute: ValidationAttribute
    {
        protected override ValidationResult? IsValid(object? value, ValidationContext validationContext)
        {
            string valueAsString = value.ToString();

            if (valueAsString == null || string.IsNullOrEmpty(valueAsString))
            {
                return ValidationResult.Success;
            }

            var firstLetter = valueAsString[0].ToString();

            if(!firstLetter.Equals(firstLetter.ToUpper())) 
            {
                return new ValidationResult("La primera letra debe ser mayuscula");
            }

            return ValidationResult.Success;
        }
    }
}
