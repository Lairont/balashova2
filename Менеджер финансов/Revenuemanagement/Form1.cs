using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Runtime.InteropServices;
using System.Windows.Forms;
using Revenuemanagement.Models;
using Revenuemanagement.Services;

namespace Revenuemanagement
{
    public partial class Form1 : Form
    {
        private FinanceManager _manager;
        private bool _gridReady;
        private List<HistoryRow> _filteredRows = new List<HistoryRow>();
        private DateTimePicker _dayFilter;
        private bool _refreshPending;
        private bool _filterBuilt;
        private bool _adjustingFilterHeight;
        private Button _btnExportExcel;
        private Panel _cardMdl;
        private Label _lblMdlAmount;
        private string _sortColumnName;
        private bool _sortAscending = true;

        private const int EM_SETCUEBANNER = 0x1501;

        [DllImport("user32.dll", CharSet = CharSet.Unicode)]
        private static extern IntPtr SendMessage(IntPtr hWnd, int msg, IntPtr wParam, string lParam);

        public Form1()
        {
            InitializeComponent();
            AddMdlBalanceCard();
            ConfigureExactDayFilter();
            BuildFilterBar();
            gridHistory.ColumnHeaderMouseClick += GridHistory_ColumnHeaderMouseClick;

            cmbPeriod.SelectedIndex = 0;
            cmbTypeFilter.SelectedIndex = 0;

            DoubleBuffered = true;
            lblWelcome.AutoEllipsis = true;
            lblAppSubtitle.AutoEllipsis = true;

            try
            {
                Icon = System.Drawing.Icon.ExtractAssociatedIcon(Application.ExecutablePath);
            }
            catch
            {
                // Иконка не критична — просто останется стандартная.
            }
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            try
            {
                _manager = new FinanceManager();
            }
            catch (Exception ex)
            {
                MessageBox.Show(this, "Не удалось загрузить данные:\n" + ex.Message, "Ошибка",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);

                try
                {
                    _manager = new FinanceManager(new JsonDataStore("finance_data_new.json"));
                }
                catch (Exception second)
                {
                    MessageBox.Show(this, "Не удалось создать новый файл данных:\n" + second.Message,
                        "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    _manager = null;
                }
            }

            if (_manager == null)
            {
                btnIncome.Enabled = false;
                btnExpense.Enabled = false;
                btnExchange.Enabled = false;
                btnDebts.Enabled = false;
                btnEditBalance.Enabled = false;
                btnDailyExpenses.Enabled = false;
                btnPrintExpenses.Enabled = false;
                if (_btnExportExcel != null) _btnExportExcel.Enabled = false;
                return;
            }

            Theme.Apply(_manager.IsDarkTheme);
            Program.ApplyKryptonTheme(_manager.IsDarkTheme);
            ApplyThemeToForm();
            AdjustFilterBarHeight();
            ConfigureGridWrap();
            ApplySearchCue();

            if (!string.IsNullOrWhiteSpace(_manager.LoadWarning))
                UiHelper.ShowInfo(this, _manager.LoadWarning);

            RefreshDashboard();
        }

        private void Form1_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (_manager == null)
                return;

            while (true)
            {
                try
                {
                    _manager.Save();
                    return;
                }
                catch (Exception ex)
                {
                    DialogResult result = MessageBox.Show(this,
                        "Не удалось сохранить данные:\n" + ex.Message +
                        "\n\nДа — попробовать ещё раз.\nНет — закрыть без сохранения.\nОтмена — остаться в программе.",
                        "Ошибка сохранения",
                        MessageBoxButtons.YesNoCancel,
                        MessageBoxIcon.Error);

                    if (result == DialogResult.Yes)
                        continue;
                    if (result == DialogResult.No)
                        return;

                    e.Cancel = true;
                    return;
                }
            }
        }

        private void ConfigureGridWrap()
        {
            gridHistory.DefaultCellStyle.WrapMode = DataGridViewTriState.False;
            gridHistory.AutoSizeRowsMode = DataGridViewAutoSizeRowsMode.AllCells;
            gridHistory.RowTemplate.Height = 34;
            gridHistory.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.None;
        }

        private void ApplyThemeToForm()
        {
            BackColor = Theme.BgMain;
            panelMain.BackColor = Theme.BgMain;
            layoutRoot.BackColor = Theme.BgMain;
            panelSidebar.BackColor = Theme.BgSidebar;
            sidebarLayout.BackColor = Theme.BgSidebar;
            sidebarMenu.BackColor = Theme.BgSidebar;
            panelFooter.BackColor = Theme.FooterBack;
            panelFilter.BackColor = Theme.BgMain;
            filterFlow.BackColor = Theme.BgMain;
            panelContent.BackColor = Theme.BgMain;
            panelContent.ColumnStyles[0].SizeType = SizeType.Percent;
            panelContent.ColumnStyles[0].Width = 76f;
            panelContent.ColumnStyles[1].SizeType = SizeType.Percent;
            panelContent.ColumnStyles[1].Width = 24f;
            panelGridHost.BackColor = Theme.BgGrid;
            panelPieHost.BackColor = Theme.BgMain;
            pieExpenses.BackColor = Theme.BgCard;
            cardsLayout.BackColor = Theme.BgMain;

            lblAppTitle.ForeColor = Theme.TextOnSidebar;
            lblAppSubtitle.ForeColor = Color.FromArgb(180, 200, 220);
            lblWelcome.ForeColor = Theme.TextPrimary;
            lblHistoryTitle.ForeColor = Theme.TextPrimary;
            lblCopyright.ForeColor = Theme.TextMuted;
            lblDataPath.ForeColor = Theme.TextMuted;

            lblWelcome.Font = UiHelper.TitleFont;
            lblHistoryTitle.Font = UiHelper.HeaderFont;
            lblPrbTitle.Font = new Font("Segoe UI Semibold", 10f, FontStyle.Bold);
            lblRubTitle.Font = new Font("Segoe UI Semibold", 10f, FontStyle.Bold);
            lblUsdTitle.Font = new Font("Segoe UI Semibold", 10f, FontStyle.Bold);
            lblEurTitle.Font = new Font("Segoe UI Semibold", 10f, FontStyle.Bold);
            lblPrbTitle.AutoEllipsis = true;
            lblRubTitle.AutoEllipsis = true;
            lblUsdTitle.AutoEllipsis = true;
            lblEurTitle.AutoEllipsis = true;

            StyleSidebarButton(btnIncome, "➕  Добавить доход");
            StyleSidebarButton(btnExpense, "➖  Добавить расход");
            StyleSidebarButton(btnExchange, "💱  Обмен валют");
            StyleSidebarButton(btnDebts, "📒  Мои долги");
            StyleSidebarButton(btnEditBalance, "✏  Изменить баланс");
            StyleSidebarButton(btnTheme, Theme.IsDark ? "☀  Светлая тема" : "🌙  Тёмная тема");

            cardPrb.BackColor = Theme.BgCardPrb;
            cardRub.BackColor = Theme.BgCardRub;
            cardUsd.BackColor = Theme.BgCardUsd;
            cardEur.BackColor = Theme.BgCardEur;
            UiHelper.ApplyRoundedCorners(cardPrb, 14);
            UiHelper.ApplyRoundedCorners(cardRub, 14);
            UiHelper.ApplyRoundedCorners(cardUsd, 14);
            UiHelper.ApplyRoundedCorners(cardEur, 14);
            UiHelper.ApplyRoundedCorners(panelGridHost, 14);
            UiHelper.ApplyRoundedCorners(pieExpenses, 14);

            lblPrbTitle.ForeColor = Theme.AccentGreen;
            lblRubTitle.ForeColor = Theme.AccentBlue;
            lblUsdTitle.ForeColor = Theme.AccentGold;
            lblEurTitle.ForeColor = Theme.AccentPurple;
            lblPrbAmount.ForeColor = Theme.TextPrimary;
            lblRubAmount.ForeColor = Theme.TextPrimary;
            lblUsdAmount.ForeColor = Theme.TextPrimary;
            lblEurAmount.ForeColor = Theme.TextPrimary;
            if (_cardMdl != null)
            {
                var mdlTitle = (Label)_cardMdl.Controls["lblMdlTitle"];
                _cardMdl.BackColor = Theme.BgCardMdl;
                UiHelper.ApplyRoundedCorners(_cardMdl, 14);
                mdlTitle.Font = new Font("Segoe UI Semibold", 10f, FontStyle.Bold);
                mdlTitle.AutoEllipsis = true;
                _lblMdlAmount.ForeColor = Theme.TextPrimary;
                mdlTitle.ForeColor = Theme.AccentBlue;
            }
            lblPrbAmount.Font = UiHelper.CardAmountFont;
            lblRubAmount.Font = UiHelper.CardAmountFont;
            lblUsdAmount.Font = UiHelper.CardAmountFont;
            lblEurAmount.Font = UiHelper.CardAmountFont;
            panelGridHost.Padding = new Padding(1);

            gridHistory.EnableHeadersVisualStyles = false;
            gridHistory.BackgroundColor = Theme.BgGrid;
            gridHistory.DefaultCellStyle.BackColor = Theme.BgGrid;
            gridHistory.DefaultCellStyle.ForeColor = Theme.TextPrimary;
            gridHistory.DefaultCellStyle.SelectionBackColor = Theme.SelectionBack;
            gridHistory.DefaultCellStyle.SelectionForeColor = Theme.TextPrimary;
            gridHistory.DefaultCellStyle.Font = UiHelper.GridFont;
            gridHistory.DefaultCellStyle.Padding = new Padding(8, 0, 8, 0);
            gridHistory.AlternatingRowsDefaultCellStyle.BackColor = Theme.BgGridAlt;
            gridHistory.AlternatingRowsDefaultCellStyle.ForeColor = Theme.TextPrimary;
            gridHistory.ColumnHeadersDefaultCellStyle.BackColor = Theme.BgHeader;
            gridHistory.ColumnHeadersDefaultCellStyle.ForeColor = Color.White;
            gridHistory.ColumnHeadersDefaultCellStyle.Font = new Font("Segoe UI Semibold", 10.5f, FontStyle.Bold);
            gridHistory.ColumnHeadersDefaultCellStyle.Padding = new Padding(10, 0, 8, 0);
            gridHistory.ColumnHeadersDefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleLeft;
            gridHistory.ColumnHeadersHeight = 42;
            gridHistory.ColumnHeadersBorderStyle = DataGridViewHeaderBorderStyle.None;
            gridHistory.CellBorderStyle = DataGridViewCellBorderStyle.SingleHorizontal;
            gridHistory.GridColor = Theme.Border;

            lblEmptyState.BackColor = Theme.BgGrid;
            lblEmptyState.ForeColor = Theme.TextMuted;

            UiHelper.StyleFlatButton(btnDailyExpenses, Theme.AccentBlue, Color.White);
            btnDailyExpenses.TextAlign = ContentAlignment.MiddleCenter;
            btnDailyExpenses.Padding = Padding.Empty;

            UiHelper.StyleFlatButton(btnPrintExpenses, Theme.AccentGold, Color.White);
            btnPrintExpenses.TextAlign = ContentAlignment.MiddleCenter;
            btnPrintExpenses.Padding = Padding.Empty;
            if (_btnExportExcel != null)
            {
                UiHelper.StyleFlatButton(_btnExportExcel, Theme.AccentGreen, Color.White);
                _btnExportExcel.TextAlign = ContentAlignment.MiddleCenter;
                _btnExportExcel.Padding = Padding.Empty;
            }

            // Общие стили для полей фильтра и подписей
            Theme.ApplyTo(panelFilter);

            ctxHistory.BackColor = Theme.BgCard;
            ctxHistory.ForeColor = Theme.TextPrimary;
            ctxHistory.ShowImageMargin = false;
            menuEdit.ForeColor = Theme.TextPrimary;
            menuDelete.ForeColor = Theme.TextPrimary;

            filterFlow.PerformLayout();
            panelFilter.PerformLayout();
            layoutRoot.PerformLayout();
        }

        private static void StyleSidebarButton(Button btn, string text)
        {
            btn.Text = text;
            btn.FlatStyle = FlatStyle.Flat;
            btn.FlatAppearance.BorderSize = 0;
            btn.UseVisualStyleBackColor = false;
            btn.FlatAppearance.MouseOverBackColor = Theme.IsDark
                ? Color.FromArgb(40, 50, 70)
                : Color.FromArgb(50, 80, 115);
            btn.FlatAppearance.MouseDownBackColor = Theme.IsDark
                ? Color.FromArgb(30, 38, 54)
                : Color.FromArgb(44, 70, 100);
            btn.BackColor = Color.Transparent;
            btn.ForeColor = Theme.TextOnSidebar;
            btn.Font = new Font("Segoe UI Semibold", 10.5f, FontStyle.Bold);
            btn.TextAlign = ContentAlignment.MiddleLeft;
            btn.Padding = new Padding(12, 0, 4, 0);
            btn.Cursor = Cursors.Hand;
            UiHelper.ApplyRoundedCorners(btn, 9, false);
        }

        private HistoryPeriod CurrentPeriod
        {
            get
            {
                switch (cmbPeriod.SelectedIndex)
                {
                    case 1: return HistoryPeriod.ThisMonth;
                    case 2: return HistoryPeriod.LastMonth;
                    default: return HistoryPeriod.AllTime;
                }
            }
        }

        private HistoryTypeFilter CurrentTypeFilter
        {
            get
            {
                switch (cmbTypeFilter.SelectedIndex)
                {
                    case 1: return HistoryTypeFilter.Income;
                    case 2: return HistoryTypeFilter.Expense;
                    case 3: return HistoryTypeFilter.Debts;
                    case 4: return HistoryTypeFilter.Exchange;
                    default: return HistoryTypeFilter.All;
                }
            }
        }

        private string PeriodTitle
        {
            get
            {
                if (ExactDay.HasValue)
                    return "За " + ExactDay.Value.ToString("dd.MM.yyyy");
                switch (cmbPeriod.SelectedIndex)
                {
                    case 1: return "Этот месяц";
                    case 2: return "Прошлый месяц";
                    default: return "За всё время";
                }
            }
        }

        private void RefreshDashboard()
        {
            if (_manager == null)
                return;

            var b = _manager.Balances;
            lblPrbAmount.Text = b.PRB.ToString("N2");
            lblRubAmount.Text = b.RUB.ToString("N2");
            lblUsdAmount.Text = b.USD.ToString("N2");
            lblEurAmount.Text = b.EUR.ToString("N2");
            if (_lblMdlAmount != null) _lblMdlAmount.Text = b.MDL.ToString("N2");

            string owner = _manager.OwnerName;
            lblWelcome.Text = "Привет, " + owner + "! Вот ваши финансы";
            lblAppSubtitle.Text = "Личные финансы — " + owner;
            Text = "Кошелёк " + owner + " — учёт финансов";
            lblDataPath.Text = "Файл данных: " + _manager.DataFilePath;
            lblCopyright.Text = "Банчу.Н&Белько.В© 2026";

            cmbPeriod.Enabled = !ExactDay.HasValue;

            BindHistoryGrid();
            pieExpenses.SetData(_manager.GetExpenseTotalsByCategory(EffectivePeriod, ExactDay));
        }

        private void BindHistoryGrid()
        {
            if (_manager == null)
                return;

            IEnumerable<Transaction> query = _manager.GetTransactions(EffectivePeriod, ExactDay);

            switch (CurrentTypeFilter)
            {
                case HistoryTypeFilter.Income:
                    query = query.Where(t => t.Type == TransactionType.Income);
                    break;
                case HistoryTypeFilter.Expense:
                    query = query.Where(t => t.Type == TransactionType.Expense);
                    break;
                case HistoryTypeFilter.Debts:
                    query = query.Where(t =>
                        t.Type == TransactionType.DebtCreated ||
                        t.Type == TransactionType.DebtPaid);
                    break;
                case HistoryTypeFilter.Exchange:
                    query = query.Where(t => t.Type == TransactionType.Exchange);
                    break;
            }

            string search = (txtSearch.Text ?? string.Empty).Trim();
            if (search.Length > 0)
            {
                query = query.Where(t =>
                    ContainsIgnoreCase(t.Category, search) ||
                    ContainsIgnoreCase(t.Note, search) ||
                    ContainsIgnoreCase(t.Shop, search) ||
                    ContainsIgnoreCase(t.CategoryDisplay, search));
            }

            _filteredRows = query
                .Select(t => new HistoryRow
                {
                    Id = t.Id,
                    Type = t.Type,
                    Amount = t.Type == TransactionType.Expense ? t.TotalAmount : t.Amount,
                    Currency = t.Currency,
                    Дата = t.Date.ToString("dd.MM.yyyy HH:mm"),
                    Тип = t.TypeDisplay,
                    Категория = t.CategoryDisplay,
                    Магазин = t.Shop ?? string.Empty,
                    Сумма = (t.Type == TransactionType.Expense ? t.TotalAmount : t.Amount) > 0
                        ? (t.Type == TransactionType.Expense ? t.TotalAmount : t.Amount).ToString("N2")
                        : "—",
                    Валюта = t.Amount > 0 ? t.CurrencyDisplay : "",
                    Заметка = FormatTransactionNote(t)
                })
                .ToList();

            if (!string.IsNullOrEmpty(_sortColumnName))
                _filteredRows = SortHistoryRows(_filteredRows, _sortColumnName, _sortAscending);

            _gridReady = false;
            gridHistory.DataSource = null;
            gridHistory.Columns.Clear();
            gridHistory.DataSource = _filteredRows;

            if (gridHistory.Columns["Id"] != null)
                gridHistory.Columns["Id"].Visible = false;
            if (gridHistory.Columns["Type"] != null)
                gridHistory.Columns["Type"].Visible = false;
            if (gridHistory.Columns["Amount"] != null)
                gridHistory.Columns["Amount"].Visible = false;
            if (gridHistory.Columns["Currency"] != null)
                gridHistory.Columns["Currency"].Visible = false;
            if (gridHistory.Columns["Магазин"] != null)
                gridHistory.Columns["Магазин"].Visible = false;

            AddDeleteColumn();
            ApplyGridColumnLayout();
            foreach (DataGridViewColumn column in gridHistory.Columns)
                column.SortMode = DataGridViewColumnSortMode.Programmatic;
            if (!string.IsNullOrEmpty(_sortColumnName) && gridHistory.Columns[_sortColumnName] != null)
                gridHistory.Columns[_sortColumnName].HeaderCell.SortGlyphDirection = _sortAscending
                    ? SortOrder.Ascending : SortOrder.Descending;

            if (gridHistory.Columns["Сумма"] != null)
                gridHistory.Columns["Сумма"].DefaultCellStyle.Font =
                    new Font("Segoe UI Semibold", 10.5f, FontStyle.Bold);

            _gridReady = true;

            bool empty = _filteredRows.Count == 0;
            bool hasFilters = !string.IsNullOrWhiteSpace(search) ||
                              CurrentTypeFilter != HistoryTypeFilter.All ||
                              ExactDay.HasValue;

            lblEmptyState.Text = hasFilters
                ? "Ничего не найдено.\r\n\r\nИзмените фильтр или поиск."
                : "Пока нет операций.\r\n\r\nДобавьте доход или расход кнопками слева.";

            lblEmptyState.Visible = empty;
            gridHistory.Visible = !empty;
        }

        private static string FormatTransactionNote(Transaction transaction)
        {
            string note = transaction.Note ?? string.Empty;
            if (transaction.Type != TransactionType.Expense || transaction.CommissionPercent <= 0m)
                return note;

            string commission = string.Format("Комиссия {0:0.##}%: {1:N2} {2}",
                transaction.CommissionPercent, transaction.CommissionAmount,
                FinanceManager.CurrencyShort(transaction.Currency));
            return string.IsNullOrWhiteSpace(note) ? commission : note + " · " + commission;
        }

        private void GridHistory_ColumnHeaderMouseClick(object sender, DataGridViewCellMouseEventArgs e)
        {
            if (e.ColumnIndex < 0 || gridHistory.Columns[e.ColumnIndex].Name == "colDelete") return;
            string clicked = gridHistory.Columns[e.ColumnIndex].Name;
            _sortAscending = _sortColumnName == clicked ? !_sortAscending : true;
            _sortColumnName = clicked;
            BindHistoryGrid();
        }

        private static List<HistoryRow> SortHistoryRows(List<HistoryRow> rows, string column, bool ascending)
        {
            IOrderedEnumerable<HistoryRow> ordered;
            switch (column)
            {
                case "Дата":
                    ordered = ascending ? rows.OrderBy(r => DateTime.ParseExact(r.Дата, "dd.MM.yyyy HH:mm", System.Globalization.CultureInfo.InvariantCulture))
                        : rows.OrderByDescending(r => DateTime.ParseExact(r.Дата, "dd.MM.yyyy HH:mm", System.Globalization.CultureInfo.InvariantCulture));
                    break;
                case "Сумма":
                    ordered = ascending ? rows.OrderBy(r => r.Amount) : rows.OrderByDescending(r => r.Amount);
                    break;
                case "Тип":
                    ordered = ascending ? rows.OrderBy(r => r.Тип) : rows.OrderByDescending(r => r.Тип);
                    break;
                case "Валюта":
                    ordered = ascending ? rows.OrderBy(r => r.Валюта) : rows.OrderByDescending(r => r.Валюта);
                    break;
                case "Заметка":
                    ordered = ascending ? rows.OrderBy(r => r.Заметка) : rows.OrderByDescending(r => r.Заметка);
                    break;
                default:
                    ordered = ascending ? rows.OrderBy(r => r.Категория) : rows.OrderByDescending(r => r.Категория);
                    break;
            }
            return ordered.ToList();
        }

        private void AddDeleteColumn()
        {
            if (gridHistory.Columns["colDelete"] != null)
                return;

            var colDel = new DataGridViewButtonColumn
            {
                Name = "colDelete",
                HeaderText = "",
                Text = "🗑",
                UseColumnTextForButtonValue = true,
                Width = 48,
                AutoSizeMode = DataGridViewAutoSizeColumnMode.None,
                FlatStyle = FlatStyle.Flat
            };
            gridHistory.Columns.Add(colDel);
        }

        private void ApplyGridColumnLayout()
        {
            foreach (DataGridViewColumn col in gridHistory.Columns)
            {
                switch (col.Name)
                {
                    case "Заметка":
                        col.AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
                        col.MinimumWidth = 120;
                        col.DefaultCellStyle.WrapMode = DataGridViewTriState.True;
                        break;

                    case "Категория":
                        col.AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
                        col.MinimumWidth = 140;
                        break;

                    case "Сумма":
                        col.AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells;
                        col.DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;
                        break;

                    case "Дата":
                    case "Тип":
                    case "Валюта":
                        col.AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells;
                        break;

                    case "colDelete":
                        col.AutoSizeMode = DataGridViewAutoSizeColumnMode.None;
                        col.Width = 48;
                        break;
                }
            }
        }

        private static bool ContainsIgnoreCase(string haystack, string needle)
        {
            if (string.IsNullOrEmpty(haystack) || string.IsNullOrEmpty(needle))
                return false;
            return haystack.IndexOf(needle, StringComparison.OrdinalIgnoreCase) >= 0;
        }

        private Guid? GetSelectedTransactionId()
        {
            if (gridHistory.CurrentRow == null || gridHistory.CurrentRow.Index < 0)
                return null;
            return GetTransactionIdAt(gridHistory.CurrentRow.Index);
        }

        private Guid? GetTransactionIdAt(int rowIndex)
        {
            if (rowIndex < 0 || rowIndex >= gridHistory.Rows.Count)
                return null;
            if (gridHistory.Columns["Id"] == null)
                return null;

            var val = gridHistory.Rows[rowIndex].Cells["Id"].Value;
            if (val is Guid g)
                return g;
            return null;
        }

        private void gridHistory_CellFormatting(object sender, DataGridViewCellFormattingEventArgs e)
        {
            if (!_gridReady || e.RowIndex < 0 || e.ColumnIndex < 0)
                return;
            if (gridHistory.Columns[e.ColumnIndex].Name != "Сумма")
                return;

            var typeObj = gridHistory.Rows[e.RowIndex].Cells["Type"].Value;
            if (!(typeObj is TransactionType type))
                return;

            if (type == TransactionType.Income)
            {
                e.CellStyle.ForeColor = Theme.AccentGreen;
            }
            else if (type == TransactionType.Expense)
            {
                e.CellStyle.ForeColor = Theme.AccentRed;
            }
            else if (type == TransactionType.Exchange)
            {
                e.CellStyle.ForeColor = Theme.AccentGold;
            }
            else if (type == TransactionType.DebtPaid)
            {
                var note = Convert.ToString(gridHistory.Rows[e.RowIndex].Cells["Заметка"].Value) ?? "";
                e.CellStyle.ForeColor = note.IndexOf("Получено", StringComparison.OrdinalIgnoreCase) >= 0
                    ? Theme.AccentGreen
                    : Theme.AccentRed;
            }
            else
            {
                e.CellStyle.ForeColor = Theme.TextPrimary;
            }

            e.CellStyle.SelectionForeColor = e.CellStyle.ForeColor;
        }

        private void gridHistory_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex < 0 || e.ColumnIndex < 0)
                return;
            if (gridHistory.Columns[e.ColumnIndex].Name != "colDelete")
                return;

            Guid? id = GetTransactionIdAt(e.RowIndex);
            if (id.HasValue)
                DeleteTransactionById(id.Value);
        }

        private void gridHistory_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex < 0 || e.ColumnIndex < 0)
                return;
            if (gridHistory.Columns[e.ColumnIndex].Name == "colDelete")
                return;

            gridHistory.CurrentCell = gridHistory.Rows[e.RowIndex].Cells[e.ColumnIndex];
            EditSelected();
        }

        private void gridHistory_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Delete)
            {
                e.Handled = true;
                DeleteSelected();
            }
            else if (e.KeyCode == Keys.Enter)
            {
                e.Handled = true;
                EditSelected();
            }
        }

        private void ctxHistory_Opening(object sender, System.ComponentModel.CancelEventArgs e)
        {
            Guid? id = GetSelectedTransactionId();
            Transaction tx = id.HasValue && _manager != null ? _manager.GetTransaction(id.Value) : null;
            menuEdit.Enabled = tx != null &&
                               (tx.Type == TransactionType.Income || tx.Type == TransactionType.Expense);
        }

        private void menuDelete_Click(object sender, EventArgs e) => DeleteSelected();

        private void menuEdit_Click(object sender, EventArgs e) => EditSelected();

        private void DeleteSelected()
        {
            Guid? id = GetSelectedTransactionId();
            if (!id.HasValue)
            {
                UiHelper.ShowError(this, "Выберите запись в таблице.");
                return;
            }
            DeleteTransactionById(id.Value);
        }

        private void DeleteTransactionById(Guid id)
        {
            if (_manager == null)
                return;

            Transaction tx = _manager.GetTransaction(id);
            if (tx == null)
            {
                RefreshDashboard();
                return;
            }

            string details;
            switch (tx.Type)
            {
                case TransactionType.Expense:
                    details = "Сумма вернётся на баланс.";
                    break;
                case TransactionType.Income:
                    details = "Сумма будет списана с баланса.";
                    break;
                case TransactionType.Exchange:
                    details = "Обмен будет отменён, валюты вернутся на счета.";
                    break;
                case TransactionType.DebtCreated:
                    details = "Долг будет удалён из списка.";
                    break;
                case TransactionType.DebtPaid:
                    details = "Долг снова станет активным.";
                    break;
                default:
                    details = "Запись будет удалена из истории.";
                    break;
            }

            if (MessageBox.Show(this,
                    "Удалить эту запись?\r\n\r\n" + details,
                    "Удаление", MessageBoxButtons.YesNo, MessageBoxIcon.Question) != DialogResult.Yes)
                return;

            try
            {
                _manager.DeleteTransaction(id);
                RefreshDashboard();
            }
            catch (Exception ex)
            {
                UiHelper.ShowError(this, ex.Message);
            }
        }

        private void EditSelected()
        {
            Guid? id = GetSelectedTransactionId();
            if (!id.HasValue)
            {
                UiHelper.ShowError(this, "Выберите запись в таблице.");
                return;
            }

            var tx = _manager.GetTransaction(id.Value);
            if (tx == null)
                return;

            if (tx.Type != TransactionType.Income && tx.Type != TransactionType.Expense)
            {
                UiHelper.ShowError(this,
                    "Редактировать можно доход или расход.\r\n" +
                    "Для обмена и долгов используйте удаление и создание заново.");
                return;
            }

            using (var dlg = new EditTransactionForm(_manager, tx))
            {
                if (dlg.ShowDialog(this) == DialogResult.OK)
                    RefreshDashboard();
            }
        }

        private void cmbPeriod_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (_manager != null)
                RefreshDashboard();
        }

        private void cmbTypeFilter_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (_manager != null)
                BindHistoryGrid();
        }

        private void txtSearch_TextChanged(object sender, EventArgs e)
        {
            if (_manager != null)
                BindHistoryGrid();
        }

        private void btnDailyExpenses_Click(object sender, EventArgs e)
        {
            if (_manager == null)
                return;

            var days = _manager.GetExpenseTotalsByDay(EffectivePeriod, ExactDay);
            using (var dlg = new DailyExpensesForm(days, "Расходы по дням — " + PeriodTitle))
                dlg.ShowDialog(this);
        }

        private DateTime? ExactDay =>
            _dayFilter != null && _dayFilter.Checked ? _dayFilter.Value.Date : (DateTime?)null;

        private HistoryPeriod EffectivePeriod =>
            ExactDay.HasValue ? HistoryPeriod.AllTime : CurrentPeriod;

        private void ConfigureExactDayFilter()
        {
            _dayFilter = new DateTimePicker
            {
                Name = "dtpExactDay",
                Format = DateTimePickerFormat.Short,
                ShowCheckBox = true,
                Checked = false,
                Width = 150,
                Font = UiHelper.BodyFont
            };

            // ValueChanged ловит выбор даты, MouseUp/KeyUp — установку галочки.
            _dayFilter.ValueChanged += (s, e) => ScheduleFilterRefresh();
            _dayFilter.MouseUp += (s, e) => ScheduleFilterRefresh();
            _dayFilter.KeyUp += (s, e) => ScheduleFilterRefresh();
        }

        private void BuildFilterBar()
        {
            if (_filterBuilt || filterFlow == null || _dayFilter == null)
                return;

            filterFlow.SuspendLayout();
            filterFlow.Controls.Clear();

            // Высоту считаем сами: FlowLayoutPanel с переносом внутри
            // AutoSize-таблицы иначе завышает её и «съедает» место у таблицы.
            filterFlow.AutoSize = false;
            filterFlow.Dock = DockStyle.Top;

            filterFlow.FlowDirection = FlowDirection.TopDown;
            filterFlow.WrapContents = false;
            filterFlow.Padding = Padding.Empty;

            var filtersRow = new FlowLayoutPanel
            {
                Name = "filtersRow",
                AutoSize = true,
                AutoSizeMode = AutoSizeMode.GrowAndShrink,
                FlowDirection = FlowDirection.LeftToRight,
                WrapContents = true,
                Margin = Padding.Empty,
                Padding = Padding.Empty
            };
            filtersRow.Controls.Add(UiHelper.CreateInlineField("Период:", cmbPeriod));
            filtersRow.Controls.Add(UiHelper.CreateInlineField("День:", _dayFilter));
            filtersRow.Controls.Add(UiHelper.CreateInlineField("Показать:", cmbTypeFilter));
            filtersRow.Controls.Add(UiHelper.CreateInlineField("Поиск:", txtSearch));

            var actionsRow = new FlowLayoutPanel
            {
                Name = "historyActionsRow",
                Height = 46,
                AutoSize = false,
                FlowDirection = FlowDirection.RightToLeft,
                WrapContents = false,
                Margin = new Padding(0, 8, 0, 0),
                Padding = Padding.Empty
            };
            btnDailyExpenses.Size = new Size(195, 40);
            btnPrintExpenses.Size = new Size(160, 40);
            btnDailyExpenses.Margin = new Padding(0, 0, 10, 0);
            btnPrintExpenses.Margin = new Padding(0, 0, 10, 0);
            actionsRow.Controls.Add(btnDailyExpenses);
            actionsRow.Controls.Add(btnPrintExpenses);
            _btnExportExcel = new Button { Name = "btnExportExcel", Text = "📊  Экспорт в Excel", Size = new Size(190, 40), Margin = new Padding(0, 0, 10, 0) };
            UiHelper.StyleFlatButton(_btnExportExcel, Theme.AccentGreen, Color.White);
            _btnExportExcel.TextAlign = ContentAlignment.MiddleCenter;
            _btnExportExcel.Padding = Padding.Empty;
            _btnExportExcel.Click += BtnExportExcel_Click;
            actionsRow.Controls.Add(_btnExportExcel);

            filterFlow.Controls.Add(filtersRow);
            filterFlow.Controls.Add(actionsRow);
            filtersRow.Width = filterFlow.ClientSize.Width;
            actionsRow.Width = filterFlow.ClientSize.Width;
            filterFlow.Resize += (s, e) =>
            {
                filtersRow.Width = filterFlow.ClientSize.Width;
                actionsRow.Width = filterFlow.ClientSize.Width;
                AdjustFilterBarHeight();
            };

            filterFlow.ResumeLayout();
            _filterBuilt = true;

            AdjustFilterBarHeight();
        }

        private void AdjustFilterBarHeight()
        {
            if (_adjustingFilterHeight || filterFlow == null || filterFlow.IsDisposed)
                return;

            int width = filterFlow.ClientSize.Width;
            if (width <= 0)
                return;

            Size preferred = filterFlow.GetPreferredSize(new Size(width, 0));
            int target = Math.Max(40, preferred.Height);
            if (filterFlow.Height == target)
                return;

            _adjustingFilterHeight = true;
            try
            {
                filterFlow.Height = target;
                if (panelFilter != null)
                    panelFilter.PerformLayout();
                if (layoutRoot != null)
                    layoutRoot.PerformLayout();
            }
            finally
            {
                _adjustingFilterHeight = false;
            }
        }

        private void ScheduleFilterRefresh()
        {
            if (!IsHandleCreated || IsDisposed)
            {
                if (_manager != null)
                    RefreshDashboard();
                return;
            }

            if (_refreshPending)
                return;

            _refreshPending = true;
            BeginInvoke(new Action(() =>
            {
                _refreshPending = false;
                if (!IsDisposed && _manager != null)
                    RefreshDashboard();
            }));
        }

        private void ApplySearchCue()
        {
            try
            {
                SendMessage(txtSearch.Handle, EM_SETCUEBANNER, (IntPtr)1,
                    "Поиск по категории...");
            }
            catch
            {
                // Подсказка в поле — необязательное украшение.
            }
        }

        private void btnPrintExpenses_Click(object sender, EventArgs e)
        {
            var expenses = _filteredRows
                .Where(r => r.Type == TransactionType.Expense)
                .ToList();
            ExpensePrintHelper.ShowPreview(this, expenses, PeriodTitle);
        }

        private void BtnExportExcel_Click(object sender, EventArgs e)
        {
            if (_manager == null) return;
            using (var dialog = new SaveFileDialog
            {
                Title = "Экспорт доходов и расходов",
                Filter = "Книга Excel (*.xlsx)|*.xlsx",
                DefaultExt = "xlsx",
                AddExtension = true,
                FileName = "Финансы_" + DateTime.Now.ToString("yyyy-MM-dd") + ".xlsx",
                OverwritePrompt = true
            })
            {
                if (dialog.ShowDialog(this) != DialogResult.OK) return;
                try
                {
                    var transactions = _manager.GetTransactions(EffectivePeriod, ExactDay);
                    ExcelExportService.Export(dialog.FileName, transactions, PeriodTitle);
                    UiHelper.ShowInfo(this, "Экспорт завершён.\nФайл сохранён:\n" + dialog.FileName);
                }
                catch (Exception ex)
                {
                    UiHelper.ShowError(this, "Не удалось создать Excel-файл:\n" + ex.Message);
                }
            }
        }

        private void AddMdlBalanceCard()
        {
            cardsLayout.SuspendLayout();
            cardsLayout.ColumnCount = 5;
            cardsLayout.ColumnStyles.Clear();
            for (int i = 0; i < 5; i++) cardsLayout.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 20f));
            _cardMdl = new Panel { Name = "cardMdl", Dock = DockStyle.Fill, Margin = new Padding(0), Padding = new Padding(12, 12, 10, 12), BackColor = Theme.BgCardMdl };
            var title = new Label { Name = "lblMdlTitle", Text = "Молдавские леи (MDL)", Dock = DockStyle.Top, Height = 28, Font = new Font("Segoe UI Semibold", 10f, FontStyle.Bold), ForeColor = Theme.AccentBlue, TextAlign = ContentAlignment.MiddleLeft };
            _lblMdlAmount = new Label { Name = "lblMdlAmount", Text = "0.00", Dock = DockStyle.Fill, Font = UiHelper.CardAmountFont, ForeColor = Theme.TextPrimary, AutoEllipsis = true, TextAlign = ContentAlignment.MiddleLeft };
            _cardMdl.Controls.Add(_lblMdlAmount);
            _cardMdl.Controls.Add(title);
            cardsLayout.Controls.Add(_cardMdl, 4, 0);
            UiHelper.ApplyRoundedCorners(_cardMdl, 14);
            // Use compact gutters so five cards fit cleanly in the dashboard row.
            foreach (Control card in cardsLayout.Controls) card.Margin = new Padding(0, 0, 8, 0);
            cardsLayout.Controls[4].Margin = new Padding(0);
            cardsLayout.ResumeLayout(true);
        }

        private void btnTheme_Click(object sender, EventArgs e)
        {
            if (_manager == null)
                return;

            try
            {
                _manager.IsDarkTheme = !_manager.IsDarkTheme;
                Theme.Apply(_manager.IsDarkTheme);
                Program.ApplyKryptonTheme(_manager.IsDarkTheme);
                ApplyThemeToForm();
                AdjustFilterBarHeight();
                RefreshDashboard();
            }
            catch (Exception ex)
            {
                UiHelper.ShowError(this, ex.Message);
            }
        }

        private void btnEditBalance_Click(object sender, EventArgs e)
        {
            if (_manager == null)
                return;

            using (var dlg = new EditBalanceForm(_manager))
            {
                if (dlg.ShowDialog(this) == DialogResult.OK)
                    RefreshDashboard();
            }
        }

        private void btnIncome_Click(object sender, EventArgs e)
        {
            if (_manager == null)
                return;

            using (var dlg = new TransactionDialogForm(_manager, true))
            {
                if (dlg.ShowDialog(this) == DialogResult.OK)
                    RefreshDashboard();
            }
        }

        private void btnExpense_Click(object sender, EventArgs e)
        {
            if (_manager == null)
                return;

            using (var dlg = new TransactionDialogForm(_manager, false))
            {
                if (dlg.ShowDialog(this) == DialogResult.OK)
                    RefreshDashboard();
            }
        }

        private void btnExchange_Click(object sender, EventArgs e)
        {
            if (_manager == null)
                return;

            using (var dlg = new ExchangeDialogForm(_manager))
            {
                if (dlg.ShowDialog(this) == DialogResult.OK)
                {
                    RefreshDashboard();
                    UiHelper.ShowInfo(this, "Обмен выполнен. Балансы обновлены.");
                }
            }
        }

        private void btnDebts_Click(object sender, EventArgs e)
        {
            if (_manager == null)
                return;

            using (var dlg = new DebtsForm(_manager))
            {
                dlg.ShowDialog(this);
                RefreshDashboard();
            }
        }
    }
}
