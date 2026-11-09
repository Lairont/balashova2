namespace Revenuemanagement.Models
{
    /// <summary>
    /// Остатки по валютам приложения.
    /// </summary>
    public class Balances
    {
        public decimal PRB { get; set; }
        public decimal RUB { get; set; }
        public decimal USD { get; set; }
        public decimal EUR { get; set; }
        public decimal MDL { get; set; }

        public decimal Get(CurrencyType currency)
        {
            switch (currency)
            {
                case CurrencyType.PRB: return PRB;
                case CurrencyType.RUB: return RUB;
                case CurrencyType.USD: return USD;
                case CurrencyType.EUR: return EUR;
                case CurrencyType.MDL: return MDL;
                default: return 0m;
            }
        }

        public void Set(CurrencyType currency, decimal value)
        {
            switch (currency)
            {
                case CurrencyType.PRB: PRB = value; break;
                case CurrencyType.RUB: RUB = value; break;
                case CurrencyType.USD: USD = value; break;
                case CurrencyType.EUR: EUR = value; break;
                case CurrencyType.MDL: MDL = value; break;
            }
        }

        public void Add(CurrencyType currency, decimal amount)
        {
            Set(currency, Get(currency) + amount);
        }
    }
}
