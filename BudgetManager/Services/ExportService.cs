using BudgetManager.Models.Entities;
using ClosedXML.Excel;
using Microsoft.AspNetCore.Mvc;
using System.Data;

namespace BudgetManager.Services
{
    public class ExportService : IExportService
    {
        public FileResult ExportReportAsExcel(string fileName, IEnumerable<Transaction> transactions)
        {
            DataTable dataTable = new("transacciones");
            dataTable.Columns.AddRange([
                new("Fecha"),
                new("Cuenta"),
                new("Categoría"),
                new("Nota"),
                new("Monto"),
                new("Ingreso / Egreso")
            ]);

            foreach (var transaction in transactions)
            {
                dataTable.Rows.Add(
                    transaction.TransactionDate,
                    transaction.Account,
                    transaction.Category,
                    transaction.Note,
                    transaction.Amount,
                    transaction.TransactionTypeId);
            }

            return ExportExcel(fileName, dataTable);
        }

        private static FileResult ExportExcel(string fileName, DataTable dataTable)
        {
            using XLWorkbook workBook = new();
            workBook.Worksheets.Add(dataTable);

            using MemoryStream memoryStream = new();              
            workBook.SaveAs(memoryStream);
            return new FileContentResult(memoryStream.ToArray(),
            "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet")
            {
                FileDownloadName = fileName
            };
        }
    }
}
