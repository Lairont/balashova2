using System;
using Newtonsoft.Json;

namespace Revenuemanagement.Models
{
    /// <summary>
    /// Одна запись в истории операций (доход, расход, обмен, долг).
    /// </summary>
    public class Transaction
    {
        public Guid Id { get; set; } = Guid.NewGuid();

        public DateTime Date { get; set; } = DateTime.Now;

        public TransactionType Type { get; set; }

        /// <summary>Категория или краткое описание («Зарплата», «Продукты», «Обмен»).</summary>
        public string Category { get; set; } = string.Empty;

        public decimal Amount { get; set; }

        public CurrencyType Currency { get; set; }

        /// <summary>Дополнительный текст (например, детали обмена).</summary>
        public string Note { get; set; } = string.Empty;

        /// <summary>Магазин для категории «Продукты» (Шериф, Гарант, Другое).</summary>
        public string Shop { get; set; } = string.Empty;

        /// <summary>Для обмена: валюта, которую отдали.</summary>
        public CurrencyType? ExchangeFromCurrency { get; set; }

        /// <summary>Для обмена: сумма, которую отдали.</summary>
        public decimal? ExchangeFromAmount { get; set; }

        /// <summary>Связанный долг (для операций по долгам).</summary>
        public Guid? RelatedDebtId { get; set; }

        [JsonIgnore]
        public string TypeDisplay
        {
            get
            {
                switch (Type)
                {
                    case TransactionType.Income: return "Доход";
                    case TransactionType.Expense: return "Расход";
                    case TransactionType.Exchange: return "Обмен";
                    case TransactionType.DebtCreated: return "Долг";
                    case TransactionType.DebtPaid: return "Погашение";
                    case TransactionType.BalanceAdjustment: return "Корректировка";
                    default: return Type.ToString();
                }
            }
        }

        [JsonIgnore]
        public string CurrencyDisplay
        {
            get
            {
                switch (Currency)
                {
                    case CurrencyType.PRB: return "PRB (ПМР)";
                    case CurrencyType.RUB: return "RUB (РФ)";
                    case CurrencyType.USD: return "USD ($)";
                    case CurrencyType.EUR: return "EUR (€)";
                    case CurrencyType.MDL: return "MDL (лей)";
                    default: return Currency.ToString();
                }
            }
        }

        [JsonIgnore]
        public string CategoryDisplay
        {
            get
            {
                if (!string.IsNullOrWhiteSpace(Shop) &&
                    string.Equals(Category, "Продукты", StringComparison.OrdinalIgnoreCase))
                    return Category + " (" + Shop.Trim() + ")";
                return Category ?? string.Empty;
            }
        }
    }
}
