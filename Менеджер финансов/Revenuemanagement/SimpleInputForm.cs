using System.Drawing;
using System.Windows.Forms;

namespace Revenuemanagement
{
    /// <summary>Небольшой диалог ввода одной строки (новая категория и т.п.).</summary>
    public class SimpleInputForm : Form
    {
        private readonly TextBox _textBox;

        public string InputText => _textBox.Text.Trim();

        public SimpleInputForm(string title, string prompt, string defaultValue = "")
        {
            Text = title;
            StartPosition = FormStartPosition.CenterParent;
            FormBorderStyle = FormBorderStyle.FixedDialog;
            MaximizeBox = false;
            MinimizeBox = false;
            ClientSize = new Size(500, 200);
            Padding = new Padding(24);
            Font = UiHelper.BodyFont;

            var lbl = new Label
            {
                Text = prompt,
                Location = new Point(24, 24),
                Size = new Size(452, 40),
                ForeColor = Theme.TextMuted
            };

            _textBox = new TextBox
            {
                Location = new Point(24, 72),
                Size = new Size(452, 30),
                Font = UiHelper.BodyFont,
                Text = defaultValue ?? string.Empty
            };

            var btnOk = new Button
            {
                Text = "OK",
                Size = new Size(100, 40),
                Location = new Point(256, 128)
            };
            UiHelper.StyleFlatButton(btnOk, Theme.AccentGreen, Color.White);
            btnOk.TextAlign = ContentAlignment.MiddleCenter;
            btnOk.Click += (s, e) =>
            {
                if (string.IsNullOrWhiteSpace(_textBox.Text))
                {
                    UiHelper.ShowError(this, "Введите название.");
                    return;
                }
                DialogResult = DialogResult.OK;
            };

            var btnCancel = new Button
            {
                Text = "Отмена",
                Size = new Size(100, 40),
                Location = new Point(376, 128)
            };
            UiHelper.StyleFlatButton(btnCancel, Color.FromArgb(200, 205, 215), Theme.TextPrimary);
            btnCancel.TextAlign = ContentAlignment.MiddleCenter;
            btnCancel.Click += (s, e) => DialogResult = DialogResult.Cancel;

            Controls.Add(lbl);
            Controls.Add(_textBox);
            Controls.Add(btnOk);
            Controls.Add(btnCancel);
            AcceptButton = btnOk;
            CancelButton = btnCancel;

            Theme.ApplyTo(this);
        }
    }
}
