namespace BudgetManager.Models.ViewModels
{
    public class ConfirmationModalViewModel
    {

        public string ModalTitle { get; set; } = "Confirmar";
        public string ModalTextBody { get; set; } = string.Empty;
        public string AcceptTextButton { get; set; } = "Aceptar";
        public string CancelTextButton { get; set; } = "Cancelar";
        public string ActionTarget { get; set; } = "Index";
        public string ControllerTarget { get; set; } = "Home";
        public string? UrlCallBack { get; set; } = null;
    }
}
