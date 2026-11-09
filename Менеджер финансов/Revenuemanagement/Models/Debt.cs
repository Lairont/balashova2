using System;
using Newtonsoft.Json;

namespace Revenuemanagement.Models
{
    /// <summary>
    /// Долг: кто должен Юлии или кому должна она.
    /// </summary>
    public class Debt
    {
        public Guid Id { get; set; } = Guid.NewGuid();

        public string PersonName { get; set; } = string.Empty;

        public decimal Amount { get; set; }

        public CurrencyType Currency { get; set; }

        /// <summary>
        /// true  — человек должен Юлии;
        /// false — Юлия должна человеку.
        /// </summary>
        public bool IsOwedToMe { get; set; }

        public bool IsPaid { get; set; }

        public DateTime CreatedDate { get; set; } = DateTime.Now;

        /// <summary>До какого числа нужно погасить долг.</summary>
        public DateTime? DueDate { get; set; }

        public string Note { get; set; } = string.Empty;

        [JsonIgnore]
        public string DirectionDisplay => IsOwedToMe ? "Должны мне" : "Я должна";

        [JsonIgnore]
        public string CurrencyDisplay
        {
            get
            {
                switch (Currency)
                {
                    case CurrencyType.PRB: return "PRB";
                    case CurrencyType.RUB: return "RUB";
                    case CurrencyType.USD: return "USD";
                    case CurrencyType.EUR: return "EUR";
                    default: return Currency.ToString();
                }
            }
        }

        [JsonIgnore]
        public string StatusDisplay => IsPaid ? "Погашен" : "Активен";

        [JsonIgnore]
        public string DueDateDisplay => DueDate.HasValue ? DueDate.Value.ToString("dd.MM.yyyy") : "—";

        [JsonIgnore]
        public bool IsOverdue => !IsPaid && DueDate.HasValue && DueDate.Value.Date < DateTime.Today;
    }
}
