namespace BudgetTracker.Models;

public enum TransactionType
{
    Income,
    Expense
}

public record Transaction(
    Guid Id,
    DateTimeOffset Timestamp,
    TransactionType Type,
    string Description,
    decimal Amount
);