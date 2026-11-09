using System;
using System.IO;
using System.Text;
using System.Windows.Forms;
using Krypton.Toolkit;

namespace Revenuemanagement
{
    internal static class Program
    {
        private static bool _showingError;
        private static KryptonManager _kryptonManager;

        internal static void ApplyKryptonTheme(bool dark)
        {
            if (_kryptonManager != null)
                _kryptonManager.GlobalPaletteMode = dark
                    ? PaletteMode.Microsoft365BlackDarkMode
                    : PaletteMode.Microsoft365White;
        }

        /// <summary>
        /// Главная точка входа для приложения.
        /// </summary>
        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);

            // Ловим необработанные ошибки, чтобы показать понятное сообщение
            // и сохранить подробности в error.log рядом с программой.
            Application.SetUnhandledExceptionMode(UnhandledExceptionMode.CatchException);
            Application.ThreadException += (s, e) => HandleException(e.Exception, true);
            AppDomain.CurrentDomain.UnhandledException +=
                (s, e) => HandleException(e.ExceptionObject as Exception, false);

            using (var krypton = new KryptonManager())
            {
                _kryptonManager = krypton;
                krypton.GlobalPaletteMode = PaletteMode.Microsoft365BlackDarkMode;
                Application.Run(new Form1());
                _kryptonManager = null;
            }
        }

        private static void HandleException(Exception exception, bool canContinue)
        {
            if (_showingError)
                return;

            _showingError = true;

            string logPath = null;
            try
            {
                logPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "error.log");
                var text = new StringBuilder();
                text.AppendLine(DateTime.Now.ToString("dd.MM.yyyy HH:mm:ss"));
                text.AppendLine(exception == null ? "Неизвестная ошибка" : exception.ToString());
                text.AppendLine(new string('-', 60));
                File.AppendAllText(logPath, text.ToString(), Encoding.UTF8);
            }
            catch
            {
                logPath = null;
            }

            string message = exception == null ? string.Empty : exception.Message;
            if (string.IsNullOrWhiteSpace(message))
                message = "Произошла непредвиденная ошибка.";

            string advice = canContinue
                ? "Попробуйте повторить действие. Если ошибка повторяется — перезапустите программу."
                : "Программа будет закрыта.";

            MessageBox.Show(
                "Произошла непредвиденная ошибка.\n\n" + message + "\n\n" + advice +
                (logPath != null ? "\n\nПодробности сохранены в файл:\n" + logPath : string.Empty),
                "Ошибка",
                MessageBoxButtons.OK,
                MessageBoxIcon.Error);

            _showingError = false;
        }
    }
}
