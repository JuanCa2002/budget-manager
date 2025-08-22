using BudgetManager.Models.Entities;
using Microsoft.AspNetCore.Mvc;

namespace BudgetManager.Services
{
    public interface IExportService
    {
        FileResult ExportReportAsExcel(string fileName, IEnumerable<Transaction> transactions);
    }
}
