using System;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using Krypton.Toolkit;
using Revenuemanagement.Models;
using Revenuemanagement.Services;

namespace Revenuemanagement
{
    /// <summary>Редактирование дохода или расхода: категория, сумма, магазин, заметка.</summary>
    public class EditTransactionForm : Form
    {
        private readonly FinanceManager _manager;
        private readonly Transaction _tx;

        private KryptonComboBox cmbCategory;
        private KryptonComboBox cmbShop;
        private Control shopSection;
        private TextBox txtAmount;
        private TextBox txtCommission;
        private Label lblExpenseTotal;
        private KryptonComboBox cmbCurrency;
        private TextBox txtNote;
        private ToolTip _toolTip;

        public EditTransactionForm(FinanceManager manager, Transaction tx)
        {
            _manager = manager;
            _tx = tx;
            BuildUi();
        }

        private bool IsExpense => _tx.Type == TransactionType.Expense;

        private void BuildUi()
        {
            Text = "Изменить запись";
            StartPosition = FormStartPosition.CenterParent;
            FormBorderStyle = FormBorderStyle.FixedDialog;
            MaximizeBox = false;
            MinimizeBox = false;
            ClientSize = new Size(560, IsExpense ? 650 : 500);
            MinimumSize = Size;
            Padding = new Padding(32, 24, 32, 24);
            BackColor = Theme.BgMain;
            Font = UiHelper.BodyFont;

            var layout = new FlowLayoutPanel
            {
                Name = "dialogLayout",
                Dock = DockStyle.Fill,
                AutoScroll = true,
                FlowDirection = FlowDirection.TopDown,
                WrapContents = false,
                Padding = new Padding(0),
                Margin = new Padding(0)
            };

            var title = new Label
            {
                Text = IsExpense ? "✏  Изменить расход" : "✏  Изменить доход",
                AutoSize = true,
                Font = UiHelper.TitleFont,
                ForeColor = Theme.TextPrimary,
                Margin = new Padding(0, 0, 0, 10)
            };

            var hint = UiHelper.CreateHintLabel(
                "Исправьте категорию, сумму или заметку. Баланс пересчитается автоматически.",
                496);

            cmbCategory = new KryptonComboBox
            {
                Width = 430,
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

            cmbShop = new KryptonComboBox
            {
                Width = 496,
                DropDownStyle = ComboBoxStyle.DropDownList,
                Font = UiHelper.BodyFont
            };
            cmbShop.Items.AddRange(GroceryShops.All);
            if (!string.IsNullOrWhiteSpace(_tx.Shop) && cmbShop.Items.Contains(_tx.Shop))
                cmbShop.SelectedItem = _tx.Shop;
            else if (cmbShop.Items.Count > 0)
                cmbShop.SelectedIndex = 0;

            txtAmount = new TextBox
            {
                Width = 240,
                Font = UiHelper.BodyFont,
                Text = _tx.Amount.ToString("0.##"),
                TextAlign = HorizontalAlignment.Right
            };

            cmbCurrency = new KryptonComboBox
            {
                Width = 236,
                Font = UiHelper.BodyFont,
                DropDownStyle = ComboBoxStyle.DropDownList
            };
            UiHelper.FillCurrencyCombo(cmbCurrency, _tx.Currency);

            var amountSection = new FlowLayoutPanel
            {
                AutoSize = true,
                AutoSizeMode = AutoSizeMode.GrowAndShrink,
                WrapContents = false,
                Margin = new Padding(0, 0, 0, 18),
                Padding = new Padding(0, 0, 0, 5)
            };
            var amountWrap = UiHelper.CreateFieldSection(IsExpense ? "Сумма покупки" : "Сумма дохода", txtAmount);
            amountWrap.Margin = new Padding(0, 0, 20, 0);
            amountSection.Controls.Add(amountWrap);
            amountSection.Controls.Add(UiHelper.CreateFieldSection("Валюта", cmbCurrency));

            Control commissionSection = null;
            if (IsExpense)
            {
                txtCommission = new TextBox
                {
                    Width = 180,
                    Font = UiHelper.BodyFont,
                    Text = _tx.CommissionPercent > 0m ? _tx.CommissionPercent.ToString("0.##") : string.Empty
                };
                commissionSection = UiHelper.CreateFieldSection("Комиссия, % (необязательно)", txtCommission, 8);
                lblExpenseTotal = new Label
                {
                    AutoSize = true,
                    Font = UiHelper.HeaderFont,
                    ForeColor = Theme.AccentRed,
                    Margin = new Padding(0, 0, 0, 16)
                };
                txtAmount.TextChanged += (s, e) => UpdateExpenseTotalPreview();
                txtCommission.TextChanged += (s, e) => UpdateExpenseTotalPreview();
                cmbCurrency.SelectedIndexChanged += (s, e) => UpdateExpenseTotalPreview();
            }

            txtNote = new TextBox
            {
                Width = 496,
                Height = 70,
                Multiline = true,
                ScrollBars = ScrollBars.Vertical,
                Font = UiHelper.BodyFont,
                Text = _tx.Note ?? string.Empty
            };

            var buttons = new FlowLayoutPanel
            {
                AutoSize = true,
                AutoSizeMode = AutoSizeMode.GrowAndShrink,
                FlowDirection = FlowDirection.RightToLeft,
                WrapContents = false,
                Width = 496,
                Margin = new Padding(0, 20, 0, 0)
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

            var save = new Button
            {
                Text = "✔  Сохранить",
                Size = new Size(240, 44)
            };
            UiHelper.StyleFlatButton(save, Theme.AccentGreen, Color.White);
            save.TextAlign = ContentAlignment.MiddleCenter;
            save.Padding = Padding.Empty;
            save.Click += BtnOk_Click;

            buttons.Controls.Add(cancel);
            buttons.Controls.Add(save);

            shopSection = UiHelper.CreateFieldSection("Магазин для категории «Продукты»", cmbShop, 18);

            layout.Controls.Add(title);
            layout.Controls.Add(hint);
            layout.Controls.Add(UiHelper.CreateFieldSection("Категория", categoryInput, 18));
            layout.Controls.Add(shopSection);
            layout.Controls.Add(amountSection);
            if (IsExpense)
            {
                layout.Controls.Add(UiHelper.CreateHintLabel("Оставьте пустым, если комиссии нет.", 496));
                layout.Controls.Add(commissionSection);
                layout.Controls.Add(lblExpenseTotal);
            }
            layout.Controls.Add(UiHelper.CreateFieldSection("Заметка", txtNote, 0));
            layout.Controls.Add(buttons);
            Controls.Add(layout);

            AcceptButton = save;
            CancelButton = cancel;

            Theme.ApplyTo(this);
            UpdateShopVisibility();
            if (IsExpense)
                UpdateExpenseTotalPreview();

            Shown += (s, e) =>
            {
                txtAmount.Focus();
                txtAmount.SelectAll();
            };
        }

        private void ReloadCategories()
        {
            cmbCategory.Items.Clear();
            var categories = IsExpense
                ? _manager.GetExpenseCategories()
                : _manager.GetIncomeCategories();

            foreach (string category in categories)
                cmbCategory.Items.Add(category);

            if (!string.IsNullOrWhiteSpace(_tx.Category) && !cmbCategory.Items.Contains(_tx.Category))
                cmbCategory.Items.Add(_tx.Category);

            if (cmbCategory.Items.Count == 0)
                cmbCategory.Items.Add("Другое");

            if (!string.IsNullOrWhiteSpace(_tx.Category) && cmbCategory.Items.Contains(_tx.Category))
                cmbCategory.SelectedItem = _tx.Category;
            else if (cmbCategory.Items.Count > 0)
                cmbCategory.SelectedIndex = 0;
        }

        private void BtnAddCategory_Click(object sender, EventArgs e)
        {
            using (var dialog = new SimpleInputDialog(
                "Новая категория",
                "Введите название категории:"))
            {
                if (dialog.ShowDialog(this) != DialogResult.OK)
                    return;

                try
                {
                    string category = IsExpense
                        ? _manager.AddExpenseCategory(dialog.InputText)
                        : _manager.AddIncomeCategory(dialog.InputText);
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
            bool show = IsExpense &&
                        string.Equals(cmbCategory.Text, "Продукты", StringComparison.OrdinalIgnoreCase);
            shopSection.Visible = show;
            PerformLayout();
        }

        private void BtnOk_Click(object sender, EventArgs e)
        {
            try
            {
                if (cmbCategory.SelectedItem == null)
                    throw new InvalidOperationException("Выберите категорию.");

                if (!UiHelper.TryParseAmount(txtAmount.Text, out decimal amount))
                    throw new InvalidOperationException("Введите сумму больше нуля.");

                decimal commissionPercent = 0m;
                if (IsExpense && !string.IsNullOrWhiteSpace(txtCommission.Text) &&
                    !UiHelper.TryParseNonNegativeAmount(txtCommission.Text, out commissionPercent))
                    throw new InvalidOperationException("Введите комиссию в процентах, например 5.");

                string shop = shopSection.Visible ? cmbShop.Text : string.Empty;

                _manager.EditTransaction(
                    _tx.Id,
                    cmbCategory.SelectedItem.ToString(),
                    amount,
                    UiHelper.GetSelectedCurrency(cmbCurrency),
                    shop,
                    txtNote.Text.Trim(),
                    commissionPercent);

                DialogResult = DialogResult.OK;
            }
            catch (Exception ex)
            {
                UiHelper.ShowError(this, ex.Message);
            }
        }

        private void UpdateExpenseTotalPreview()
        {
            if (lblExpenseTotal == null)
                return;
            if (!UiHelper.TryParseAmount(txtAmount.Text, out decimal amount))
            {
                lblExpenseTotal.Text = "К списанию: введите сумму";
                return;
            }

            decimal commissionPercent = 0m;
            if (!string.IsNullOrWhiteSpace(txtCommission.Text) &&
                !UiHelper.TryParseNonNegativeAmount(txtCommission.Text, out commissionPercent))
            {
                lblExpenseTotal.Text = "Проверьте комиссию";
                return;
            }

            decimal total = amount + Math.Round(amount * commissionPercent / 100m, 2, MidpointRounding.AwayFromZero);
            lblExpenseTotal.Text = string.Format("К списанию: {0:N2} {1}",
                total, FinanceManager.CurrencyShort(UiHelper.GetSelectedCurrency(cmbCurrency)));
        }
    }
}
