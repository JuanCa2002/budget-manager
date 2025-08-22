namespace BudgetManager.Services
{
    public interface IEmailService
    {
        Task SendEmailForgotPassword(string receiver, string link);
    }
}
