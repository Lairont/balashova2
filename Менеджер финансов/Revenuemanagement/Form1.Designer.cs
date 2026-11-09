namespace Revenuemanagement
{
    partial class Form1
    {
        private System.ComponentModel.IContainer components = null;

        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
                components.Dispose();
            base.Dispose(disposing);
        }

        #region Код, автоматически созданный конструктором форм Windows

        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            this.panelSidebar = new System.Windows.Forms.Panel();
            this.sidebarLayout = new System.Windows.Forms.TableLayoutPanel();
            this.lblAppTitle = new System.Windows.Forms.Label();
            this.lblAppSubtitle = new System.Windows.Forms.Label();
            this.sidebarMenu = new System.Windows.Forms.TableLayoutPanel();
            this.btnIncome = new System.Windows.Forms.Button();
            this.btnExpense = new System.Windows.Forms.Button();
            this.btnExchange = new System.Windows.Forms.Button();
            this.btnDebts = new System.Windows.Forms.Button();
            this.btnEditBalance = new System.Windows.Forms.Button();
            this.btnTheme = new System.Windows.Forms.Button();
            this.panelMain = new System.Windows.Forms.Panel();
            this.layoutRoot = new System.Windows.Forms.TableLayoutPanel();
            this.lblWelcome = new System.Windows.Forms.Label();
            this.cardsLayout = new System.Windows.Forms.TableLayoutPanel();
            this.cardPrb = new System.Windows.Forms.Panel();
            this.lblPrbTitle = new System.Windows.Forms.Label();
            this.lblPrbAmount = new System.Windows.Forms.Label();
            this.cardRub = new System.Windows.Forms.Panel();
            this.lblRubTitle = new System.Windows.Forms.Label();
            this.lblRubAmount = new System.Windows.Forms.Label();
            this.cardUsd = new System.Windows.Forms.Panel();
            this.lblUsdTitle = new System.Windows.Forms.Label();
            this.lblUsdAmount = new System.Windows.Forms.Label();
            this.cardEur = new System.Windows.Forms.Panel();
            this.lblEurTitle = new System.Windows.Forms.Label();
            this.lblEurAmount = new System.Windows.Forms.Label();
            this.panelFilter = new System.Windows.Forms.TableLayoutPanel();
            this.lblHistoryTitle = new System.Windows.Forms.Label();
            this.filterFlow = new System.Windows.Forms.FlowLayoutPanel();
            this.cmbPeriod = new Krypton.Toolkit.KryptonComboBox();
            this.cmbTypeFilter = new Krypton.Toolkit.KryptonComboBox();
            this.txtSearch = new System.Windows.Forms.TextBox();
            this.btnDailyExpenses = new System.Windows.Forms.Button();
            this.btnPrintExpenses = new System.Windows.Forms.Button();
            this.panelContent = new System.Windows.Forms.TableLayoutPanel();
            this.panelGridHost = new System.Windows.Forms.Panel();
            this.gridHistory = new System.Windows.Forms.DataGridView();
            this.lblEmptyState = new System.Windows.Forms.Label();
            this.panelPieHost = new System.Windows.Forms.Panel();
            this.pieExpenses = new Revenuemanagement.ExpensePiePanel();
            this.panelFooter = new System.Windows.Forms.TableLayoutPanel();
            this.lblCopyright = new System.Windows.Forms.Label();
            this.lblDataPath = new System.Windows.Forms.Label();
            this.ctxHistory = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.menuEdit = new System.Windows.Forms.ToolStripMenuItem();
            this.menuDelete = new System.Windows.Forms.ToolStripMenuItem();
            this.panelSidebar.SuspendLayout();
            this.sidebarLayout.SuspendLayout();
            this.sidebarMenu.SuspendLayout();
            this.panelMain.SuspendLayout();
            this.layoutRoot.SuspendLayout();
            this.cardsLayout.SuspendLayout();
            this.cardPrb.SuspendLayout();
            this.cardRub.SuspendLayout();
            this.cardUsd.SuspendLayout();
            this.cardEur.SuspendLayout();
            this.panelFilter.SuspendLayout();
            this.filterFlow.SuspendLayout();
            this.panelContent.SuspendLayout();
            this.panelGridHost.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.gridHistory)).BeginInit();
            this.panelPieHost.SuspendLayout();
            this.panelFooter.SuspendLayout();
            this.ctxHistory.SuspendLayout();
            this.SuspendLayout();
            // 
            // panelSidebar
            // 
            this.panelSidebar.BackColor = System.Drawing.Color.FromArgb(36, 59, 85);
            this.panelSidebar.Controls.Add(this.sidebarLayout);
            this.panelSidebar.Dock = System.Windows.Forms.DockStyle.Left;
            this.panelSidebar.Location = new System.Drawing.Point(0, 0);
            this.panelSidebar.Name = "panelSidebar";
            this.panelSidebar.Size = new System.Drawing.Size(250, 720);
            this.panelSidebar.TabIndex = 0;
            // 
            // sidebarLayout
            // 
            this.sidebarLayout.BackColor = System.Drawing.Color.FromArgb(36, 59, 85);
            this.sidebarLayout.ColumnCount = 1;
            this.sidebarLayout.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.sidebarLayout.Controls.Add(this.lblAppTitle, 0, 0);
            this.sidebarLayout.Controls.Add(this.lblAppSubtitle, 0, 1);
            this.sidebarLayout.Controls.Add(this.sidebarMenu, 0, 2);
            this.sidebarLayout.Controls.Add(this.btnTheme, 0, 3);
            this.sidebarLayout.Dock = System.Windows.Forms.DockStyle.Fill;
            this.sidebarLayout.Location = new System.Drawing.Point(0, 0);
            this.sidebarLayout.Name = "sidebarLayout";
            this.sidebarLayout.Padding = new System.Windows.Forms.Padding(16, 20, 16, 16);
            this.sidebarLayout.RowCount = 4;
            this.sidebarLayout.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.sidebarLayout.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.sidebarLayout.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.sidebarLayout.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.sidebarLayout.Size = new System.Drawing.Size(250, 720);
            this.sidebarLayout.TabIndex = 0;
            // 
            // lblAppTitle
            // 
            this.lblAppTitle.AutoSize = false;
            this.lblAppTitle.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lblAppTitle.Font = new System.Drawing.Font("Segoe UI Semibold", 17F, System.Drawing.FontStyle.Bold);
            this.lblAppTitle.ForeColor = System.Drawing.Color.White;
            this.lblAppTitle.Location = new System.Drawing.Point(16, 20);
            this.lblAppTitle.Margin = new System.Windows.Forms.Padding(0);
            this.lblAppTitle.Name = "lblAppTitle";
            this.lblAppTitle.Size = new System.Drawing.Size(218, 40);
            this.lblAppTitle.TabIndex = 0;
            this.lblAppTitle.Text = "💰  Кошелёк";
            this.lblAppTitle.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // lblAppSubtitle
            // 
            this.lblAppSubtitle.AutoSize = false;
            this.lblAppSubtitle.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lblAppSubtitle.Font = new System.Drawing.Font("Segoe UI", 9.5F);
            this.lblAppSubtitle.ForeColor = System.Drawing.Color.FromArgb(180, 200, 220);
            this.lblAppSubtitle.Location = new System.Drawing.Point(16, 60);
            this.lblAppSubtitle.Margin = new System.Windows.Forms.Padding(0, 0, 0, 16);
            this.lblAppSubtitle.Name = "lblAppSubtitle";
            this.lblAppSubtitle.Size = new System.Drawing.Size(218, 24);
            this.lblAppSubtitle.TabIndex = 1;
            this.lblAppSubtitle.Text = "Личные финансы";
            this.lblAppSubtitle.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // sidebarMenu
            // 
            this.sidebarMenu.BackColor = System.Drawing.Color.FromArgb(36, 59, 85);
            this.sidebarMenu.ColumnCount = 1;
            this.sidebarMenu.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.sidebarMenu.Controls.Add(this.btnIncome, 0, 0);
            this.sidebarMenu.Controls.Add(this.btnExpense, 0, 1);
            this.sidebarMenu.Controls.Add(this.btnExchange, 0, 2);
            this.sidebarMenu.Controls.Add(this.btnDebts, 0, 3);
            this.sidebarMenu.Controls.Add(this.btnEditBalance, 0, 4);
            this.sidebarMenu.Dock = System.Windows.Forms.DockStyle.Fill;
            this.sidebarMenu.Location = new System.Drawing.Point(16, 100);
            this.sidebarMenu.Margin = new System.Windows.Forms.Padding(0);
            this.sidebarMenu.Name = "sidebarMenu";
            this.sidebarMenu.RowCount = 6;
            this.sidebarMenu.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 52F));
            this.sidebarMenu.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 52F));
            this.sidebarMenu.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 52F));
            this.sidebarMenu.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 52F));
            this.sidebarMenu.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 52F));
            this.sidebarMenu.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.sidebarMenu.Size = new System.Drawing.Size(218, 548);
            this.sidebarMenu.TabIndex = 2;
            // 
            // btnIncome
            // 
            this.btnIncome.Dock = System.Windows.Forms.DockStyle.Fill;
            this.btnIncome.Location = new System.Drawing.Point(0, 0);
            this.btnIncome.Margin = new System.Windows.Forms.Padding(0, 0, 0, 8);
            this.btnIncome.Name = "btnIncome";
            this.btnIncome.Size = new System.Drawing.Size(218, 44);
            this.btnIncome.TabIndex = 0;
            this.btnIncome.Text = "➕  Добавить доход";
            this.btnIncome.Click += new System.EventHandler(this.btnIncome_Click);
            // 
            // btnExpense
            // 
            this.btnExpense.Dock = System.Windows.Forms.DockStyle.Fill;
            this.btnExpense.Location = new System.Drawing.Point(0, 52);
            this.btnExpense.Margin = new System.Windows.Forms.Padding(0, 0, 0, 8);
            this.btnExpense.Name = "btnExpense";
            this.btnExpense.Size = new System.Drawing.Size(218, 44);
            this.btnExpense.TabIndex = 1;
            this.btnExpense.Text = "➖  Добавить расход";
            this.btnExpense.Click += new System.EventHandler(this.btnExpense_Click);
            // 
            // btnExchange
            // 
            this.btnExchange.Dock = System.Windows.Forms.DockStyle.Fill;
            this.btnExchange.Location = new System.Drawing.Point(0, 104);
            this.btnExchange.Margin = new System.Windows.Forms.Padding(0, 0, 0, 8);
            this.btnExchange.Name = "btnExchange";
            this.btnExchange.Size = new System.Drawing.Size(218, 44);
            this.btnExchange.TabIndex = 2;
            this.btnExchange.Text = "💱  Обмен валют";
            this.btnExchange.Click += new System.EventHandler(this.btnExchange_Click);
            // 
            // btnDebts
            // 
            this.btnDebts.Dock = System.Windows.Forms.DockStyle.Fill;
            this.btnDebts.Location = new System.Drawing.Point(0, 156);
            this.btnDebts.Margin = new System.Windows.Forms.Padding(0, 0, 0, 8);
            this.btnDebts.Name = "btnDebts";
            this.btnDebts.Size = new System.Drawing.Size(218, 44);
            this.btnDebts.TabIndex = 3;
            this.btnDebts.Text = "📒  Мои долги";
            this.btnDebts.Click += new System.EventHandler(this.btnDebts_Click);
            // 
            // btnEditBalance
            // 
            this.btnEditBalance.Dock = System.Windows.Forms.DockStyle.Fill;
            this.btnEditBalance.Location = new System.Drawing.Point(0, 208);
            this.btnEditBalance.Margin = new System.Windows.Forms.Padding(0, 0, 0, 8);
            this.btnEditBalance.Name = "btnEditBalance";
            this.btnEditBalance.Size = new System.Drawing.Size(218, 44);
            this.btnEditBalance.TabIndex = 4;
            this.btnEditBalance.Text = "✏  Изменить баланс";
            this.btnEditBalance.Click += new System.EventHandler(this.btnEditBalance_Click);
            // 
            // btnTheme
            // 
            this.btnTheme.Dock = System.Windows.Forms.DockStyle.Fill;
            this.btnTheme.Location = new System.Drawing.Point(16, 652);
            this.btnTheme.Margin = new System.Windows.Forms.Padding(0, 12, 0, 0);
            this.btnTheme.Name = "btnTheme";
            this.btnTheme.Size = new System.Drawing.Size(218, 52);
            this.btnTheme.TabIndex = 3;
            this.btnTheme.Text = "🌙  Тёмная тема";
            this.btnTheme.Click += new System.EventHandler(this.btnTheme_Click);
            // 
            // panelMain
            // 
            this.panelMain.BackColor = System.Drawing.Color.FromArgb(245, 248, 252);
            this.panelMain.Controls.Add(this.layoutRoot);
            this.panelMain.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panelMain.Location = new System.Drawing.Point(250, 0);
            this.panelMain.Name = "panelMain";
            this.panelMain.Size = new System.Drawing.Size(1030, 720);
            this.panelMain.TabIndex = 1;
            // 
            // layoutRoot
            // 
            this.layoutRoot.BackColor = System.Drawing.Color.FromArgb(245, 248, 252);
            this.layoutRoot.ColumnCount = 1;
            this.layoutRoot.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.layoutRoot.Controls.Add(this.lblWelcome, 0, 0);
            this.layoutRoot.Controls.Add(this.cardsLayout, 0, 1);
            this.layoutRoot.Controls.Add(this.panelFilter, 0, 2);
            this.layoutRoot.Controls.Add(this.panelContent, 0, 3);
            this.layoutRoot.Controls.Add(this.panelFooter, 0, 4);
            this.layoutRoot.Dock = System.Windows.Forms.DockStyle.Fill;
            this.layoutRoot.Location = new System.Drawing.Point(0, 0);
            this.layoutRoot.Name = "layoutRoot";
            this.layoutRoot.Padding = new System.Windows.Forms.Padding(24, 18, 24, 10);
            this.layoutRoot.RowCount = 5;
            this.layoutRoot.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.layoutRoot.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 140F));
            this.layoutRoot.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.layoutRoot.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.layoutRoot.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 52F));
            this.layoutRoot.Size = new System.Drawing.Size(1030, 720);
            this.layoutRoot.TabIndex = 0;
            // 
            // lblWelcome
            // 
            this.lblWelcome.AutoSize = false;
            this.lblWelcome.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lblWelcome.Font = new System.Drawing.Font("Segoe UI Semibold", 16F, System.Drawing.FontStyle.Bold);
            this.lblWelcome.ForeColor = System.Drawing.Color.FromArgb(40, 50, 60);
            this.lblWelcome.Location = new System.Drawing.Point(24, 18);
            this.lblWelcome.Margin = new System.Windows.Forms.Padding(0, 0, 0, 12);
            this.lblWelcome.Name = "lblWelcome";
            this.lblWelcome.Size = new System.Drawing.Size(982, 44);
            this.lblWelcome.TabIndex = 0;
            this.lblWelcome.Text = "Привет! Вот ваши финансы";
            this.lblWelcome.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // cardsLayout
            // 
            this.cardsLayout.BackColor = System.Drawing.Color.FromArgb(245, 248, 252);
            this.cardsLayout.ColumnCount = 4;
            this.cardsLayout.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 25F));
            this.cardsLayout.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 25F));
            this.cardsLayout.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 25F));
            this.cardsLayout.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 25F));
            this.cardsLayout.Controls.Add(this.cardPrb, 0, 0);
            this.cardsLayout.Controls.Add(this.cardRub, 1, 0);
            this.cardsLayout.Controls.Add(this.cardUsd, 2, 0);
            this.cardsLayout.Controls.Add(this.cardEur, 3, 0);
            this.cardsLayout.Dock = System.Windows.Forms.DockStyle.Fill;
            this.cardsLayout.Location = new System.Drawing.Point(24, 74);
            this.cardsLayout.Margin = new System.Windows.Forms.Padding(0, 0, 0, 12);
            this.cardsLayout.Name = "cardsLayout";
            this.cardsLayout.RowCount = 1;
            this.cardsLayout.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.cardsLayout.Size = new System.Drawing.Size(982, 128);
            this.cardsLayout.TabIndex = 1;
            // 
            // cardPrb
            // 
            this.cardPrb.BackColor = System.Drawing.Color.FromArgb(230, 245, 238);
            this.cardPrb.Controls.Add(this.lblPrbAmount);
            this.cardPrb.Controls.Add(this.lblPrbTitle);
            this.cardPrb.Dock = System.Windows.Forms.DockStyle.Fill;
            this.cardPrb.Location = new System.Drawing.Point(0, 0);
            this.cardPrb.Margin = new System.Windows.Forms.Padding(0, 0, 12, 0);
            this.cardPrb.Name = "cardPrb";
            this.cardPrb.Padding = new System.Windows.Forms.Padding(16, 12, 16, 12);
            this.cardPrb.Size = new System.Drawing.Size(233, 128);
            this.cardPrb.TabIndex = 0;
            // 
            // lblPrbTitle
            // 
            this.lblPrbTitle.AutoSize = false;
            this.lblPrbTitle.Dock = System.Windows.Forms.DockStyle.Top;
            this.lblPrbTitle.Font = new System.Drawing.Font("Segoe UI Semibold", 11.5F, System.Drawing.FontStyle.Bold);
            this.lblPrbTitle.ForeColor = System.Drawing.Color.FromArgb(46, 160, 120);
            this.lblPrbTitle.Location = new System.Drawing.Point(16, 12);
            this.lblPrbTitle.Margin = new System.Windows.Forms.Padding(0);
            this.lblPrbTitle.Name = "lblPrbTitle";
            this.lblPrbTitle.Size = new System.Drawing.Size(201, 28);
            this.lblPrbTitle.TabIndex = 0;
            this.lblPrbTitle.Text = "Рубли ПМР (PRB)";
            this.lblPrbTitle.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // lblPrbAmount
            // 
            this.lblPrbAmount.AutoEllipsis = true;
            this.lblPrbAmount.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lblPrbAmount.Font = new System.Drawing.Font("Segoe UI Semibold", 18F, System.Drawing.FontStyle.Bold);
            this.lblPrbAmount.ForeColor = System.Drawing.Color.FromArgb(40, 50, 60);
            this.lblPrbAmount.Location = new System.Drawing.Point(16, 40);
            this.lblPrbAmount.Margin = new System.Windows.Forms.Padding(0);
            this.lblPrbAmount.Name = "lblPrbAmount";
            this.lblPrbAmount.Size = new System.Drawing.Size(201, 76);
            this.lblPrbAmount.TabIndex = 1;
            this.lblPrbAmount.Text = "0.00";
            this.lblPrbAmount.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // cardRub
            // 
            this.cardRub.BackColor = System.Drawing.Color.FromArgb(232, 240, 250);
            this.cardRub.Controls.Add(this.lblRubAmount);
            this.cardRub.Controls.Add(this.lblRubTitle);
            this.cardRub.Dock = System.Windows.Forms.DockStyle.Fill;
            this.cardRub.Location = new System.Drawing.Point(245, 0);
            this.cardRub.Margin = new System.Windows.Forms.Padding(0, 0, 12, 0);
            this.cardRub.Name = "cardRub";
            this.cardRub.Padding = new System.Windows.Forms.Padding(16, 12, 16, 12);
            this.cardRub.Size = new System.Drawing.Size(233, 128);
            this.cardRub.TabIndex = 1;
            // 
            // lblRubTitle
            // 
            this.lblRubTitle.AutoSize = false;
            this.lblRubTitle.Dock = System.Windows.Forms.DockStyle.Top;
            this.lblRubTitle.Font = new System.Drawing.Font("Segoe UI Semibold", 11.5F, System.Drawing.FontStyle.Bold);
            this.lblRubTitle.ForeColor = System.Drawing.Color.FromArgb(70, 130, 180);
            this.lblRubTitle.Location = new System.Drawing.Point(16, 12);
            this.lblRubTitle.Margin = new System.Windows.Forms.Padding(0);
            this.lblRubTitle.Name = "lblRubTitle";
            this.lblRubTitle.Size = new System.Drawing.Size(201, 28);
            this.lblRubTitle.TabIndex = 0;
            this.lblRubTitle.Text = "Рубли РФ (RUB)";
            this.lblRubTitle.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // lblRubAmount
            // 
            this.lblRubAmount.AutoEllipsis = true;
            this.lblRubAmount.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lblRubAmount.Font = new System.Drawing.Font("Segoe UI Semibold", 18F, System.Drawing.FontStyle.Bold);
            this.lblRubAmount.ForeColor = System.Drawing.Color.FromArgb(40, 50, 60);
            this.lblRubAmount.Location = new System.Drawing.Point(16, 40);
            this.lblRubAmount.Margin = new System.Windows.Forms.Padding(0);
            this.lblRubAmount.Name = "lblRubAmount";
            this.lblRubAmount.Size = new System.Drawing.Size(201, 76);
            this.lblRubAmount.TabIndex = 1;
            this.lblRubAmount.Text = "0.00";
            this.lblRubAmount.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // cardUsd
            // 
            this.cardUsd.BackColor = System.Drawing.Color.FromArgb(255, 246, 230);
            this.cardUsd.Controls.Add(this.lblUsdAmount);
            this.cardUsd.Controls.Add(this.lblUsdTitle);
            this.cardUsd.Dock = System.Windows.Forms.DockStyle.Fill;
            this.cardUsd.Location = new System.Drawing.Point(490, 0);
            this.cardUsd.Margin = new System.Windows.Forms.Padding(0, 0, 12, 0);
            this.cardUsd.Name = "cardUsd";
            this.cardUsd.Padding = new System.Windows.Forms.Padding(16, 12, 16, 12);
            this.cardUsd.Size = new System.Drawing.Size(233, 128);
            this.cardUsd.TabIndex = 2;
            // 
            // lblUsdTitle
            // 
            this.lblUsdTitle.AutoSize = false;
            this.lblUsdTitle.Dock = System.Windows.Forms.DockStyle.Top;
            this.lblUsdTitle.Font = new System.Drawing.Font("Segoe UI Semibold", 11.5F, System.Drawing.FontStyle.Bold);
            this.lblUsdTitle.ForeColor = System.Drawing.Color.FromArgb(212, 160, 50);
            this.lblUsdTitle.Location = new System.Drawing.Point(16, 12);
            this.lblUsdTitle.Margin = new System.Windows.Forms.Padding(0);
            this.lblUsdTitle.Name = "lblUsdTitle";
            this.lblUsdTitle.Size = new System.Drawing.Size(201, 28);
            this.lblUsdTitle.TabIndex = 0;
            this.lblUsdTitle.Text = "Доллары США (USD)";
            this.lblUsdTitle.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // lblUsdAmount
            // 
            this.lblUsdAmount.AutoEllipsis = true;
            this.lblUsdAmount.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lblUsdAmount.Font = new System.Drawing.Font("Segoe UI Semibold", 18F, System.Drawing.FontStyle.Bold);
            this.lblUsdAmount.ForeColor = System.Drawing.Color.FromArgb(40, 50, 60);
            this.lblUsdAmount.Location = new System.Drawing.Point(16, 40);
            this.lblUsdAmount.Margin = new System.Windows.Forms.Padding(0);
            this.lblUsdAmount.Name = "lblUsdAmount";
            this.lblUsdAmount.Size = new System.Drawing.Size(201, 76);
            this.lblUsdAmount.TabIndex = 1;
            this.lblUsdAmount.Text = "0.00";
            this.lblUsdAmount.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // cardEur
            // 
            this.cardEur.BackColor = System.Drawing.Color.FromArgb(238, 232, 250);
            this.cardEur.Controls.Add(this.lblEurAmount);
            this.cardEur.Controls.Add(this.lblEurTitle);
            this.cardEur.Dock = System.Windows.Forms.DockStyle.Fill;
            this.cardEur.Location = new System.Drawing.Point(735, 0);
            this.cardEur.Margin = new System.Windows.Forms.Padding(0);
            this.cardEur.Name = "cardEur";
            this.cardEur.Padding = new System.Windows.Forms.Padding(16, 12, 16, 12);
            this.cardEur.Size = new System.Drawing.Size(247, 128);
            this.cardEur.TabIndex = 3;
            // 
            // lblEurTitle
            // 
            this.lblEurTitle.AutoSize = false;
            this.lblEurTitle.Dock = System.Windows.Forms.DockStyle.Top;
            this.lblEurTitle.Font = new System.Drawing.Font("Segoe UI Semibold", 11.5F, System.Drawing.FontStyle.Bold);
            this.lblEurTitle.ForeColor = System.Drawing.Color.FromArgb(140, 100, 190);
            this.lblEurTitle.Location = new System.Drawing.Point(16, 12);
            this.lblEurTitle.Margin = new System.Windows.Forms.Padding(0);
            this.lblEurTitle.Name = "lblEurTitle";
            this.lblEurTitle.Size = new System.Drawing.Size(215, 28);
            this.lblEurTitle.TabIndex = 0;
            this.lblEurTitle.Text = "Евро (EUR)";
            this.lblEurTitle.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // lblEurAmount
            // 
            this.lblEurAmount.AutoEllipsis = true;
            this.lblEurAmount.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lblEurAmount.Font = new System.Drawing.Font("Segoe UI Semibold", 18F, System.Drawing.FontStyle.Bold);
            this.lblEurAmount.ForeColor = System.Drawing.Color.FromArgb(40, 50, 60);
            this.lblEurAmount.Location = new System.Drawing.Point(16, 40);
            this.lblEurAmount.Margin = new System.Windows.Forms.Padding(0);
            this.lblEurAmount.Name = "lblEurAmount";
            this.lblEurAmount.Size = new System.Drawing.Size(215, 76);
            this.lblEurAmount.TabIndex = 1;
            this.lblEurAmount.Text = "0.00";
            this.lblEurAmount.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // panelFilter
            // 
            this.panelFilter.AutoSize = true;
            this.panelFilter.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.panelFilter.BackColor = System.Drawing.Color.FromArgb(245, 248, 252);
            this.panelFilter.ColumnCount = 1;
            this.panelFilter.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.panelFilter.Controls.Add(this.lblHistoryTitle, 0, 0);
            this.panelFilter.Controls.Add(this.filterFlow, 0, 1);
            this.panelFilter.Dock = System.Windows.Forms.DockStyle.Top;
            this.panelFilter.Location = new System.Drawing.Point(24, 214);
            this.panelFilter.Margin = new System.Windows.Forms.Padding(0, 0, 0, 8);
            this.panelFilter.Name = "panelFilter";
            this.panelFilter.RowCount = 2;
            this.panelFilter.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.panelFilter.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.panelFilter.Size = new System.Drawing.Size(982, 84);
            this.panelFilter.TabIndex = 2;
            // 
            // lblHistoryTitle
            // 
            this.lblHistoryTitle.AutoSize = true;
            this.lblHistoryTitle.Font = new System.Drawing.Font("Segoe UI Semibold", 13F, System.Drawing.FontStyle.Bold);
            this.lblHistoryTitle.ForeColor = System.Drawing.Color.FromArgb(40, 50, 60);
            this.lblHistoryTitle.Location = new System.Drawing.Point(0, 0);
            this.lblHistoryTitle.Margin = new System.Windows.Forms.Padding(0, 0, 0, 8);
            this.lblHistoryTitle.Name = "lblHistoryTitle";
            this.lblHistoryTitle.Size = new System.Drawing.Size(185, 30);
            this.lblHistoryTitle.TabIndex = 0;
            this.lblHistoryTitle.Text = "История операций";
            // 
            // filterFlow
            // 
            this.filterFlow.AutoSize = true;
            this.filterFlow.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.filterFlow.BackColor = System.Drawing.Color.FromArgb(245, 248, 252);
            this.filterFlow.Controls.Add(this.cmbPeriod);
            this.filterFlow.Controls.Add(this.cmbTypeFilter);
            this.filterFlow.Controls.Add(this.txtSearch);
            this.filterFlow.Controls.Add(this.btnDailyExpenses);
            this.filterFlow.Controls.Add(this.btnPrintExpenses);
            this.filterFlow.Dock = System.Windows.Forms.DockStyle.Fill;
            this.filterFlow.FlowDirection = System.Windows.Forms.FlowDirection.LeftToRight;
            this.filterFlow.Location = new System.Drawing.Point(0, 38);
            this.filterFlow.Margin = new System.Windows.Forms.Padding(0);
            this.filterFlow.Name = "filterFlow";
            this.filterFlow.Size = new System.Drawing.Size(982, 46);
            this.filterFlow.TabIndex = 1;
            this.filterFlow.WrapContents = true;
            // 
            // cmbPeriod
            // 
            this.cmbPeriod.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbPeriod.Font = new System.Drawing.Font("Segoe UI", 10.5F);
            this.cmbPeriod.FormattingEnabled = true;
            this.cmbPeriod.Items.AddRange(new object[] {
            "За всё время",
            "Этот месяц",
            "Прошлый месяц"});
            this.cmbPeriod.Location = new System.Drawing.Point(0, 0);
            this.cmbPeriod.Margin = new System.Windows.Forms.Padding(0);
            this.cmbPeriod.Name = "cmbPeriod";
            this.cmbPeriod.Size = new System.Drawing.Size(170, 32);
            this.cmbPeriod.TabIndex = 0;
            this.cmbPeriod.SelectedIndexChanged += new System.EventHandler(this.cmbPeriod_SelectedIndexChanged);
            // 
            // cmbTypeFilter
            // 
            this.cmbTypeFilter.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbTypeFilter.Font = new System.Drawing.Font("Segoe UI", 10.5F);
            this.cmbTypeFilter.FormattingEnabled = true;
            this.cmbTypeFilter.Items.AddRange(new object[] {
            "Все операции",
            "Доходы",
            "Расходы",
            "Долги",
            "Обмен валют"});
            this.cmbTypeFilter.Location = new System.Drawing.Point(0, 0);
            this.cmbTypeFilter.Margin = new System.Windows.Forms.Padding(0);
            this.cmbTypeFilter.Name = "cmbTypeFilter";
            this.cmbTypeFilter.Size = new System.Drawing.Size(170, 32);
            this.cmbTypeFilter.TabIndex = 1;
            this.cmbTypeFilter.SelectedIndexChanged += new System.EventHandler(this.cmbTypeFilter_SelectedIndexChanged);
            // 
            // txtSearch
            // 
            this.txtSearch.Font = new System.Drawing.Font("Segoe UI", 10.5F);
            this.txtSearch.Location = new System.Drawing.Point(0, 0);
            this.txtSearch.Margin = new System.Windows.Forms.Padding(0);
            this.txtSearch.Name = "txtSearch";
            this.txtSearch.Size = new System.Drawing.Size(240, 31);
            this.txtSearch.TabIndex = 2;
            this.txtSearch.TextChanged += new System.EventHandler(this.txtSearch_TextChanged);
            // 
            // btnDailyExpenses
            // 
            this.btnDailyExpenses.Font = new System.Drawing.Font("Segoe UI Semibold", 10F, System.Drawing.FontStyle.Bold);
            this.btnDailyExpenses.Location = new System.Drawing.Point(0, 0);
            this.btnDailyExpenses.Margin = new System.Windows.Forms.Padding(0, 0, 12, 8);
            this.btnDailyExpenses.Name = "btnDailyExpenses";
            this.btnDailyExpenses.Size = new System.Drawing.Size(220, 38);
            this.btnDailyExpenses.TabIndex = 3;
            this.btnDailyExpenses.Text = "📅  Расходы по дням";
            this.btnDailyExpenses.UseVisualStyleBackColor = false;
            this.btnDailyExpenses.Click += new System.EventHandler(this.btnDailyExpenses_Click);
            // 
            // btnPrintExpenses
            // 
            this.btnPrintExpenses.Font = new System.Drawing.Font("Segoe UI Semibold", 10F, System.Drawing.FontStyle.Bold);
            this.btnPrintExpenses.Location = new System.Drawing.Point(0, 0);
            this.btnPrintExpenses.Margin = new System.Windows.Forms.Padding(0, 0, 0, 8);
            this.btnPrintExpenses.Name = "btnPrintExpenses";
            this.btnPrintExpenses.Size = new System.Drawing.Size(220, 38);
            this.btnPrintExpenses.TabIndex = 4;
            this.btnPrintExpenses.Text = "🖨  Печать расходов";
            this.btnPrintExpenses.UseVisualStyleBackColor = false;
            this.btnPrintExpenses.Click += new System.EventHandler(this.btnPrintExpenses_Click);
            // 
            // panelContent
            // 
            this.panelContent.BackColor = System.Drawing.Color.FromArgb(245, 248, 252);
            this.panelContent.ColumnCount = 2;
            this.panelContent.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 70F));
            this.panelContent.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 30F));
            this.panelContent.Controls.Add(this.panelGridHost, 0, 0);
            this.panelContent.Controls.Add(this.panelPieHost, 1, 0);
            this.panelContent.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panelContent.Location = new System.Drawing.Point(24, 306);
            this.panelContent.Margin = new System.Windows.Forms.Padding(0);
            this.panelContent.Name = "panelContent";
            this.panelContent.RowCount = 1;
            this.panelContent.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.panelContent.Size = new System.Drawing.Size(982, 352);
            this.panelContent.TabIndex = 3;
            // 
            // panelGridHost
            // 
            this.panelGridHost.BackColor = System.Drawing.Color.White;
            this.panelGridHost.Controls.Add(this.gridHistory);
            this.panelGridHost.Controls.Add(this.lblEmptyState);
            this.panelGridHost.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panelGridHost.Location = new System.Drawing.Point(0, 0);
            this.panelGridHost.Margin = new System.Windows.Forms.Padding(0);
            this.panelGridHost.Name = "panelGridHost";
            this.panelGridHost.Size = new System.Drawing.Size(682, 352);
            this.panelGridHost.TabIndex = 0;
            // 
            // gridHistory
            // 
            this.gridHistory.AllowUserToAddRows = false;
            this.gridHistory.AllowUserToDeleteRows = false;
            this.gridHistory.BackgroundColor = System.Drawing.Color.White;
            this.gridHistory.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.gridHistory.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.gridHistory.ContextMenuStrip = this.ctxHistory;
            this.gridHistory.Dock = System.Windows.Forms.DockStyle.Fill;
            this.gridHistory.Font = new System.Drawing.Font("Segoe UI", 10.5F);
            this.gridHistory.Location = new System.Drawing.Point(0, 0);
            this.gridHistory.Margin = new System.Windows.Forms.Padding(0);
            this.gridHistory.MultiSelect = false;
            this.gridHistory.Name = "gridHistory";
            this.gridHistory.ReadOnly = true;
            this.gridHistory.RowHeadersVisible = false;
            this.gridHistory.RowHeadersWidth = 51;
            this.gridHistory.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.gridHistory.Size = new System.Drawing.Size(682, 352);
            this.gridHistory.TabIndex = 0;
            this.gridHistory.CellContentClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.gridHistory_CellContentClick);
            this.gridHistory.CellDoubleClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.gridHistory_CellDoubleClick);
            this.gridHistory.CellFormatting += new System.Windows.Forms.DataGridViewCellFormattingEventHandler(this.gridHistory_CellFormatting);
            this.gridHistory.KeyDown += new System.Windows.Forms.KeyEventHandler(this.gridHistory_KeyDown);
            // 
            // lblEmptyState
            // 
            this.lblEmptyState.BackColor = System.Drawing.Color.White;
            this.lblEmptyState.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lblEmptyState.Font = new System.Drawing.Font("Segoe UI Semibold", 12F, System.Drawing.FontStyle.Bold);
            this.lblEmptyState.ForeColor = System.Drawing.Color.FromArgb(110, 120, 135);
            this.lblEmptyState.Location = new System.Drawing.Point(0, 0);
            this.lblEmptyState.Name = "lblEmptyState";
            this.lblEmptyState.Size = new System.Drawing.Size(682, 352);
            this.lblEmptyState.TabIndex = 1;
            this.lblEmptyState.Text = "Пока нет операций.\r\n\r\nДобавьте доход или расход кнопками слева.";
            this.lblEmptyState.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.lblEmptyState.Visible = false;
            // 
            // panelPieHost
            // 
            this.panelPieHost.BackColor = System.Drawing.Color.FromArgb(245, 248, 252);
            this.panelPieHost.Controls.Add(this.pieExpenses);
            this.panelPieHost.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panelPieHost.Location = new System.Drawing.Point(682, 0);
            this.panelPieHost.Margin = new System.Windows.Forms.Padding(0);
            this.panelPieHost.Name = "panelPieHost";
            this.panelPieHost.Padding = new System.Windows.Forms.Padding(12, 0, 0, 0);
            this.panelPieHost.Size = new System.Drawing.Size(300, 352);
            this.panelPieHost.TabIndex = 1;
            // 
            // pieExpenses
            // 
            this.pieExpenses.BackColor = System.Drawing.Color.White;
            this.pieExpenses.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pieExpenses.Location = new System.Drawing.Point(12, 0);
            this.pieExpenses.Margin = new System.Windows.Forms.Padding(0);
            this.pieExpenses.Name = "pieExpenses";
            this.pieExpenses.Size = new System.Drawing.Size(288, 352);
            this.pieExpenses.TabIndex = 0;
            // 
            // panelFooter
            // 
            this.panelFooter.BackColor = System.Drawing.Color.FromArgb(236, 240, 245);
            this.panelFooter.ColumnCount = 1;
            this.panelFooter.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.panelFooter.Controls.Add(this.lblCopyright, 0, 0);
            this.panelFooter.Controls.Add(this.lblDataPath, 0, 1);
            this.panelFooter.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panelFooter.Location = new System.Drawing.Point(24, 658);
            this.panelFooter.Margin = new System.Windows.Forms.Padding(0);
            this.panelFooter.Name = "panelFooter";
            this.panelFooter.RowCount = 2;
            this.panelFooter.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 28F));
            this.panelFooter.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 20F));
            this.panelFooter.Size = new System.Drawing.Size(982, 52);
            this.panelFooter.TabIndex = 4;
            // 
            // lblCopyright
            // 
            this.lblCopyright.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lblCopyright.Font = new System.Drawing.Font("Segoe UI", 9.5F);
            this.lblCopyright.ForeColor = System.Drawing.Color.FromArgb(110, 120, 135);
            this.lblCopyright.Location = new System.Drawing.Point(0, 0);
            this.lblCopyright.Margin = new System.Windows.Forms.Padding(0);
            this.lblCopyright.Name = "lblCopyright";
            this.lblCopyright.Size = new System.Drawing.Size(982, 28);
            this.lblCopyright.TabIndex = 0;
            this.lblCopyright.Text = "Банчу.Н&Белько.В© 2026";
            this.lblCopyright.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // lblDataPath
            // 
            this.lblDataPath.AutoEllipsis = true;
            this.lblDataPath.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lblDataPath.Font = new System.Drawing.Font("Segoe UI", 8F);
            this.lblDataPath.ForeColor = System.Drawing.Color.FromArgb(110, 120, 135);
            this.lblDataPath.Location = new System.Drawing.Point(0, 28);
            this.lblDataPath.Margin = new System.Windows.Forms.Padding(0);
            this.lblDataPath.Name = "lblDataPath";
            this.lblDataPath.Size = new System.Drawing.Size(982, 24);
            this.lblDataPath.TabIndex = 1;
            this.lblDataPath.Text = "Файл данных:";
            this.lblDataPath.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // ctxHistory
            // 
            this.ctxHistory.ImageScalingSize = new System.Drawing.Size(20, 20);
            this.ctxHistory.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.menuEdit,
            this.menuDelete});
            this.ctxHistory.Name = "ctxHistory";
            this.ctxHistory.Size = new System.Drawing.Size(182, 52);
            this.ctxHistory.Opening += new System.ComponentModel.CancelEventHandler(this.ctxHistory_Opening);
            // 
            // menuEdit
            // 
            this.menuEdit.Name = "menuEdit";
            this.menuEdit.Size = new System.Drawing.Size(181, 24);
            this.menuEdit.Text = "✏ Изменить...";
            this.menuEdit.Click += new System.EventHandler(this.menuEdit_Click);
            // 
            // menuDelete
            // 
            this.menuDelete.Name = "menuDelete";
            this.menuDelete.Size = new System.Drawing.Size(181, 24);
            this.menuDelete.Text = "🗑 Удалить";
            this.menuDelete.Click += new System.EventHandler(this.menuDelete_Click);
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.FromArgb(245, 248, 252);
            this.ClientSize = new System.Drawing.Size(1280, 720);
            this.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.Controls.Add(this.panelMain);
            this.Controls.Add(this.panelSidebar);
            this.Margin = new System.Windows.Forms.Padding(4);
            this.MinimumSize = new System.Drawing.Size(1000, 620);
            this.Name = "Form1";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Кошелёк — учёт финансов";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.Form1_FormClosing);
            this.Load += new System.EventHandler(this.Form1_Load);
            this.panelSidebar.ResumeLayout(false);
            this.sidebarLayout.ResumeLayout(false);
            this.sidebarLayout.PerformLayout();
            this.sidebarMenu.ResumeLayout(false);
            this.panelMain.ResumeLayout(false);
            this.layoutRoot.ResumeLayout(false);
            this.layoutRoot.PerformLayout();
            this.cardsLayout.ResumeLayout(false);
            this.cardPrb.ResumeLayout(false);
            this.cardRub.ResumeLayout(false);
            this.cardUsd.ResumeLayout(false);
            this.cardEur.ResumeLayout(false);
            this.panelFilter.ResumeLayout(false);
            this.panelFilter.PerformLayout();
            this.filterFlow.ResumeLayout(false);
            this.filterFlow.PerformLayout();
            this.panelContent.ResumeLayout(false);
            this.panelGridHost.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.gridHistory)).EndInit();
            this.panelPieHost.ResumeLayout(false);
            this.panelFooter.ResumeLayout(false);
            this.panelFooter.PerformLayout();
            this.ctxHistory.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Panel panelSidebar;
        private System.Windows.Forms.TableLayoutPanel sidebarLayout;
        private System.Windows.Forms.Label lblAppTitle;
        private System.Windows.Forms.Label lblAppSubtitle;
        private System.Windows.Forms.TableLayoutPanel sidebarMenu;
        private System.Windows.Forms.Button btnIncome;
        private System.Windows.Forms.Button btnExpense;
        private System.Windows.Forms.Button btnExchange;
        private System.Windows.Forms.Button btnDebts;
        private System.Windows.Forms.Button btnEditBalance;
        private System.Windows.Forms.Button btnTheme;
        private System.Windows.Forms.Panel panelMain;
        private System.Windows.Forms.TableLayoutPanel layoutRoot;
        private System.Windows.Forms.Label lblWelcome;
        private System.Windows.Forms.TableLayoutPanel cardsLayout;
        private System.Windows.Forms.Panel cardPrb;
        private System.Windows.Forms.Label lblPrbTitle;
        private System.Windows.Forms.Label lblPrbAmount;
        private System.Windows.Forms.Panel cardRub;
        private System.Windows.Forms.Label lblRubTitle;
        private System.Windows.Forms.Label lblRubAmount;
        private System.Windows.Forms.Panel cardUsd;
        private System.Windows.Forms.Label lblUsdTitle;
        private System.Windows.Forms.Label lblUsdAmount;
        private System.Windows.Forms.Panel cardEur;
        private System.Windows.Forms.Label lblEurTitle;
        private System.Windows.Forms.Label lblEurAmount;
        private System.Windows.Forms.TableLayoutPanel panelFilter;
        private System.Windows.Forms.Label lblHistoryTitle;
        private System.Windows.Forms.FlowLayoutPanel filterFlow;
        private Krypton.Toolkit.KryptonComboBox cmbPeriod;
        private Krypton.Toolkit.KryptonComboBox cmbTypeFilter;
        private System.Windows.Forms.TextBox txtSearch;
        private System.Windows.Forms.Button btnDailyExpenses;
        private System.Windows.Forms.Button btnPrintExpenses;
        private System.Windows.Forms.TableLayoutPanel panelContent;
        private System.Windows.Forms.Panel panelGridHost;
        private System.Windows.Forms.DataGridView gridHistory;
        private System.Windows.Forms.Label lblEmptyState;
        private System.Windows.Forms.Panel panelPieHost;
        private Revenuemanagement.ExpensePiePanel pieExpenses;
        private System.Windows.Forms.TableLayoutPanel panelFooter;
        private System.Windows.Forms.Label lblCopyright;
        private System.Windows.Forms.Label lblDataPath;
        private System.Windows.Forms.ContextMenuStrip ctxHistory;
        private System.Windows.Forms.ToolStripMenuItem menuEdit;
        private System.Windows.Forms.ToolStripMenuItem menuDelete;
    }
}
