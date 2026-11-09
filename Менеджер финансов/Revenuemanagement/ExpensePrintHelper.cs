using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Printing;
using System.Linq;
using System.Windows.Forms;
using Revenuemanagement.Models;
using Revenuemanagement.Services;

namespace Revenuemanagement
{
    /// <summary>Печать расходов в виде кассовой квитанции.</summary>
    public static class ExpensePrintHelper
    {
        private const int ReceiptWidthChars = 42;

        public static void ShowPreview(IWin32Window owner, IList<HistoryRow> rows, string periodTitle)
        {
            if (rows == null || rows.Count == 0)
            {
                UiHelper.ShowError(owner, "Нет расходов для печати в текущем списке.");
                return;
            }

            try
            {
                var receipt = BuildReceipt(rows, periodTitle);
                int lineIndex = 0;

                var doc = new PrintDocument();
                doc.DocumentName = "Квитанция расходов";

                doc.PrintPage += (s, e) =>
                {
                    float y = e.MarginBounds.Top;
                    float centerX = e.MarginBounds.Left + e.MarginBounds.Width / 2f;

                    using (var fontTitle = new Font("Courier New", 12f, FontStyle.Bold))
                    using (var fontBold = new Font("Courier New", 9f, FontStyle.Bold))
                    using (var fontBody = new Font("Courier New", 9f, FontStyle.Regular))
                    {
                        while (lineIndex < receipt.Count)
                        {
                            ReceiptLine line = receipt[lineIndex];
                            Font font = line.Bold ? fontBold : (line.Title ? fontTitle : fontBody);
                            float height = font.GetHeight(e.Graphics) + 2f;

                            if (y + height > e.MarginBounds.Bottom)
                            {
                                e.HasMorePages = true;
                                return;
                            }

                            if (line.Centered)
                            {
                                SizeF size = e.Graphics.MeasureString(line.Text, font);
                                e.Graphics.DrawString(
                                    line.Text, font, Brushes.Black,
                                    centerX - size.Width / 2f, y);
                            }
                            else
                            {
                                e.Graphics.DrawString(
                                    line.Text, font, Brushes.Black,
                                    e.MarginBounds.Left, y);
                            }

                            y += height;
                            lineIndex++;
                        }
                    }

                    e.HasMorePages = false;
                };

                using (var preview = new PrintPreviewDialog())
                {
                    preview.Document = doc;
                    preview.Width = 480;
                    preview.Height = 720;
                    preview.ShowDialog(owner);
                }
            }
            catch (Exception ex)
            {
                UiHelper.ShowError(owner, "Не удалось открыть печать.\n" + ex.Message);
            }
        }

        private static List<ReceiptLine> BuildReceipt(IList<HistoryRow> rows, string periodTitle)
        {
            var lines = new List<ReceiptLine>();
            string dash = new string('-', ReceiptWidthChars);

            lines.Add(new ReceiptLine { Text = "КВИТАНЦИЯ ПО РАСХОДАМ", Title = true, Centered = true });
            lines.Add(new ReceiptLine { Text = periodTitle, Centered = true });
            lines.Add(new ReceiptLine { Text = DateTime.Now.ToString("dd.MM.yyyy HH:mm"), Centered = true });
            lines.Add(new ReceiptLine { Text = dash });

            lines.Add(new ReceiptLine
            {
                Text = PadColumns("ДАТА", "КАТЕГОРИЯ", "СУММА"),
                Bold = true
            });
            lines.Add(new ReceiptLine { Text = dash });

            var totalsByCurrency = new Dictionary<CurrencyType, decimal>();

            foreach (var row in rows)
            {
                string date = row.Дата ?? "";
                if (date.Length > 10)
                    date = date.Substring(0, 10);

                string category = Truncate(row.Категория ?? "", 14);
                string sum = string.Format(
                    "{0:N2} {1}",
                    row.Amount,
                    FinanceManager.CurrencyShort(row.Currency));

                lines.Add(new ReceiptLine { Text = PadColumns(date, category, sum) });

                if (totalsByCurrency.ContainsKey(row.Currency))
                    totalsByCurrency[row.Currency] += row.Amount;
                else
                    totalsByCurrency[row.Currency] = row.Amount;
            }

            lines.Add(new ReceiptLine { Text = dash });

            string total = string.Join(" | ", totalsByCurrency
                .OrderBy(pair => FinanceManager.CurrencyShort(pair.Key))
                .Select(pair => string.Format(
                    "{0:N2} {1}",
                    pair.Value,
                    FinanceManager.CurrencyShort(pair.Key))));

            lines.Add(new ReceiptLine { Text = "ИТОГО: " + total, Bold = true });

            return lines;
        }

        private static string PadColumns(string col1, string col2, string col3)
        {
            return Fit(col1, 10) + Fit(col2, 14) + Fit(col3, 18, alignRight: true);
        }

        private static string Fit(string text, int width, bool alignRight = false)
        {
            text = text ?? "";
            if (text.Length > width)
                text = text.Substring(0, width);
            return alignRight ? text.PadLeft(width) : text.PadRight(width);
        }

        private static string Truncate(string text, int max)
        {
            if (string.IsNullOrEmpty(text) || text.Length <= max)
                return text ?? "";
            return text.Substring(0, max - 1) + "…";
        }

        private sealed class ReceiptLine
        {
            public string Text { get; set; }
            public bool Bold { get; set; }
            public bool Title { get; set; }
            public bool Centered { get; set; }
        }
    }
}
