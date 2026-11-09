using System.Drawing;
using System.Windows.Forms;
using Krypton.Toolkit;

namespace Revenuemanagement
{
    /// <summary>Цвета интерфейса для светлого и тёмного режимов.</summary>
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
        public static Color BgCardMdl { get; private set; }
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
        public static Color Border { get; private set; }

        static Theme()
        {
            Apply(false);
        }

        public static void Apply(bool dark)
        {
            IsDark = dark;
            AccentGreen = Color.FromArgb(48, 185, 125);
            AccentRed = Color.FromArgb(230, 86, 91);
            AccentBlue = Color.FromArgb(83, 145, 220);
            AccentGold = Color.FromArgb(224, 168, 71);
            AccentPurple = Color.FromArgb(166, 131, 226);

            if (dark)
            {
                BgMain = Color.FromArgb(26, 27, 32);
                BgDialog = Color.FromArgb(31, 33, 39);
                BgInput = Color.FromArgb(39, 42, 50);
                BgSidebar = Color.FromArgb(22, 24, 29);
                BgCard = Color.FromArgb(36, 38, 45);
                BgCardPrb = Color.FromArgb(31, 54, 45);
                BgCardRub = Color.FromArgb(31, 45, 63);
                BgCardUsd = Color.FromArgb(55, 46, 31);
                BgCardEur = Color.FromArgb(47, 39, 59);
                BgCardMdl = Color.FromArgb(32, 50, 51);
                BgGrid = Color.FromArgb(32, 34, 40);
                BgGridAlt = Color.FromArgb(37, 39, 46);
                BgHeader = Color.FromArgb(45, 48, 57);
                TextPrimary = Color.FromArgb(244, 245, 247);
                TextMuted = Color.FromArgb(166, 171, 181);
                TextOnSidebar = Color.FromArgb(240, 242, 246);
                SelectionBack = Color.FromArgb(54, 74, 99);
                FooterBack = Color.FromArgb(30, 32, 38);
                Border = Color.FromArgb(55, 58, 68);
            }
            else
            {
                BgMain = Color.FromArgb(244, 246, 249);
                BgDialog = Color.FromArgb(249, 250, 252);
                BgInput = Color.White;
                BgSidebar = Color.FromArgb(27, 36, 50);
                BgCard = Color.White;
                BgCardPrb = Color.FromArgb(229, 245, 237);
                BgCardRub = Color.FromArgb(231, 240, 251);
                BgCardUsd = Color.FromArgb(252, 244, 228);
                BgCardEur = Color.FromArgb(240, 234, 250);
                BgCardMdl = Color.FromArgb(229, 243, 241);
                BgGrid = Color.White;
                BgGridAlt = Color.FromArgb(247, 249, 252);
                BgHeader = Color.FromArgb(38, 49, 67);
                TextPrimary = Color.FromArgb(38, 43, 52);
                TextMuted = Color.FromArgb(105, 113, 126);
                TextOnSidebar = Color.White;
                SelectionBack = Color.FromArgb(218, 232, 248);
                FooterBack = Color.FromArgb(236, 239, 244);
                Border = Color.FromArgb(249, 250, 252);
            }
        }

        /// <summary>Применяет текущую палитру ко всем элементам формы.</summary>
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
                bool card = c.Name == "panelAdd" || c.Name == "panelGridHost" ||
                            c.Name.StartsWith("card") || c is GroupBox;
                c.BackColor = c is Form || c.Name == "dialogLayout" || c is TableLayoutPanel
                    ? BgDialog
                    : (card ? BgCard : Color.Transparent);
                c.ForeColor = TextPrimary;

                if (card)
                    UiHelper.ApplyRoundedCorners(c, 12);
                return;
            }

            if (c is Label || c is CheckBox || c is RadioButton || c is LinkLabel)
            {
                c.ForeColor = c is Label && LooksLikeMutedLabel(c) ? TextMuted : TextPrimary;
                c.BackColor = Color.Transparent;
                return;
            }

            if (c is TextBox || c is MaskedTextBox || c is RichTextBox)
            {
                c.BackColor = BgInput;
                c.ForeColor = TextPrimary;
                return;
            }

            if (c is KryptonComboBox kryptonCombo)
            {
                kryptonCombo.PaletteMode = PaletteMode.Global;
                kryptonCombo.BackColor = BgInput;
                kryptonCombo.ForeColor = TextPrimary;
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
                grid.BorderStyle = BorderStyle.None;
                grid.CellBorderStyle = DataGridViewCellBorderStyle.SingleHorizontal;
                grid.ColumnHeadersBorderStyle = DataGridViewHeaderBorderStyle.None;
                grid.DefaultCellStyle.Padding = new Padding(8, 0, 8, 0);
                grid.ColumnHeadersDefaultCellStyle.Padding = new Padding(10, 0, 8, 0);
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
                grid.GridColor = Border;
                return;
            }

            if (c is Button btn)
            {
                UiHelper.ApplyRoundedCorners(btn, 9);
                if (IsAccentButton(btn))
                    return;

                btn.FlatStyle = FlatStyle.Flat;
                btn.FlatAppearance.BorderSize = 0;
                btn.FlatAppearance.MouseOverBackColor = IsDark
                    ? Color.FromArgb(57, 61, 71)
                    : Color.FromArgb(224, 229, 237);
                btn.FlatAppearance.MouseDownBackColor = IsDark
                    ? Color.FromArgb(46, 50, 59)
                    : Color.FromArgb(213, 220, 230);
                btn.BackColor = IsDark ? Color.FromArgb(48, 52, 61) : Color.FromArgb(232, 236, 242);
                btn.ForeColor = TextPrimary;
                return;
            }

            if (!(c is ExpensePiePanel))
                c.ForeColor = TextPrimary;
        }

        private static bool LooksLikeMutedLabel(Control c)
        {
            string t = c.Text ?? string.Empty;
            return c.Name == "tip" || c.Name == "fieldLabel" || c.Name == "hint" ||
                   t.EndsWith(":") || t == "Имя" || t == "Сумма" || t == "Валюта" ||
                   t == "Кто кому" || t == "Период:" || t == "Категория" ||
                   t.IndexOf("необязательно", System.StringComparison.OrdinalIgnoreCase) >= 0 ||
                   t.IndexOf("Пример:", System.StringComparison.OrdinalIgnoreCase) >= 0 ||
                   t.IndexOf("Если случайно", System.StringComparison.OrdinalIgnoreCase) >= 0;
        }

        private static bool IsAccentButton(Button btn)
        {
            Color b = btn.BackColor;
            return ColorsClose(b, AccentGreen) || ColorsClose(b, AccentRed) ||
                   ColorsClose(b, AccentBlue) || ColorsClose(b, AccentGold);
        }

        private static bool ColorsClose(Color a, Color b)
        {
            return System.Math.Abs(a.R - b.R) < 8 &&
                   System.Math.Abs(a.G - b.G) < 8 &&
                   System.Math.Abs(a.B - b.B) < 8;
        }
    }
}
