using System;
using System.Drawing;
using System.Windows.Forms;
using Krypton.Toolkit;
using Revenuemanagement.Services;

namespace Revenuemanagement
{
    /// <summary>Диалог ручной правки балансов по всем валютам.</summary>
    public class EditBalanceForm : Form
    {
        private readonly FinanceManager _manager;
        private TextBox txtPrb;
        private TextBox txtRub;
        private TextBox txtUsd;
        private TextBox txtEur;

        public EditBalanceForm(FinanceManager manager)
        {
            _manager = manager;
            BuildUi();
        }

        private void BuildUi()
        {
            Text = "Изменить баланс";
            StartPosition = FormStartPosition.CenterParent;
            FormBorderStyle = FormBorderStyle.FixedDialog;
            MaximizeBox = false;
            MinimizeBox = false;
            ClientSize = new Size(580, 420);
            Padding = new Padding(30, 24, 30, 24);
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
                Text = "✏  Исправьте суммы на счетах",
                AutoSize = true,
                Font = UiHelper.TitleFont,
                ForeColor = Theme.TextPrimary,
                Margin = new Padding(0, 0, 0, 10)
            };

            var hint = UiHelper.CreateHintLabel(
                "Если случайно ввели неверную сумму — просто укажите правильный остаток. Значения не могут быть отрицательными.",
                500);

            var fields = new TableLayoutPanel
            {
                ColumnCount = 2,
                RowCount = 4,
                AutoSize = true,
                AutoSizeMode = AutoSizeMode.GrowAndShrink,
                Margin = new Padding(0, 0, 0, 20),
                Padding = new Padding(0)
            };
            fields.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 150F));
            fields.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 340F));
            for (int i = 0; i < 4; i++)
                fields.RowStyles.Add(new RowStyle(SizeType.AutoSize));

            txtPrb = AddBalanceRow(fields, "PRB (ПМР)", _manager.Balances.PRB);
            txtRub = AddBalanceRow(fields, "RUB (РФ)", _manager.Balances.RUB);
            txtUsd = AddBalanceRow(fields, "USD ($)", _manager.Balances.USD);
            txtEur = AddBalanceRow(fields, "EUR (€)", _manager.Balances.EUR);

            var buttons = new FlowLayoutPanel
            {
                AutoSize = true,
                AutoSizeMode = AutoSizeMode.GrowAndShrink,
                FlowDirection = FlowDirection.RightToLeft,
                WrapContents = false,
                Width = 490,
                Margin = new Padding(0, 8, 0, 0)
            };

            var btnCancel = new Button
            {
                Text = "Отмена",
                Size = new Size(140, 44),
                Margin = new Padding(12, 0, 0, 0)
            };
            UiHelper.StyleFlatButton(btnCancel, Color.FromArgb(200, 205, 215), Theme.TextPrimary);
            btnCancel.TextAlign = ContentAlignment.MiddleCenter;
            btnCancel.Padding = Padding.Empty;
            btnCancel.Click += (s, e) => DialogResult = DialogResult.Cancel;

            var btnOk = new Button
            {
                Text = "✔  Сохранить",
                Size = new Size(220, 44)
            };
            UiHelper.StyleFlatButton(btnOk, Theme.AccentGreen, Color.White);
            btnOk.TextAlign = ContentAlignment.MiddleCenter;
            btnOk.Padding = Padding.Empty;
            btnOk.Click += BtnOk_Click;

            buttons.Controls.Add(btnCancel);
            buttons.Controls.Add(btnOk);

            layout.Controls.Add(title);
            layout.Controls.Add(hint);
            layout.Controls.Add(fields);
            layout.Controls.Add(buttons);
            Controls.Add(layout);

            AcceptButton = btnOk;
            CancelButton = btnCancel;

            Theme.ApplyTo(this);

            Shown += (s, e) =>
            {
                txtPrb.Focus();
                txtPrb.SelectAll();
            };
        }

        private TextBox AddBalanceRow(TableLayoutPanel table, string caption, decimal value)
        {
            int row = table.RowCount - 1;
            for (int i = 0; i < table.RowCount; i++)
            {
                if (table.GetControlFromPosition(0, i) == null)
                {
                    row = i;
                    break;
                }
            }

            var label = new Label
            {
                Text = caption,
                Name = "fieldLabel",
                AutoSize = true,
                ForeColor = Theme.TextMuted,
                Font = UiHelper.FieldLabelFont,
                Anchor = AnchorStyles.Left,
                Margin = new Padding(0, 9, 10, 12)
            };

            var box = new TextBox
            {
                Width = 330,
                Font = UiHelper.BodyFont,
                Text = value.ToString("0.##"),
                TextAlign = HorizontalAlignment.Right,
                Margin = new Padding(0, 4, 0, 10)
            };

            table.Controls.Add(label, 0, row);
            table.Controls.Add(box, 1, row);
            return box;
        }

        private void BtnOk_Click(object sender, EventArgs e)
        {
            try
            {
                if (!UiHelper.TryParseNonNegativeAmount(txtPrb.Text, out decimal prb) ||
                    !UiHelper.TryParseNonNegativeAmount(txtRub.Text, out decimal rub) ||
                    !UiHelper.TryParseNonNegativeAmount(txtUsd.Text, out decimal usd) ||
                    !UiHelper.TryParseNonNegativeAmount(txtEur.Text, out decimal eur))
                {
                    UiHelper.ShowError(this,
                        "Введите числа не меньше нуля для всех валют.\n" +
                        "Можно использовать точку или запятую.");
                    return;
                }

                _manager.SetBalances(prb, rub, usd, eur);
                DialogResult = DialogResult.OK;
            }
            catch (Exception ex)
            {
                UiHelper.ShowError(this, ex.Message);
            }
        }
    }
}
