
using BudgetTracker.Models;
using BudgetTracker.Events;

namespace BudgetTracker.Services;

public class TransactionService(StorageService storageService)
{
    public event EventHandler<TransactionAddedEventArgs>? TransactionAdded;

    public void AddTransaction(Transaction transaction)
    {
        if (transaction.Amount <= 0)
            throw new ArgumentException("Amount must be positive.");

        storageService.SaveTransaction(transaction);

        TransactionAdded?.Invoke(
            this,
            new TransactionAddedEventArgs(transaction)
        );
    }

    public bool RemoveTransaction(Guid id)
    {
        return storageService.RemoveTransaction(id);
    }

    public List<Transaction> GetTransactions(
        DateOnly startDate,
        DateOnly endDate)
    {
        if (startDate > endDate)
            throw new ArgumentException("Start date must not be after end date.");

        return storageService.LoadTransactions(startDate, endDate);
    }
}
