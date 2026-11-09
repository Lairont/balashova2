using System.Collections.Generic;

namespace Revenuemanagement.Models
{
    /// <summary>
    /// Корневой объект, который целиком сохраняется в finance_data.json.
    /// </summary>
    public class FinanceData
    {
        public string OwnerName { get; set; } = "Юлия";

        public Balances Balances { get; set; } = new Balances();

        public List<Transaction> Transactions { get; set; } = new List<Transaction>();

        public List<Debt> Debts { get; set; } = new List<Debt>();

        /// <summary>true — тёмная тема интерфейса.</summary>
        public bool IsDarkTheme { get; set; } = true;

        /// <summary>Категории расходов (редактируются пользователем).</summary>
        public List<string> ExpenseCategories { get; set; } = new List<string>();

        /// <summary>Категории доходов (редактируются пользователем).</summary>
        public List<string> IncomeCategories { get; set; } = new List<string>();

        /// <summary>Последние курсы обмена: ключ «USD_PRB» — сколько PRB за 1 USD.</summary>
        public Dictionary<string, decimal> ExchangeRates { get; set; } = new Dictionary<string, decimal>();
    }
}
