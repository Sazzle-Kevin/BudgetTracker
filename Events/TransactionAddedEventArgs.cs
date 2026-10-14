
using BudgetTracker.Models;

namespace BudgetTracker.Events;

public class TransactionAddedEventArgs(Transaction transaction)
    : EventArgs
{
    public Transaction Transaction { get; } = transaction;
}
