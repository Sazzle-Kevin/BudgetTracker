
using BudgetTracker.Events;
using BudgetTracker.Models;

namespace BudgetTracker.Services;

public class LoggerService
{
    private readonly string _logFilePath;

    public LoggerService(TransactionService transactionService)
    {
        Directory.CreateDirectory("logs");
        _logFilePath = Path.Combine("logs", "transactions.log");

        transactionService.TransactionAdded += OnTransactionAdded;
    }

    private void OnTransactionAdded(
        object? sender,
        TransactionAddedEventArgs e)
    {
        Transaction transaction = e.Transaction;

        string logEntry =
            $"{transaction.Timestamp:yyyy-MM-dd HH:mm:ss} | " +
            $"{transaction.Type} | " +
            $"{transaction.Description} | " +
            $"{transaction.Amount:F2}{Environment.NewLine}";

        File.AppendAllText(_logFilePath, logEntry);
    }
}
