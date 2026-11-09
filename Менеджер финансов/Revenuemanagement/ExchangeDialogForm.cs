using System;
using System.Drawing;
using System.Windows.Forms;
using Revenuemanagement.Models;
using Revenuemanagement.Services;

namespace Revenuemanagement
{
    /// <summary>
    /// Конвертер валют. Для пары с USD/EUR курс всегда указан за одну
    /// единицу USD/EUR: например, 1 USD = 16.35 PRB.
    /// </summary>
    public class ExchangeDialogForm : Form
    {
        private readonly FinanceManager _manager;
        private ComboBox cmbFrom;
        private ComboBox cmbTo;
        private Label lblRateHint;
        private TextBox txtRate;
        private TextBox txtAmount;
        private Label lblCalcGive;
        private Label lblCalcReceive;
        private Label lblBalances;
        private Button btnExchange;
        private bool _changingCurrencies;

        public ExchangeDialogForm(FinanceManager manager)
        {
            _manager = manager;
            BuildUi();
            LoadSavedRate();
            Recalculate();
        }

        private void BuildUi()
        {
            Text = "Обмен валют";
            StartPosition = FormStartPosition.CenterParent;
            FormBorderStyle = FormBorderStyle.FixedDialog;
            MaximizeBox = false;
            MinimizeBox = false;
            ClientSize = new Size(740, 650);
            Padding = new Padding(40, 30, 40, 30);
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
                Text = "💱  Обмен валют",
                AutoSize = true,
                Font = UiHelper.TitleFont,
                ForeColor = UiHelper.TextDark,
                Margin = new Padding(0, 0, 0, 10)
            };

            var hint = UiHelper.CreateHintLabel(
                "Введите сумму, которую хотите получить. Например, 15 USD — программа рассчитает, сколько PRB списать по курсу. Последний курс пары сохранится для следующего обмена.",
                632);

            cmbFrom = new ComboBox
            {
                Width = 306,
                DropDownStyle = ComboBoxStyle.DropDownList,
                Font = UiHelper.BodyFont
            };
            UiHelper.FillCurrencyCombo(cmbFrom, CurrencyType.PRB);

            cmbTo = new ComboBox
            {
                Width = 306,
                DropDownStyle = ComboBoxStyle.DropDownList,
                Font = UiHelper.BodyFont
            };
            UiHelper.FillCurrencyCombo(cmbTo, CurrencyType.USD);

            var currencies = new FlowLayoutPanel
            {
                FlowDirection = FlowDirection.LeftToRight,
                AutoSize = true,
                AutoSizeMode = AutoSizeMode.GrowAndShrink,
                WrapContents = false,
                Margin = new Padding(0, 0, 0, 18),
                Padding = new Padding(0, 0, 0, 5)
            };
            var fromWrap = UiHelper.CreateFieldSection("Из валюты", cmbFrom);
            fromWrap.Margin = new Padding(0, 0, 20, 0);
            currencies.Controls.Add(fromWrap);
            currencies.Controls.Add(UiHelper.CreateFieldSection("В валюту", cmbTo));

            // Курс: подпись обновляется при смене пары валют.
            var rateSection = new Panel
            {
                Width = 632,
                Margin = new Padding(0, 0, 0, 18)
            };
            lblRateHint = new Label
            {
                Name = "fieldLabel",
                AutoSize = true,
                ForeColor = UiHelper.TextMuted,
                Font = UiHelper.FieldLabelFont,
                Location = new Point(0, 0)
            };
            txtRate = new TextBox
            {
                Width = 632,
                Font = UiHelper.BodyFont,
                Location = new Point(0, lblRateHint.PreferredHeight + 8)
            };
            rateSection.Controls.Add(lblRateHint);
            rateSection.Controls.Add(txtRate);
            rateSection.Height = txtRate.Location.Y + txtRate.PreferredHeight + 6;

            txtAmount = new TextBox { Width = 632, Font = UiHelper.BodyFont };
            var amountSection = UiHelper.CreateFieldSection("Сколько получить", txtAmount, 18);

            // Блок результата
            var resultPanel = new Panel
            {
                Width = 632,
                Height = 94,
                Padding = new Padding(16, 12, 16, 12),
                Margin = new Padding(0, 0, 0, 18)
            };
            lblCalcReceive = new Label
            {
                AutoSize = true,
                Font = new Font("Segoe UI Semibold", 13F, FontStyle.Bold),
                ForeColor = UiHelper.AccentGreen,
                Location = new Point(16, 12)
            };
            lblCalcGive = new Label
            {
                AutoSize = true,
                Font = new Font("Segoe UI Semibold", 11F, FontStyle.Bold),
                ForeColor = UiHelper.AccentRed,
                Location = new Point(16, 50)
            };
            resultPanel.Controls.Add(lblCalcReceive);
            resultPanel.Controls.Add(lblCalcGive);

            lblBalances = new Label
            {
                AutoSize = true,
                Font = UiHelper.HintFont,
                ForeColor = UiHelper.AccentBlue,
                Margin = new Padding(0, 0, 0, 16)
            };
            RefreshBalancesLabel();

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
                DialogResult = DialogResult.Cancel,
                Size = new Size(190, 48),
                Margin = new Padding(14, 0, 0, 0)
            };
            UiHelper.StyleFlatButton(cancel, Color.FromArgb(200, 205, 215), UiHelper.TextDark);
            cancel.TextAlign = ContentAlignment.MiddleCenter;
            cancel.Padding = Padding.Empty;

            btnExchange = new Button { Text = "Выполнить обмен", Size = new Size(330, 48) };
            UiHelper.StyleFlatButton(btnExchange, UiHelper.AccentGold, Color.White);
            btnExchange.TextAlign = ContentAlignment.MiddleCenter;
            btnExchange.Padding = Padding.Empty;
            btnExchange.Click += BtnExchange_Click;

            buttons.Controls.Add(cancel);
            buttons.Controls.Add(btnExchange);

            layout.Controls.Add(title);
            layout.Controls.Add(hint);
            layout.Controls.Add(currencies);
            layout.Controls.Add(rateSection);
            layout.Controls.Add(amountSection);
            layout.Controls.Add(resultPanel);
            layout.Controls.Add(lblBalances);
            layout.Controls.Add(buttons);
            Controls.Add(layout);

            AcceptButton = btnExchange;
            CancelButton = cancel;

            cmbFrom.SelectedIndexChanged += CurrencyPairChanged;
            cmbTo.SelectedIndexChanged += CurrencyPairChanged;
            txtRate.TextChanged += (s, e) => Recalculate();
            txtAmount.TextChanged += (s, e) => Recalculate();

            Theme.ApplyTo(this);

            // Карточка результата должна отличаться от фона диалога.
            resultPanel.BackColor = Theme.BgCard;

            UpdateRateHintText();
        }

        private void CurrencyPairChanged(object sender, EventArgs e)
        {
            if (_changingCurrencies)
                return;

            if (UiHelper.GetSelectedCurrency(cmbFrom) == UiHelper.GetSelectedCurrency(cmbTo))
            {
                _changingCurrencies = true;
                cmbTo.SelectedIndex = (cmbTo.SelectedIndex + 1) % cmbTo.Items.Count;
                _changingCurrencies = false;
            }

            LoadSavedRate();
            UpdateRateHintText();
            Recalculate();
        }

        private void GetRateCurrencies(out CurrencyType baseCurrency, out CurrencyType quoteCurrency)
        {
            CurrencyType from = UiHelper.GetSelectedCurrency(cmbFrom);
            CurrencyType to = UiHelper.GetSelectedCurrency(cmbTo);
            if (IsHardCurrency(from) && IsLocalCurrency(to))
            {
                baseCurrency = from;
                quoteCurrency = to;
            }
            else if (IsHardCurrency(to) && IsLocalCurrency(from))
            {
                baseCurrency = to;
                quoteCurrency = from;
            }
            else
            {
                baseCurrency = from;
                quoteCurrency = to;
            }
        }

        private static bool IsHardCurrency(CurrencyType currency)
        {
            return currency == CurrencyType.USD || currency == CurrencyType.EUR;
        }

        private static bool IsLocalCurrency(CurrencyType currency)
        {
            return currency == CurrencyType.PRB || currency == CurrencyType.RUB || currency == CurrencyType.MDL;
        }

        private void LoadSavedRate()
        {
            GetRateCurrencies(out CurrencyType baseCurrency, out CurrencyType quoteCurrency);
            decimal? savedRate = _manager.GetSavedExchangeRate(baseCurrency, quoteCurrency);
            txtRate.Text = savedRate.HasValue ? savedRate.Value.ToString("0.####") : string.Empty;
        }

        private void UpdateRateHintText()
        {
            GetRateCurrencies(out CurrencyType baseCurrency, out CurrencyType quoteCurrency);
            lblRateHint.Text = string.Format(
                "Курс: 1 {0} = сколько {1}?",
                FinanceManager.CurrencyShort(baseCurrency),
                FinanceManager.CurrencyShort(quoteCurrency));
        }

        private bool TryCalculate(out decimal sourceAmount, out decimal receivedAmount)
        {
            sourceAmount = 0m;
            receivedAmount = 0m;
            if (!UiHelper.TryParseAmount(txtAmount.Text, out sourceAmount) ||
                !UiHelper.TryParseAmount(txtRate.Text, out decimal rate))
                return false;

            CurrencyType from = UiHelper.GetSelectedCurrency(cmbFrom);
            CurrencyType to = UiHelper.GetSelectedCurrency(cmbTo);
            if (from == to)
                return false;

            GetRateCurrencies(out CurrencyType baseCurrency, out CurrencyType quoteCurrency);
            // В поле вводится сумма, которую пользователь хочет получить.
            receivedAmount = sourceAmount;
            if (from == baseCurrency && to == quoteCurrency)
                sourceAmount = receivedAmount / rate;
            else if (from == quoteCurrency && to == baseCurrency)
                sourceAmount = receivedAmount * rate;
            else
                return false;

            sourceAmount = Math.Round(sourceAmount, 2, MidpointRounding.AwayFromZero);
            return receivedAmount > 0 && sourceAmount > 0;
        }

        private void Recalculate()
        {
            CurrencyType from = UiHelper.GetSelectedCurrency(cmbFrom);
            CurrencyType to = UiHelper.GetSelectedCurrency(cmbTo);

            if (from == to)
            {
                lblCalcGive.Text = "К оплате: —";
                lblCalcReceive.Text = "Выберите разные валюты";
                lblCalcReceive.ForeColor = UiHelper.TextMuted;
                btnExchange.Enabled = false;
                return;
            }

            if (!UiHelper.TryParseAmount(txtAmount.Text, out decimal receivedAmount))
            {
                lblCalcGive.Text = "К оплате: —";
                lblCalcReceive.Text = "Введите сумму, которую хотите получить";
                lblCalcReceive.ForeColor = UiHelper.TextMuted;
                btnExchange.Enabled = false;
                return;
            }

            if (!UiHelper.TryParseAmount(txtRate.Text, out decimal rate))
            {
                lblCalcReceive.Text = string.Format(
                    "Получаете: {0:N2} {1}", receivedAmount, FinanceManager.CurrencyShort(to));
                lblCalcGive.Text = "К оплате: введите курс больше нуля";
                lblCalcGive.ForeColor = UiHelper.TextMuted;
                btnExchange.Enabled = false;
                return;
            }

            GetRateCurrencies(out CurrencyType baseCurrency, out CurrencyType quoteCurrency);
            decimal sourceAmount;
            if (from == baseCurrency && to == quoteCurrency)
                sourceAmount = receivedAmount / rate;
            else if (from == quoteCurrency && to == baseCurrency)
                sourceAmount = receivedAmount * rate;
            else
            {
                lblCalcReceive.Text = "Не удалось рассчитать обмен";
                lblCalcReceive.ForeColor = UiHelper.TextMuted;
                btnExchange.Enabled = false;
                return;
            }

            sourceAmount = Math.Round(sourceAmount, 2, MidpointRounding.AwayFromZero);

            lblCalcReceive.Text = string.Format(
                "Получаете: {0:N2} {1}", receivedAmount, FinanceManager.CurrencyShort(to));

            decimal available = _manager.Balances.Get(from);
            if (available < sourceAmount)
            {
                lblCalcGive.Text = string.Format(
                    "Недостаточно средств: на счету {0:N2} {1}",
                    available, FinanceManager.CurrencyShort(from));
                lblCalcGive.ForeColor = UiHelper.AccentRed;
                btnExchange.Enabled = false;
                return;
            }

            lblCalcGive.Text = string.Format(
                "К оплате: {0:N2} {1}", sourceAmount, FinanceManager.CurrencyShort(from));
            lblCalcGive.ForeColor = UiHelper.AccentRed;
            btnExchange.Enabled = receivedAmount > 0;
        }

        private void RefreshBalancesLabel()
        {
            Balances balances = _manager.Balances;
            lblBalances.Text = string.Format(
                "На счетах: PRB {0:N2}   ·   RUB {1:N2}   ·   USD {2:N2}   ·   EUR {3:N2}   ·   MDL {4:N2}",
                balances.PRB, balances.RUB, balances.USD, balances.EUR, balances.MDL);
        }

        private void BtnExchange_Click(object sender, EventArgs e)
        {
            try
            {
                if (!TryCalculate(out decimal sourceAmount, out decimal receivedAmount))
                    throw new InvalidOperationException("Введите корректную сумму и текущий курс.");

                CurrencyType from = UiHelper.GetSelectedCurrency(cmbFrom);
                CurrencyType to = UiHelper.GetSelectedCurrency(cmbTo);
                GetRateCurrencies(out CurrencyType baseCurrency, out CurrencyType quoteCurrency);
                if (!UiHelper.TryParseAmount(txtRate.Text, out decimal rate))
                    throw new InvalidOperationException("Курс должен быть больше нуля.");

                // Сначала сохраняем курс, затем выполняем обмен. Так при ошибке обмена
                // курс всё равно останется введённым, а повторное нажатие не задвоит операцию.
                _manager.SaveExchangeRate(baseCurrency, quoteCurrency, rate);
                _manager.Exchange(from, sourceAmount, to, receivedAmount);

                DialogResult = DialogResult.OK;
            }
            catch (Exception exception)
            {
                UiHelper.ShowError(this, exception.Message);
            }
        }
    }
}
