namespace Revenuemanagement.Models
{
    /// <summary>
    /// Тип финансовой операции.
    /// </summary>
    public enum TransactionType
    {
        Income = 0,
        Expense = 1,
        Exchange = 2,
        DebtCreated = 3,
        DebtPaid = 4,

        /// <summary>Ручная корректировка баланса (запись только в истории).</summary>
        BalanceAdjustment = 5
    }
}
