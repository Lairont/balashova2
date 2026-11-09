using System;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using Revenuemanagement.Models;
using Revenuemanagement.Services;

namespace Revenuemanagement
{
    /// <summary>
    /// Окно списка долгов: добавить, срок погашения и погасить.
    /// </summary>
    public class DebtsForm : Form
    {
        private static readonly Font BoldGridFont =
            new Font("Segoe UI Semibold", 10.5f, FontStyle.Bold);

        private readonly FinanceManager _manager;

        private DataGridView grid;
        private TextBox txtPerson;
        private TextBox txtAmount;
        private ComboBox cmbCurrency;
        private ComboBox cmbDirection;
        private DateTimePicker dtpDue;
        private CheckBox chkHasDue;
        private Button btnAdd;
        private Button btnPay;
        private Button btnClose;
        private Panel panelAdd;
        private Label lblTitle;

        public DebtsForm(FinanceManager manager)
        {
            _manager = manager;
            BuildUi();
            Theme.ApplyTo(this);
            // Акцентные кнопки после ApplyTo
            UiHelper.StyleFlatButton(btnAdd, Theme.AccentBlue, Color.White);
            btnAdd.TextAlign = ContentAlignment.MiddleCenter;
            btnAdd.Padding = Padding.Empty;
            UiHelper.StyleFlatButton(btnPay, Theme.AccentGreen, Color.White);
            btnPay.TextAlign = ContentAlignment.MiddleCenter;
            btnPay.Padding = Padding.Empty;
            RefreshGrid();
        }

        private void BuildUi()
        {
            Text = "Мои долги";
            StartPosition = FormStartPosition.CenterParent;
            FormBorderStyle = FormBorderStyle.FixedDialog;
            MaximizeBox = false;
            MinimizeBox = false;
            ClientSize = new Size(860, 650);
            BackColor = Theme.BgMain;
            Font = UiHelper.BodyFont;

            lblTitle = new Label
            {
                Text = "📒  Долги",
                Font = UiHelper.TitleFont,
                ForeColor = Theme.TextPrimary,
                Location = new Point(20, 16),
                AutoSize = true
            };

            var hint = UiHelper.CreateHintLabel(
                "Долг не меняет баланс, пока вы не погасите его. Просроченные долги выделены красным.",
                800);
            hint.Location = new Point(20, 50);

            grid = new DataGridView
            {
                Location = new Point(20, 92),
                Size = new Size(800, 288),
                ReadOnly = true,
                AllowUserToAddRows = false,
                AllowUserToDeleteRows = false,
                SelectionMode = DataGridViewSelectionMode.FullRowSelect,
                MultiSelect = false,
                AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill,
                BorderStyle = BorderStyle.None,
                RowHeadersVisible = false,
                Font = UiHelper.GridFont,
                RowTemplate = { Height = 34 },
                EnableHeadersVisualStyles = false
            };
            grid.ColumnHeadersHeight = 40;
            grid.CellFormatting += Grid_CellFormatting;
            grid.SelectionChanged += (s, e) => UpdatePayButton();

            panelAdd = new Panel
            {
                Name = "panelAdd",
                Location = new Point(20, 392),
                Size = new Size(800, 172)
            };

            var lblNew = new Label
            {
                Text = "Новый долг",
                Font = UiHelper.HeaderFont,
                Location = new Point(12, 6),
                AutoSize = true,
                Name = "lblNew"
            };

            var tipPerson = MakeFieldLabel("Имя", 12, 42);
            txtPerson = MakeTextBox(12, 62, 200);

            var tipAmount = MakeFieldLabel("Сумма", 230, 42);
            txtAmount = MakeTextBox(230, 62, 140);

            var tipCur = MakeFieldLabel("Валюта", 390, 42);
            cmbCurrency = new ComboBox
            {
                Location = new Point(390, 62),
                Size = new Size(160, 28),
                DropDownStyle = ComboBoxStyle.DropDownList,
                Font = UiHelper.BodyFont
            };
            UiHelper.FillCurrencyCombo(cmbCurrency);

            var tipDir = MakeFieldLabel("Кто кому", 570, 42);
            cmbDirection = new ComboBox
            {
                Location = new Point(570, 62),
                Size = new Size(218, 28),
                DropDownStyle = ComboBoxStyle.DropDownList,
                Font = UiHelper.BodyFont
            };
            cmbDirection.Items.Add("Мне должны");
            cmbDirection.Items.Add("Я должна");
            cmbDirection.SelectedIndex = 0;

            chkHasDue = new CheckBox
            {
                Text = "Погасить до:",
                Location = new Point(12, 116),
                AutoSize = true,
                Checked = true,
                Font = UiHelper.BodyFont
            };
            chkHasDue.CheckedChanged += (s, e) => dtpDue.Enabled = chkHasDue.Checked;

            dtpDue = new DateTimePicker
            {
                Location = new Point(170, 112),
                Size = new Size(160, 28),
                Format = DateTimePickerFormat.Short,
                Font = UiHelper.BodyFont,
                Value = DateTime.Today.AddDays(14)
            };

            btnAdd = new Button
            {
                Text = "+  Добавить долг",
                Location = new Point(570, 108),
                Size = new Size(218, 46)
            };
            btnAdd.Click += BtnAdd_Click;

            panelAdd.Controls.AddRange(new Control[]
            {
                lblNew, tipPerson, txtPerson, tipAmount, txtAmount,
                tipCur, cmbCurrency, tipDir, cmbDirection,
                chkHasDue, dtpDue, btnAdd
            });

            btnPay = new Button
            {
                Text = "✔  Погасить выбранный долг",
                Location = new Point(20, 580),
                Size = new Size(340, 48),
                Enabled = false
            };
            btnPay.Click += BtnPay_Click;

            btnClose = new Button
            {
                Text = "Закрыть",
                Location = new Point(660, 580),
                Size = new Size(180, 48)
            };
            UiHelper.StyleFlatButton(btnClose, Color.FromArgb(180, 185, 195), Theme.TextPrimary);
            btnClose.TextAlign = ContentAlignment.MiddleCenter;
            btnClose.Padding = Padding.Empty;
            btnClose.Click += (s, e) => Close();

            Controls.Add(lblTitle);
            Controls.Add(hint);
            Controls.Add(grid);
            Controls.Add(panelAdd);
            Controls.Add(btnPay);
            Controls.Add(btnClose);
        }

        private static Label MakeFieldLabel(string text, int left, int top)
        {
            return new Label
            {
                Text = text,
                Name = "fieldLabel",
                Location = new Point(left, top),
                AutoSize = true,
                Font = UiHelper.FieldLabelFont,
                ForeColor = Theme.TextMuted
            };
        }

        private static TextBox MakeTextBox(int left, int top, int width)
        {
            return new TextBox
            {
                Location = new Point(left, top),
                Size = new Size(width, 28),
                Font = UiHelper.BodyFont
            };
        }

        private void RefreshGrid()
        {
            var rows = _manager.GetAllDebts()
                .Where(d => !string.IsNullOrWhiteSpace(d.PersonName) && d.Amount > 0)
                .OrderBy(d => d.IsPaid)
                .ThenBy(d => d.DueDate ?? DateTime.MaxValue)
                .ThenByDescending(d => d.CreatedDate)
                .Select(d => new
                {
                    d.Id,
                    Человек = d.PersonName,
                    Направление = d.DirectionDisplay,
                    Сумма = d.Amount,
                    Валюта = d.CurrencyDisplay,
                    Погасить_до = d.DueDateDisplay,
                    Статус = d.IsOverdue ? "Просрочен!" : d.StatusDisplay,
                    d.IsPaid,
                    d.IsOverdue,
                    Дата = d.CreatedDate.ToString("dd.MM.yyyy")
                })
                .ToList();

            grid.DataSource = null;
            grid.DataSource = rows;

            HideColumn("Id");
            HideColumn("IsPaid");
            HideColumn("IsOverdue");

            if (grid.Columns["Погасить_до"] != null)
                grid.Columns["Погасить_до"].HeaderText = "Погасить до";

            if (grid.Columns["Сумма"] != null)
            {
                grid.Columns["Сумма"].DefaultCellStyle.Format = "N2";
                grid.Columns["Сумма"].DefaultCellStyle.Alignment =
                    DataGridViewContentAlignment.MiddleRight;
            }

            SetFillWeight("Человек", 24);
            SetFillWeight("Направление", 19);
            SetFillWeight("Сумма", 15);
            SetFillWeight("Валюта", 10);
            SetFillWeight("Погасить_до", 15);
            SetFillWeight("Статус", 17);
            SetFillWeight("Дата", 15);

            grid.ClearSelection();
            grid.CurrentCell = null;
            UpdatePayButton();
        }

        private void HideColumn(string name)
        {
            if (grid.Columns[name] != null)
                grid.Columns[name].Visible = false;
        }

        private void SetFillWeight(string name, float weight)
        {
            if (grid.Columns[name] != null)
                grid.Columns[name].FillWeight = weight;
        }

        private void UpdatePayButton()
        {
            if (btnPay == null || grid == null)
                return;

            bool canPay = false;
            var row = grid.CurrentRow;
            if (row != null && grid.Columns["IsPaid"] != null)
            {
                object paid = row.Cells["IsPaid"].Value;
                canPay = !(paid is bool value && value);
            }

            btnPay.Enabled = canPay;
        }

        private void Grid_CellFormatting(object sender, DataGridViewCellFormattingEventArgs e)
        {
            if (e.RowIndex < 0 || e.ColumnIndex < 0)
                return;
            if (grid.Columns["IsPaid"] == null || grid.Columns["IsOverdue"] == null)
                return;
            if (e.RowIndex >= grid.Rows.Count)
                return;

            var row = grid.Rows[e.RowIndex];
            bool paid = row.Cells["IsPaid"].Value is bool paidValue && paidValue;
            bool overdue = row.Cells["IsOverdue"].Value is bool overdueValue && overdueValue;

            if (paid)
            {
                e.CellStyle.ForeColor = Theme.TextMuted;
                return;
            }

            if (overdue)
            {
                e.CellStyle.ForeColor = Theme.AccentRed;
                string columnName = grid.Columns[e.ColumnIndex].Name;
                if (columnName == "Статус" || columnName == "Погасить_до")
                    e.CellStyle.Font = BoldGridFont;
            }
        }

        private void BtnAdd_Click(object sender, EventArgs e)
        {
            try
            {
                if (!UiHelper.TryParseAmount(txtAmount.Text, out decimal amount))
                {
                    UiHelper.ShowError(this, "Введите сумму больше нуля.");
                    return;
                }

                DateTime? due = chkHasDue.Checked ? dtpDue.Value.Date : (DateTime?)null;
                bool isOwedToMe = cmbDirection.SelectedIndex == 0;

                _manager.AddDebt(
                    txtPerson.Text,
                    amount,
                    UiHelper.GetSelectedCurrency(cmbCurrency),
                    isOwedToMe,
                    due);

                txtPerson.Clear();
                txtAmount.Clear();
                RefreshGrid();
                UiHelper.ShowInfo(this, "Долг добавлен.");
            }
            catch (Exception ex)
            {
                UiHelper.ShowError(this, ex.Message);
            }
        }

        private void BtnPay_Click(object sender, EventArgs e)
        {
            try
            {
                if (grid.CurrentRow == null)
                {
                    UiHelper.ShowError(this, "Выберите долг в таблице.");
                    return;
                }

                if (!(grid.CurrentRow.Cells["Id"].Value is Guid id))
                    return;

                var debt = _manager.GetAllDebts().FirstOrDefault(d => d.Id == id);
                if (debt == null)
                    return;

                if (debt.IsPaid)
                {
                    UiHelper.ShowError(this, "Этот долг уже погашен.");
                    return;
                }

                string dueInfo = debt.DueDate.HasValue
                    ? "\nСрок: " + debt.DueDate.Value.ToString("dd.MM.yyyy")
                    : "";
                if (debt.IsOverdue)
                    dueInfo += " (просрочен)";

                string ask = debt.IsOwedToMe
                    ? string.Format("Получить {0:N2} {1} от «{2}» на баланс?{3}",
                        debt.Amount, debt.CurrencyDisplay, debt.PersonName, dueInfo)
                    : string.Format("Отдать {0:N2} {1} человеку «{2}» с баланса?{3}",
                        debt.Amount, debt.CurrencyDisplay, debt.PersonName, dueInfo);

                if (MessageBox.Show(this, ask, "Погашение долга",
                        MessageBoxButtons.YesNo, MessageBoxIcon.Question) != DialogResult.Yes)
                    return;

                _manager.PayDebt(id);
                RefreshGrid();
                UiHelper.ShowInfo(this, "Долг погашен. Баланс обновлён.");
            }
            catch (Exception ex)
            {
                UiHelper.ShowError(this, ex.Message);
            }
        }
    }
}
