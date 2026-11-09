using System;
using System.Drawing;
using System.Globalization;
using System.Windows.Forms;
using Revenuemanagement.Models;

namespace Revenuemanagement
{
    public static class UiHelper
    {
        public static readonly Font TitleFont = new Font("Segoe UI Semibold", 16f, FontStyle.Bold);
        public static readonly Font HeaderFont = new Font("Segoe UI Semibold", 13f, FontStyle.Bold);
        public static readonly Font BodyFont = new Font("Segoe UI", 11f);
        public static readonly Font BigButtonFont = new Font("Segoe UI Semibold", 11.5f, FontStyle.Bold);
        public static readonly Font CardAmountFont = new Font("Segoe UI Semibold", 18f, FontStyle.Bold);
        public static readonly Font GridFont = new Font("Segoe UI", 10.5f);
        public static readonly Font FieldLabelFont = new Font("Segoe UI", 9.5f);
        public static readonly Font HintFont = new Font("Segoe UI", 9.5f);

        // Совместимость со старыми ссылками
        public static Color BgMain => Theme.BgMain;
        public static Color BgSidebar => Theme.BgSidebar;
        public static Color AccentGreen => Theme.AccentGreen;
        public static Color AccentRed => Theme.AccentRed;
        public static Color AccentBlue => Theme.AccentBlue;
        public static Color AccentGold => Theme.AccentGold;
        public static Color TextDark => Theme.TextPrimary;
        public static Color TextMuted => Theme.TextMuted;

        public static void StyleFlatButton(Button btn, Color back, Color fore)
        {
            btn.FlatStyle = FlatStyle.Flat;
            btn.FlatAppearance.BorderSize = 0;
            btn.FlatAppearance.MouseOverBackColor = Shift(back, 18);
            btn.FlatAppearance.MouseDownBackColor = Shift(back, -18);
            btn.BackColor = back;
            btn.ForeColor = fore;
            btn.Font = BigButtonFont;
            btn.Cursor = Cursors.Hand;
            btn.TextAlign = ContentAlignment.MiddleLeft;
            btn.Padding = new Padding(16, 0, 8, 0);
        }

        private static Color Shift(Color color, int delta)
        {
            return Color.FromArgb(
                Math.Max(0, Math.Min(255, color.R + delta)),
                Math.Max(0, Math.Min(255, color.G + delta)),
                Math.Max(0, Math.Min(255, color.B + delta)));
        }

        public static void FillCurrencyCombo(ComboBox combo, CurrencyType selected = CurrencyType.PRB)
        {
            combo.DropDownStyle = ComboBoxStyle.DropDownList;
            combo.Font = BodyFont;
            combo.Items.Clear();
            combo.Items.Add(new CurrencyItem(CurrencyType.PRB, "Рубли ПМР (PRB)"));
            combo.Items.Add(new CurrencyItem(CurrencyType.RUB, "Рубли РФ (RUB)"));
            combo.Items.Add(new CurrencyItem(CurrencyType.USD, "Доллары США (USD)"));
            combo.Items.Add(new CurrencyItem(CurrencyType.EUR, "Евро (EUR)"));
            combo.Items.Add(new CurrencyItem(CurrencyType.MDL, "Молдавские леи (MDL)"));
            combo.DisplayMember = "Display";
            combo.ValueMember = "Value";
            combo.SelectedIndex = Enum.IsDefined(typeof(CurrencyType), selected) ? (int)selected : 0;
        }

        public static CurrencyType GetSelectedCurrency(ComboBox combo)
        {
            var item = combo.SelectedItem as CurrencyItem;
            return item != null ? item.Value : CurrencyType.PRB;
        }

        /// <summary>Поле с подписью сверху. Возвращает панель, готовую для FlowLayoutPanel.</summary>
        public static Panel CreateFieldSection(string labelText, Control input, int bottomMargin = 0)
        {
            var section = new Panel
            {
                Margin = new Padding(0, 0, 0, bottomMargin)
            };

            var label = new Label
            {
                Text = labelText,
                Name = "fieldLabel",
                AutoSize = true,
                ForeColor = Theme.TextMuted,
                Font = FieldLabelFont,
                Location = new Point(0, 0)
            };

            input.Location = new Point(0, label.PreferredHeight + 8);

            section.Controls.Add(label);
            section.Controls.Add(input);

            int inputWidth = Math.Max(input.Width, input.PreferredSize.Width);
            int inputHeight = Math.Max(input.Height, input.PreferredSize.Height);
            section.Width = Math.Max(inputWidth, label.PreferredWidth);
            section.Height = input.Location.Y + inputHeight + 6;

            return section;
        }

        /// <summary>Компактная группа «Подпись: поле» для панели фильтров.</summary>
        public static FlowLayoutPanel CreateInlineField(string labelText, Control input)
        {
            var group = new FlowLayoutPanel
            {
                AutoSize = true,
                AutoSizeMode = AutoSizeMode.GrowAndShrink,
                WrapContents = false,
                FlowDirection = FlowDirection.LeftToRight,
                Margin = new Padding(0, 0, 18, 8)
            };

            var label = new Label
            {
                Text = labelText,
                Name = "fieldLabel",
                AutoSize = true,
                ForeColor = Theme.TextMuted,
                Font = FieldLabelFont,
                Margin = new Padding(0, 7, 8, 0)
            };

            input.Margin = new Padding(0, 3, 0, 0);

            group.Controls.Add(label);
            group.Controls.Add(input);
            return group;
        }

        /// <summary>Поясняющий текст с переносом строк.</summary>
        public static Label CreateHintLabel(string text, int maxWidth)
        {
            return new Label
            {
                Text = text,
                Name = "hint",
                AutoSize = true,
                MaximumSize = new Size(maxWidth, 0),
                ForeColor = Theme.TextMuted,
                Font = HintFont,
                Margin = new Padding(0, 0, 0, 16)
            };
        }

        public static bool TryParseAmount(string text, out decimal amount)
        {
            return TryParseAmount(text, out amount, false);
        }

        /// <summary>
        /// Понимает «1 234,56», «1234.56», «1,234.56» и «1.234,56».
        /// </summary>
        public static bool TryParseAmount(string text, out decimal amount, bool allowZero)
        {
            amount = 0m;
            if (string.IsNullOrWhiteSpace(text))
                return false;

            string value = text.Trim()
                .Replace("\u00A0", " ")
                .Replace(" ", "")
                .Replace("₽", "")
                .Replace("$", "")
                .Replace("€", "");

            int lastComma = value.LastIndexOf(',');
            int lastDot = value.LastIndexOf('.');

            if (lastComma >= 0 && lastDot >= 0)
            {
                if (lastComma > lastDot)
                    value = value.Replace(".", "").Replace(',', '.');
                else
                    value = value.Replace(",", "");
            }
            else if (lastComma >= 0)
            {
                value = value.Replace(',', '.');
            }

            if (!decimal.TryParse(value, NumberStyles.Number, CultureInfo.InvariantCulture, out amount))
                return false;

            return allowZero ? amount >= 0 : amount > 0;
        }

        public static bool TryParseNonNegativeAmount(string text, out decimal amount)
        {
            return TryParseAmount(text, out amount, true);
        }

        public static void ShowError(IWin32Window owner, string message)
        {
            MessageBox.Show(owner, message, "Внимание", MessageBoxButtons.OK, MessageBoxIcon.Warning);
        }

        public static void ShowInfo(IWin32Window owner, string message)
        {
            MessageBox.Show(owner, message, "Готово", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        private sealed class CurrencyItem
        {
            public CurrencyItem(CurrencyType value, string display)
            {
                Value = value;
                Display = display;
            }

            public CurrencyType Value { get; }
            public string Display { get; }
            public override string ToString() => Display;
        }
    }
}
