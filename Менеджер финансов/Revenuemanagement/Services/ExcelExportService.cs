using System;
using System.Collections.Generic;
using System.Linq;
using ClosedXML.Excel;
using Revenuemanagement.Models;
using Revenuemanagement.Services;

namespace Revenuemanagement
{
    internal static class ExcelExportService
    {
        public static void Export(string path, IEnumerable<Transaction> source, string periodTitle)
        {
            var rows = source.Where(t => t.Type == TransactionType.Income || t.Type == TransactionType.Expense)
                .OrderBy(t => t.Date).ToList();

            using (var workbook = new XLWorkbook())
            {
                var sheet = workbook.Worksheets.Add("Доходы и расходы");
                sheet.Cell(1, 1).Value = "Доходы и расходы — " + periodTitle;
                sheet.Range(1, 1, 1, 6).Merge();
                sheet.Cell(1, 1).Style.Font.Bold = true;
                sheet.Cell(1, 1).Style.Font.FontSize = 16;
                sheet.Cell(1, 1).Style.Font.FontColor = XLColor.White;
                sheet.Cell(1, 1).Style.Fill.BackgroundColor = XLColor.FromHtml("#243B55");
                sheet.Row(1).Height = 30;

                string[] headers = { "Дата", "Тип", "Категория", "Сумма", "Валюта", "Заметка" };
                for (int c = 0; c < headers.Length; c++) sheet.Cell(3, c + 1).Value = headers[c];
                var header = sheet.Range(3, 1, 3, headers.Length);
                header.Style.Font.Bold = true;
                header.Style.Font.FontColor = XLColor.White;
                header.Style.Fill.BackgroundColor = XLColor.FromHtml("#3B6286");
                header.Style.Alignment.Vertical = XLAlignmentVerticalValues.Center;
                header.Style.Border.OutsideBorder = XLBorderStyleValues.Thin;
                header.Style.Border.InsideBorder = XLBorderStyleValues.Thin;

                int row = 4;
                foreach (var tx in rows)
                {
                    sheet.Cell(row, 1).Value = tx.Date;
                    sheet.Cell(row, 1).Style.DateFormat.Format = "dd.mm.yyyy hh:mm";
                    sheet.Cell(row, 2).Value = tx.Type == TransactionType.Income ? "Доход" : "Расход";
                    sheet.Cell(row, 3).Value = tx.CategoryDisplay;
                    sheet.Cell(row, 4).Value = tx.Amount;
                    sheet.Cell(row, 4).Style.NumberFormat.Format = "#,##0.00";
                    sheet.Cell(row, 5).Value = FinanceManager.CurrencyShort(tx.Currency);
                    sheet.Cell(row, 6).Value = tx.Note ?? string.Empty;
                    var line = sheet.Range(row, 1, row, 6);
                    line.Style.Fill.BackgroundColor = XLColor.FromHtml(tx.Type == TransactionType.Income ? "#E7F5EC" : "#FCEAEA");
                    line.Style.Border.OutsideBorder = XLBorderStyleValues.Thin;
                    line.Style.Border.InsideBorder = XLBorderStyleValues.Thin;
                    row++;
                }

                row += 1;
                sheet.Cell(row, 1).Value = "Итоги по валютам";
                sheet.Range(row, 1, row, 5).Merge();
                sheet.Cell(row, 1).Style.Font.Bold = true;
                sheet.Cell(row, 1).Style.Font.FontSize = 13;
                sheet.Cell(row, 1).Style.Fill.BackgroundColor = XLColor.FromHtml("#E8EEF5");
                row++;
                string[] summaryHeaders = { "Валюта", "Доходы", "Расходы", "Баланс" };
                for (int c = 0; c < summaryHeaders.Length; c++) sheet.Cell(row, c + 1).Value = summaryHeaders[c];
                sheet.Range(row, 1, row, 4).Style.Font.Bold = true;
                sheet.Range(row, 1, row, 4).Style.Fill.BackgroundColor = XLColor.FromHtml("#DCE6F1");
                row++;
                foreach (var currency in Enum.GetValues(typeof(CurrencyType)).Cast<CurrencyType>())
                {
                    decimal income = rows.Where(t => t.Currency == currency && t.Type == TransactionType.Income).Sum(t => t.Amount);
                    decimal expense = rows.Where(t => t.Currency == currency && t.Type == TransactionType.Expense).Sum(t => t.Amount);
                    if (income == 0m && expense == 0m) continue;
                    sheet.Cell(row, 1).Value = FinanceManager.CurrencyShort(currency);
                    sheet.Cell(row, 2).Value = income;
                    sheet.Cell(row, 3).Value = expense;
                    sheet.Cell(row, 4).Value = income - expense;
                    sheet.Range(row, 2, row, 4).Style.NumberFormat.Format = "#,##0.00";
                    sheet.Range(row, 1, row, 4).Style.Border.OutsideBorder = XLBorderStyleValues.Thin;
                    sheet.Range(row, 1, row, 4).Style.Border.InsideBorder = XLBorderStyleValues.Thin;
                    row++;
                }

                sheet.Columns(1, 6).AdjustToContents();
                sheet.Column(1).Width = Math.Min(22, Math.Max(18, sheet.Column(1).Width));
                sheet.Column(3).Width = Math.Min(36, Math.Max(16, sheet.Column(3).Width));
                sheet.Column(6).Width = Math.Min(50, Math.Max(18, sheet.Column(6).Width));
                sheet.SheetView.FreezeRows(3);
                workbook.SaveAs(path);
            }
        }
    }
}
