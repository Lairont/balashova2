using System.Drawing;
using System.Windows.Forms;

namespace Revenuemanagement
{
    /// <summary>
    /// Светлая и тёмная темы. Цвета берутся из текущего режима.
    /// </summary>
    public static class Theme
    {
        public static bool IsDark { get; private set; }

        public static Color BgMain { get; private set; }
        public static Color BgDialog { get; private set; }
        public static Color BgInput { get; private set; }
        public static Color BgSidebar { get; private set; }
        public static Color BgCard { get; private set; }
        public static Color BgCardPrb { get; private set; }
        public static Color BgCardRub { get; private set; }
        public static Color BgCardUsd { get; private set; }
        public static Color BgCardEur { get; private set; }
        public static Color AccentPurple { get; private set; }
        public static Color BgGrid { get; private set; }
        public static Color BgGridAlt { get; private set; }
        public static Color BgHeader { get; private set; }
        public static Color TextPrimary { get; private set; }
        public static Color TextMuted { get; private set; }
        public static Color TextOnSidebar { get; private set; }
        public static Color AccentGreen { get; private set; }
        public static Color AccentRed { get; private set; }
        public static Color AccentBlue { get; private set; }
        public static Color AccentGold { get; private set; }
        public static Color SelectionBack { get; private set; }
        public static Color FooterBack { get; private set; }

        static Theme()
        {
            Apply(false);
        }

        public static void Apply(bool dark)
        {
            IsDark = dark;
            AccentGreen = Color.FromArgb(46, 160, 120);
            AccentRed = Color.FromArgb(210, 85, 85);
            AccentBlue = Color.FromArgb(70, 130, 180);
            AccentGold = Color.FromArgb(212, 160, 50);
            AccentPurple = Color.FromArgb(140, 100, 190);

            if (dark)
            {
                // Диалоги: #1E2329, поля ввода: #2A2F35
                BgMain = Color.FromArgb(0x1E, 0x23, 0x29);
                BgDialog = Color.FromArgb(0x1E, 0x23, 0x29);
                BgInput = Color.FromArgb(0x2A, 0x2F, 0x35);
                BgSidebar = Color.FromArgb(18, 22, 30);
                BgCard = Color.FromArgb(40, 46, 58);
                BgCardPrb = Color.FromArgb(32, 55, 48);
                BgCardRub = Color.FromArgb(32, 45, 62);
                BgCardUsd = Color.FromArgb(55, 48, 32);
                BgCardEur = Color.FromArgb(52, 42, 62);
                BgGrid = Color.FromArgb(36, 42, 54);
                BgGridAlt = Color.FromArgb(42, 48, 60);
                BgHeader = Color.FromArgb(22, 28, 38);
                TextPrimary = Color.White;
                TextMuted = Color.FromArgb(180, 188, 198);
                TextOnSidebar = Color.FromArgb(220, 228, 240);
                SelectionBack = Color.FromArgb(55, 75, 100);
                FooterBack = Color.FromArgb(22, 26, 34);
            }
            else
            {
                BgMain = Color.FromArgb(245, 248, 252);
                BgDialog = Color.FromArgb(245, 248, 252);
                BgInput = Color.White;
                BgSidebar = Color.FromArgb(36, 59, 85);
                BgCard = Color.White;
                BgCardPrb = Color.FromArgb(230, 245, 238);
                BgCardRub = Color.FromArgb(232, 240, 250);
                BgCardUsd = Color.FromArgb(255, 246, 230);
                BgCardEur = Color.FromArgb(238, 232, 250);
                BgGrid = Color.White;
                BgGridAlt = Color.FromArgb(248, 250, 252);
                BgHeader = Color.FromArgb(36, 59, 85);
                TextPrimary = Color.FromArgb(40, 50, 60);
                TextMuted = Color.FromArgb(110, 120, 135);
                TextOnSidebar = Color.White;
                SelectionBack = Color.FromArgb(210, 230, 245);
                FooterBack = Color.FromArgb(236, 240, 245);
            }
        }

        /// <summary>
        /// Рекурсивно применяет текущую тему ко всем контролам формы/панели.
        /// Кнопки с уже заданным цветным фоном (акцентные) не перекрашиваются.
        /// </summary>
        public static void ApplyTo(Control root)
        {
            if (root == null)
                return;

            ApplyToControl(root);

            foreach (Control child in root.Controls)
                ApplyTo(child);
        }

        private static void ApplyToControl(Control c)
        {
            if (c is Form || c is Panel || c is GroupBox || c is TableLayoutPanel || c is FlowLayoutPanel)
            {
                // Акцентные карточки главной формы не трогаем по имени
                if (c.Name == "cardPrb" || c.Name == "cardRub" || c.Name == "cardUsd" ||
                    c.Name == "cardEur" || c.Name == "panelSidebar")
                    return;

                c.BackColor = (c is Form) ? BgDialog : (c.Name == "panelAdd" || c is GroupBox ? BgCard : BgDialog);
                c.ForeColor = TextPrimary;
                return;
            }

            if (c is Label || c is CheckBox || c is RadioButton || c is LinkLabel)
            {
                c.ForeColor = c is Label && LooksLikeMutedLabel(c)
                    ? TextMuted
                    : TextPrimary;
                c.BackColor = Color.Transparent;
                return;
            }

            if (c is TextBox || c is MaskedTextBox || c is RichTextBox)
            {
                c.BackColor = BgInput;
                c.ForeColor = TextPrimary;
                return;
            }

            if (c is ComboBox combo)
            {
                combo.BackColor = BgInput;
                combo.ForeColor = TextPrimary;
                combo.FlatStyle = FlatStyle.Flat;
                return;
            }

            if (c is DateTimePicker picker)
            {
                picker.BackColor = BgInput;
                picker.ForeColor = TextPrimary;
                picker.CalendarForeColor = TextPrimary;
                picker.CalendarMonthBackground = BgInput;
                picker.CalendarTitleBackColor = BgHeader;
                picker.CalendarTitleForeColor = Color.White;
                picker.CalendarTrailingForeColor = TextMuted;
                return;
            }

            if (c is NumericUpDown || c is ListBox || c is CheckedListBox)
            {
                c.BackColor = BgInput;
                c.ForeColor = TextPrimary;
                return;
            }

            if (c is DataGridView grid)
            {
                grid.BackgroundColor = BgGrid;
                grid.DefaultCellStyle.BackColor = BgGrid;
                grid.DefaultCellStyle.ForeColor = TextPrimary;
                grid.DefaultCellStyle.SelectionBackColor = SelectionBack;
                grid.DefaultCellStyle.SelectionForeColor = TextPrimary;
                grid.AlternatingRowsDefaultCellStyle.BackColor = BgGridAlt;
                grid.AlternatingRowsDefaultCellStyle.ForeColor = TextPrimary;
                grid.EnableHeadersVisualStyles = false;
                grid.ColumnHeadersDefaultCellStyle.BackColor = BgHeader;
                grid.ColumnHeadersDefaultCellStyle.ForeColor = Color.White;
                grid.GridColor = IsDark ? Color.FromArgb(55, 60, 70) : Color.FromArgb(220, 225, 230);
                return;
            }

            if (c is Button btn)
            {
                // Акцентные кнопки (зелёные/красные/синие) оставляем как есть
                if (IsAccentButton(btn))
                    return;

                // Нейтральная «Отмена» и подобные
                btn.FlatStyle = FlatStyle.Flat;
                btn.FlatAppearance.BorderSize = 0;
                btn.BackColor = IsDark ? BgInput : Color.FromArgb(200, 205, 215);
                btn.ForeColor = TextPrimary;
                return;
            }

            // Прочие контролы — хотя бы читаемый текст
            if (!(c is ExpensePiePanel))
                c.ForeColor = TextPrimary;
        }

        private static bool LooksLikeMutedLabel(Control c)
        {
            string t = c.Text ?? string.Empty;
            // Подписи полей обычно короткие и заканчиваются на «:» или это tip
            return c.Name == "tip" ||
                   c.Name == "fieldLabel" ||
                   c.Name == "hint" ||
                   t.EndsWith(":") ||
                   t == "Имя" || t == "Сумма" || t == "Валюта" || t == "Кто кому" ||
                   t == "Период:" || t == "Категория" ||
                   t.IndexOf("необязательно", System.StringComparison.OrdinalIgnoreCase) >= 0 ||
                   t.IndexOf("Пример:", System.StringComparison.OrdinalIgnoreCase) >= 0 ||
                   t.IndexOf("Если случайно", System.StringComparison.OrdinalIgnoreCase) >= 0;
        }

        private static bool IsAccentButton(Button btn)
        {
            Color b = btn.BackColor;
            return ColorsClose(b, AccentGreen) ||
                   ColorsClose(b, AccentRed) ||
                   ColorsClose(b, AccentBlue) ||
                   ColorsClose(b, AccentGold);
        }

        private static bool ColorsClose(Color a, Color b)
        {
            return System.Math.Abs(a.R - b.R) < 8 &&
                   System.Math.Abs(a.G - b.G) < 8 &&
                   System.Math.Abs(a.B - b.B) < 8;
        }
    }
}
