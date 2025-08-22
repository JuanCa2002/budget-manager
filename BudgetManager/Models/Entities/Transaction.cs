using BudgetManager.Models.Enums;
using System.ComponentModel.DataAnnotations;

namespace BudgetManager.Models.Entities
{
    public class Transaction
    {
        public int Id { get; set; }
        public int UserId { get; set; }

        [Display(Name = "Fecha Transacción")]
        [DataType(DataType.Date)]
        public DateTime TransactionDate { get; set; } = DateTime.Today;

        [Display(Name = "Monto")]
        public decimal Amount { get; set; }

        [StringLength(maximumLength: 1000, ErrorMessage = "La nota no puede superar los {0} caracteres")]
        [Display(Name = "Nota")]
        public string? Note { get; set; }

        [Range(1, maximum: int.MaxValue, ErrorMessage = "Debe seleccionar una cuenta")]
        [Display(Name = "Cuenta")]
        public int AccountId { get; set; }

        [Range(1, maximum: int.MaxValue, ErrorMessage = "Debe seleccionar una categoría")]
        [Display(Name = "Categoría")]
        public int CategoryId { get; set; }

        [Display(Name = "Tipo Transacción")]
        public TransactionType TransactionTypeId { get; set; } = TransactionType.Income;

        [Display(Name = "Categoría")]
        public string Category { get; set; } = string.Empty;

        [Display(Name = "Cuenta")]
        public string Account { get; set; } = string.Empty;
    }
}
