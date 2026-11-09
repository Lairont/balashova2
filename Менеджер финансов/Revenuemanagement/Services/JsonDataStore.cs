using System;
using System.IO;
using System.Text;
using Newtonsoft.Json;
using Revenuemanagement.Models;

namespace Revenuemanagement.Services
{
    /// <summary>
    /// Сохранение и загрузка всех данных в локальный JSON-файл (без БД).
    /// Сохранение атомарное: сначала пишется временный файл, затем он заменяет
    /// основной. Предыдущая версия остаётся в файле .bak.
    /// </summary>
    public class JsonDataStore
    {
        private readonly string _filePath;
        private static readonly JsonSerializerSettings Settings = new JsonSerializerSettings
        {
            Formatting = Formatting.Indented,
            DateFormatString = "yyyy-MM-ddTHH:mm:ss",
            NullValueHandling = NullValueHandling.Ignore
        };

        public JsonDataStore(string fileName = "finance_data.json")
        {
            // Файл рядом с exe — удобно для резервных копий
            string folder = AppDomain.CurrentDomain.BaseDirectory;
            _filePath = Path.Combine(folder, fileName);
        }

        public string FilePath => _filePath;

        /// <summary>
        /// Предупреждение, если основной файл был повреждён и данные взяты из .bak.
        /// </summary>
        public string LastLoadWarning { get; private set; }

        public FinanceData Load()
        {
            LastLoadWarning = null;

            if (!File.Exists(_filePath))
                return new FinanceData();

            try
            {
                return LoadFile(_filePath);
            }
            catch (Exception mainError)
            {
                string backupPath = _filePath + ".bak";
                if (File.Exists(backupPath))
                {
                    try
                    {
                        var restored = LoadFile(backupPath);
                        LastLoadWarning =
                            "Основной файл данных был повреждён, поэтому загружена резервная копия." +
                            Environment.NewLine + "Проверьте последние операции." +
                            Environment.NewLine + Environment.NewLine + backupPath;
                        return restored;
                    }
                    catch
                    {
                        // Если и резервная копия не читается — покажем исходную ошибку.
                    }
                }

                throw new InvalidOperationException(
                    "Не удалось прочитать файл данных: " + _filePath +
                    Environment.NewLine + mainError.Message, mainError);
            }
        }

        private static FinanceData LoadFile(string path)
        {
            string json = File.ReadAllText(path, Encoding.UTF8);
            if (string.IsNullOrWhiteSpace(json))
                return new FinanceData();

            var data = JsonConvert.DeserializeObject<FinanceData>(json, Settings);
            return data ?? new FinanceData();
        }

        public void Save(FinanceData data)
        {
            if (data == null)
                throw new ArgumentNullException(nameof(data));

            string json = JsonConvert.SerializeObject(data, Settings);
            string folder = Path.GetDirectoryName(_filePath);
            if (!string.IsNullOrEmpty(folder) && !Directory.Exists(folder))
                Directory.CreateDirectory(folder);

            string tempPath = _filePath + ".tmp";
            Exception lastError = null;

            // Несколько попыток на случай, если файл на секунду занят (OneDrive, антивирус).
            for (int attempt = 0; attempt < 3; attempt++)
            {
                try
                {
                    File.WriteAllText(tempPath, json, new UTF8Encoding(false));

                    if (File.Exists(_filePath))
                    {
                        string backupPath = _filePath + ".bak";
                        try
                        {
                            File.Replace(tempPath, _filePath, backupPath, true);
                        }
                        catch (PlatformNotSupportedException)
                        {
                            File.Copy(tempPath, _filePath, true);
                            File.Delete(tempPath);
                        }
                        catch (IOException)
                        {
                            // File.Replace может не сработать на некоторых дисках/сетевых папках.
                            File.Copy(tempPath, _filePath, true);
                            File.Delete(tempPath);
                        }
                    }
                    else
                    {
                        File.Move(tempPath, _filePath);
                    }

                    return;
                }
                catch (Exception ex)
                {
                    lastError = ex;
                    TryDelete(tempPath);
                    if (attempt < 2)
                        System.Threading.Thread.Sleep(120);
                }
            }

            throw new InvalidOperationException(
                "Не удалось сохранить файл данных: " + _filePath +
                Environment.NewLine + (lastError != null ? lastError.Message : string.Empty), lastError);
        }

        private static void TryDelete(string path)
        {
            try
            {
                if (File.Exists(path))
                    File.Delete(path);
            }
            catch
            {
                // Нечего делать — временный файл будет перезаписан при следующем сохранении.
            }
        }
    }
}
