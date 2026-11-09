using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;
using Revenuemanagement.Models;

namespace Revenuemanagement
{
    /// <summary>Расходы по дням за выбранный период.</summary>
    public class DailyExpensesForm : Form
    {
        public DailyExpensesForm(IReadOnlyList<DayExpenseTotal> days, string periodTitle)
        {
            Text = "Расходы по дням";
            StartPosition = FormStartPosition.CenterParent;
            FormBorderStyle = FormBorderStyle.Sizable;
            MinimizeBox = false;
            ClientSize = new Size(560, 460);
            MinimumSize = new Size(420, 320);
            Padding = new Padding(16);
            BackColor = Theme.BgMain;
            Font = UiHelper.BodyFont;

            var root = new TableLayoutPanel
            {
                Dock = DockStyle.Fill,
                ColumnCount = 1,
                RowCount = 3,
                Padding = new Padding(0),
                Margin = new Padding(0)
            };
            root.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100F));
            root.RowStyles.Add(new RowStyle(SizeType.AutoSize));
            root.RowStyles.Add(new RowStyle(SizeType.Percent, 100F));
            root.RowStyles.Add(new RowStyle(SizeType.AutoSize));

            var lbl = new Label
            {
                Text = "📅  " + periodTitle,
                Font = UiHelper.HeaderFont,
                ForeColor = Theme.TextPrimary,
                AutoSize = true,
                Margin = new Padding(0, 0, 0, 10)
            };

            var list = new ListBox
            {
                Dock = DockStyle.Fill,
                Font = UiHelper.BodyFont,
                IntegralHeight = false,
                BorderStyle = BorderStyle.FixedSingle,
                HorizontalScrollbar = true,
                Margin = new Padding(0)
            };

            if (days == null || days.Count == 0)
                list.Items.Add("За этот период расходов нет.");
            else
            {
                foreach (var day in days)
                    list.Items.Add(day.Summary);
            }

            var buttons = new FlowLayoutPanel
            {
                Dock = DockStyle.Fill,
                AutoSize = true,
                AutoSizeMode = AutoSizeMode.GrowAndShrink,
                FlowDirection = FlowDirection.RightToLeft,
                WrapContents = false,
                Margin = new Padding(0, 12, 0, 0)
            };

            var btnClose = new Button
            {
                Text = "Закрыть",
                Size = new Size(130, 42),
                Margin = new Padding(0)
            };
            UiHelper.StyleFlatButton(btnClose, Color.FromArgb(200, 205, 215), Theme.TextPrimary);
            btnClose.TextAlign = ContentAlignment.MiddleCenter;
            btnClose.Padding = Padding.Empty;
            btnClose.Click += (s, e) => Close();
            buttons.Controls.Add(btnClose);

            root.Controls.Add(lbl, 0, 0);
            root.Controls.Add(list, 0, 1);
            root.Controls.Add(buttons, 0, 2);
            Controls.Add(root);

            CancelButton = btnClose;

            Theme.ApplyTo(this);
        }
    }
}
