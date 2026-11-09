using System;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using Revenuemanagement.Models;
using Revenuemanagement.Services;

namespace Revenuemanagement
{
    /// <summary>Просторный диалог добавления дохода или расхода.</summary>
    public class TransactionDialogForm : Form
    {
        private readonly FinanceManager _manager;
        private readonly bool _isIncome;

        private ComboBox cmbCategory;
        private ComboBox cmbShop;
        private TextBox txtAmount;
        private ComboBox cmbCurrency;
        private TextBox txtNote;
        private Control shopSection;
        private ToolTip _toolTip;

        public TransactionDialogForm(FinanceManager manager, bool isIncome)
        {
            _manager = manager;
            _isIncome = isIncome;
            BuildUi();
        }

        private void BuildUi()
        {
            Text = _isIncome ? "Добавить доход" : "Добавить расход";
            StartPosition = FormStartPosition.CenterParent;
            FormBorderStyle = FormBorderStyle.FixedDialog;
            MaximizeBox = false;
            MinimizeBox = false;
            ClientSize = new Size(820, _isIncome ? 580 : 660);
            MinimumSize = Size;
            Padding = new Padding(42, 32, 42, 32);
            BackColor = UiHelper.BgMain;
            Font = UiHelper.BodyFont;

            var layout = new FlowLayoutPanel
            {
                Dock = DockStyle.Fill,
                AutoScroll = true,
                FlowDirection = FlowDirection.TopDown,
                WrapContents = false,
                Padding = new Padding(0),
                Margin = new Padding(0)
            };

            var title = new Label
            {
                Text = _isIncome ? "Новый доход" : "Новый расход",
                AutoSize = true,
                Font = UiHelper.TitleFont,
                ForeColor = UiHelper.TextDark,
                Margin = new Padding(0, 0, 0, 10)
            };

            var hint = UiHelper.CreateHintLabel(
                "Заполните сумму, валюту и категорию. Категорию можно добавить кнопкой +.",
                632);

            cmbCategory = new ComboBox
            {
                Width = 570,
                DropDownStyle = ComboBoxStyle.DropDownList,
                Font = UiHelper.BodyFont
            };
            ReloadCategories();
            cmbCategory.SelectedIndexChanged += (s, e) => UpdateShopVisibility();

            var addCategory = new Button
            {
                Text = "+",
                Size = new Size(48, 36),
                Margin = new Padding(14, 0, 0, 0),
                Font = new Font("Segoe UI Semibold", 14F, FontStyle.Bold)
            };
            UiHelper.StyleFlatButton(addCategory, Theme.AccentBlue, Color.White);
            addCategory.TextAlign = ContentAlignment.MiddleCenter;
            addCategory.Padding = Padding.Empty;
            addCategory.Click += BtnAddCategory_Click;

            _toolTip = new ToolTip();
            _toolTip.SetToolTip(addCategory, "Добавить новую категорию");

            var categoryInput = new FlowLayoutPanel
            {
                AutoSize = true,
                AutoSizeMode = AutoSizeMode.GrowAndShrink,
                WrapContents = false,
                Margin = new Padding(0),
                Padding = new Padding(0, 0, 0, 5)
            };
            categoryInput.Controls.Add(cmbCategory);
            categoryInput.Controls.Add(addCategory);

            cmbShop = new ComboBox
            {
                Width = 632,
                DropDownStyle = ComboBoxStyle.DropDownList,
                Font = UiHelper.BodyFont
            };
            cmbShop.Items.AddRange(GroceryShops.All);
            if (cmbShop.Items.Count > 0)
                cmbShop.SelectedIndex = 0;

            txtAmount = new TextBox
            {
                Width = 300,
                Font = UiHelper.BodyFont,
                TextAlign = HorizontalAlignment.Right
            };
            cmbCurrency = new ComboBox
            {
                Width = 312,
                Font = UiHelper.BodyFont,
                DropDownStyle = ComboBoxStyle.DropDownList
            };
            UiHelper.FillCurrencyCombo(cmbCurrency);

            var amountSection = new FlowLayoutPanel
            {
                AutoSize = true,
                AutoSizeMode = AutoSizeMode.GrowAndShrink,
                WrapContents = false,
                Margin = new Padding(0, 0, 0, 18),
                Padding = new Padding(0, 0, 0, 5)
            };
            var amountWrap = UiHelper.CreateFieldSection("Сумма", txtAmount);
            amountWrap.Margin = new Padding(0, 0, 20, 0);
            amountSection.Controls.Add(amountWrap);
            amountSection.Controls.Add(UiHelper.CreateFieldSection("Валюта", cmbCurrency));

            txtNote = new TextBox
            {
                Width = 632,
                Height = 82,
                Multiline = true,
                ScrollBars = ScrollBars.Vertical,
                Font = UiHelper.BodyFont
            };

            var buttons = new FlowLayoutPanel
            {
                AutoSize = true,
                AutoSizeMode = AutoSizeMode.GrowAndShrink,
                FlowDirection = FlowDirection.RightToLeft,
                WrapContents = false,
                Width = 632,
                Margin = new Padding(0, 20, 0, 0)
            };

            var cancel = new Button
            {
                Text = "Отмена",
                Size = new Size(190, 48),
                Margin = new Padding(12, 0, 0, 0),
                DialogResult = DialogResult.Cancel
            };
            UiHelper.StyleFlatButton(cancel, Color.FromArgb(200, 205, 215), UiHelper.TextDark);
            cancel.TextAlign = ContentAlignment.MiddleCenter;
            cancel.Padding = Padding.Empty;

            var save = new Button
            {
                Text = _isIncome ? "Сохранить доход" : "Сохранить расход",
                Size = new Size(290, 48)
            };
            UiHelper.StyleFlatButton(save, _isIncome ? UiHelper.AccentGreen : UiHelper.AccentRed, Color.White);
            save.TextAlign = ContentAlignment.MiddleCenter;
            save.Padding = Padding.Empty;
            save.Click += BtnSave_Click;

            buttons.Controls.Add(cancel);
            buttons.Controls.Add(save);

            shopSection = UiHelper.CreateFieldSection(
                "Магазин для категории «Продукты»", cmbShop, 18);

            layout.Controls.Add(title);
            layout.Controls.Add(hint);
            layout.Controls.Add(UiHelper.CreateFieldSection("Категория", categoryInput, 18));
            layout.Controls.Add(shopSection);
            layout.Controls.Add(amountSection);
            layout.Controls.Add(UiHelper.CreateFieldSection("Заметка (необязательно)", txtNote, 0));
            layout.Controls.Add(buttons);
            Controls.Add(layout);

            AcceptButton = save;
            CancelButton = cancel;

            Theme.ApplyTo(this);
            UpdateShopVisibility();

            Shown += (s, e) => txtAmount.Focus();
        }

        private void ReloadCategories()
        {
            string previous = cmbCategory.Text;
            cmbCategory.Items.Clear();
            var categories = _isIncome ? _manager.GetIncomeCategories() : _manager.GetExpenseCategories();
            cmbCategory.Items.AddRange(categories.Cast<object>().ToArray());

            if (cmbCategory.Items.Count == 0)
                cmbCategory.Items.Add("Другое");

            if (!string.IsNullOrWhiteSpace(previous) && cmbCategory.Items.Contains(previous))
                cmbCategory.SelectedItem = previous;
            else if (cmbCategory.Items.Count > 0)
                cmbCategory.SelectedIndex = 0;
        }

        private void BtnAddCategory_Click(object sender, EventArgs e)
        {
            using (var dialog = new SimpleInputDialog(
                "Новая категория",
                "Введите название категории (например, «Маникюр»):"))
            {
                if (dialog.ShowDialog(this) != DialogResult.OK)
                    return;

                try
                {
                    string category = _isIncome
                        ? _manager.AddIncomeCategory(dialog.InputText)
                        : _manager.AddExpenseCategory(dialog.InputText);
                    ReloadCategories();
                    cmbCategory.SelectedItem = category;
                }
                catch (Exception exception)
                {
                    UiHelper.ShowError(this, exception.Message);
                }
            }
        }

        private void UpdateShopVisibility()
        {
            bool visible = !_isIncome &&
                           string.Equals(cmbCategory.Text, "Продукты", StringComparison.OrdinalIgnoreCase);
            shopSection.Visible = visible;
            PerformLayout();
        }

        private void BtnSave_Click(object sender, EventArgs e)
        {
            try
            {
                if (cmbCategory.SelectedItem == null)
                    throw new InvalidOperationException("Добавьте и выберите категорию.");
                if (!UiHelper.TryParseAmount(txtAmount.Text, out decimal amount))
                    throw new InvalidOperationException(
                        "Введите сумму больше нуля. Можно использовать точку или запятую.");

                string category = cmbCategory.SelectedItem.ToString();
                CurrencyType currency = UiHelper.GetSelectedCurrency(cmbCurrency);
                string note = txtNote.Text.Trim();
                string shop = shopSection.Visible ? cmbShop.Text : string.Empty;

                if (_isIncome)
                    _manager.AddIncome(category, amount, currency, note);
                else
                    _manager.AddExpense(category, amount, currency, note, shop);

                DialogResult = DialogResult.OK;
            }
            catch (Exception exception)
            {
                UiHelper.ShowError(this, exception.Message);
            }
        }
    }

    /// <summary>Небольшой диалог ввода новой пользовательской категории.</summary>
    public class SimpleInputDialog : Form
    {
        private readonly TextBox _textBox;

        public string InputText => _textBox.Text.Trim();

        public SimpleInputDialog(string title, string prompt, string defaultValue = "")
        {
            Text = title;
            StartPosition = FormStartPosition.CenterParent;
            FormBorderStyle = FormBorderStyle.FixedDialog;
            MaximizeBox = false;
            MinimizeBox = false;
            ClientSize = new Size(560, 230);
            Padding = new Padding(28);
            BackColor = Theme.BgMain;
            Font = UiHelper.BodyFont;

            var caption = new Label
            {
                Text = prompt,
                Location = new Point(28, 24),
                Size = new Size(504, 48),
                ForeColor = Theme.TextMuted,
                Font = UiHelper.BodyFont
            };

            _textBox = new TextBox
            {
                Location = new Point(28, 80),
                Width = 504,
                Font = UiHelper.BodyFont,
                Text = defaultValue ?? string.Empty,
                MaxLength = 40
            };

            var buttons = new FlowLayoutPanel
            {
                Location = new Point(28, 130),
                Size = new Size(504, 48),
                FlowDirection = FlowDirection.RightToLeft,
                WrapContents = false
            };

            var cancel = new Button
            {
                Text = "Отмена",
                Size = new Size(150, 44),
                Margin = new Padding(12, 0, 0, 0),
                DialogResult = DialogResult.Cancel
            };
            UiHelper.StyleFlatButton(cancel, Color.FromArgb(200, 205, 215), Theme.TextPrimary);
            cancel.TextAlign = ContentAlignment.MiddleCenter;
            cancel.Padding = Padding.Empty;

            var accept = new Button
            {
                Text = "Добавить",
                Size = new Size(150, 44)
            };
            UiHelper.StyleFlatButton(accept, Theme.AccentGreen, Color.White);
            accept.TextAlign = ContentAlignment.MiddleCenter;
            accept.Padding = Padding.Empty;
            accept.Click += (s, e) =>
            {
                if (string.IsNullOrWhiteSpace(_textBox.Text))
                {
                    UiHelper.ShowError(this, "Введите название.");
                    return;
                }
                DialogResult = DialogResult.OK;
            };

            buttons.Controls.Add(cancel);
            buttons.Controls.Add(accept);

            Controls.Add(caption);
            Controls.Add(_textBox);
            Controls.Add(buttons);

            AcceptButton = accept;
            CancelButton = cancel;

            Theme.ApplyTo(this);

            Shown += (s, e) =>
            {
                _textBox.Focus();
                _textBox.SelectAll();
            };
        }
    }
}
