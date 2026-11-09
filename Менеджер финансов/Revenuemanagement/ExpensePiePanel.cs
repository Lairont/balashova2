using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Linq;
using System.Windows.Forms;
using Revenuemanagement.Models;

namespace Revenuemanagement
{
    /// <summary>Простая круговая диаграмма расходов по категориям (без внешних библиотек).</summary>
    public class ExpensePiePanel : Panel
    {
        private List<CategoryTotal> _data = new List<CategoryTotal>();
        private readonly string[] _palette =
        {
            "#2EA078", "#4682B4", "#D4A032", "#D25555",
            "#7B68EE", "#20B2AA", "#CD853F", "#708090"
        };

        public ExpensePiePanel()
        {
            // Полная двойная буферизация — убирает полосы и «хвосты» при ресайзе
            SetStyle(ControlStyles.AllPaintingInWmPaint |
                     ControlStyles.OptimizedDoubleBuffer |
                     ControlStyles.UserPaint |
                     ControlStyles.ResizeRedraw |
                     ControlStyles.Opaque, true);
            DoubleBuffered = true;
            ResizeRedraw = true;
            UpdateStyles();
        }

        public void SetData(IEnumerable<CategoryTotal> data)
        {
            var items = (data ?? Enumerable.Empty<CategoryTotal>())
                .Where(x => x != null && x.Total > 0)
                .OrderByDescending(x => x.Total)
                .ToList();

            // На диаграмме показываем не больше шести категорий,
            // остальные объединяем в «Прочее», чтобы легенда помещалась.
            if (items.Count > 6)
            {
                var top = items.Take(6).ToList();
                decimal other = items.Skip(6).Sum(x => x.Total);
                if (other > 0)
                    top.Add(new CategoryTotal { Category = "Прочее", Total = other });
                items = top;
            }

            _data = items;
            Invalidate();
        }

        protected override void OnResize(EventArgs e)
        {
            base.OnResize(e);
            Invalidate();
        }

        protected override void OnPaintBackground(PaintEventArgs e)
        {
            // Сами очищаем фон в OnPaint — иначе WinForms оставляет артефакты
            e.Graphics.Clear(BackColor);
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            // Не вызываем base.OnPaint — рисуем кадр целиком сами
            var g = e.Graphics;
            g.SmoothingMode = SmoothingMode.AntiAlias;
            g.Clear(BackColor);

            using (var titleFont = new Font("Segoe UI Semibold", 11f, FontStyle.Bold))
            using (var smallFont = new Font("Segoe UI", 9f))
            using (var brushTitle = new SolidBrush(Theme.TextPrimary))
            using (var brushMuted = new SolidBrush(Theme.TextMuted))
            {
                g.DrawString("Расходы по категориям", titleFont, brushTitle, 8, 8);

                if (_data.Count == 0)
                {
                    g.DrawString("Пока нет расходов\nза выбранный период", smallFont, brushMuted, 8, 40);
                    return;
                }

                decimal sum = _data.Sum(x => x.Total);
                if (sum <= 0)
                {
                    g.DrawString("Пока нет расходов\nза выбранный период", smallFont, brushMuted, 8, 40);
                    return;
                }

                int legendHeight = _data.Count * 18;
                int sizeByHeight = Height - 36 - 12 - legendHeight - 10;
                int size = Math.Min(Width - 24, sizeByHeight);
                if (size < 60) size = 60;

                int pieX = Math.Max(12, (Width - size) / 2);
                var rect = new Rectangle(pieX, 36, size, size);

                float start = -90f;
                for (int i = 0; i < _data.Count; i++)
                {
                    float sweep = (float)((double)_data[i].Total / (double)sum * 360.0);
                    using (var brush = new SolidBrush(ColorTranslator.FromHtml(_palette[i % _palette.Length])))
                    {
                        if (sweep > 0.1f)
                            g.FillPie(brush, rect, start, sweep);
                    }
                    start += sweep;
                }

                using (var pen = new Pen(BackColor, 2))
                    g.DrawEllipse(pen, rect);

                int legendY = rect.Bottom + 12;
                int maxTextWidth = Math.Max(40, Width - 30 - 8);
                for (int i = 0; i < _data.Count && legendY < Height - 10; i++)
                {
                    var color = ColorTranslator.FromHtml(_palette[i % _palette.Length]);
                    using (var brush = new SolidBrush(color))
                        g.FillRectangle(brush, 12, legendY + 2, 12, 12);

                    double percent = (double)_data[i].Total / (double)sum * 100.0;
                    string label = string.Format(
                        "{0}: {1:N0} · {2:0}%",
                        _data[i].Category, _data[i].Total, percent);
                    label = FitText(g, label, smallFont, maxTextWidth);

                    g.DrawString(label, smallFont, brushTitle, 30, legendY);
                    legendY += 18;
                }
            }
        }

        private static string FitText(Graphics g, string text, Font font, float maxWidth)
        {
            if (string.IsNullOrEmpty(text))
                return string.Empty;
            if (g.MeasureString(text, font).Width <= maxWidth)
                return text;

            const string ellipsis = "…";
            while (text.Length > 1 &&
                   g.MeasureString(text + ellipsis, font).Width > maxWidth)
            {
                text = text.Substring(0, text.Length - 1);
            }

            return text + ellipsis;
        }
    }
}
