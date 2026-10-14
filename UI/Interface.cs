using BudgetTracker.Models;
using BudgetTracker.Services;

namespace BudgetTracker.UI;

public class Interface(TransactionService transactionService)
{
    public void ShowMenu()
    {
        Console.WriteLine("\nChoose an option (1 - 4):");
        Console.WriteLine("---------------------------");
        Console.WriteLine("1. Add transaction");
        Console.WriteLine("2. Remove transaction");
        Console.WriteLine("3. Show report");
        Console.WriteLine("4. Exit");
    }

    public void AddTransaction()
    {
        Console.Write("Type (Income/Expense): ");
        if (!Enum.TryParse(Console.ReadLine(), true, out TransactionType type)
            || !Enum.IsDefined(type))
        {
            Console.WriteLine("Invalid type.");
            return;
        }

        Console.Write("Description: ");
        string description = Console.ReadLine() ?? "";

        if (string.IsNullOrWhiteSpace(description))
        {
            Console.WriteLine("Description cannot be empty.");
            return;
        }

        Console.Write("Amount: ");
        if (!decimal.TryParse(Console.ReadLine(), out decimal amount)
            || amount <= 0)
        {
            Console.WriteLine("Invalid amount.");
            return;
        }

        var transaction = new Transaction(
            Guid.NewGuid(),
            DateTimeOffset.Now,
            type,
            description,
            amount
        );

        transactionService.AddTransaction(transaction);
        Console.WriteLine("Transaction added.");
    }

    public void RemoveTransaction()
    {
        Console.Write("Transaction ID: ");

        if (!Guid.TryParse(Console.ReadLine(), out Guid id))
        {
            Console.WriteLine("Invalid ID.");
            return;
        }

        Console.WriteLine(
            transactionService.RemoveTransaction(id)
                ? "Transaction removed."
                : "Transaction not found."
        );
    }

    public void ShowReport()
    {
        Console.Write("Start date (yyyy-MM-dd): ");
        if (!DateOnly.TryParseExact(
                Console.ReadLine(), "yyyy-MM-dd",
                out DateOnly start))
        {
            Console.WriteLine("Invalid start date.");
            return;
        }

        Console.Write("End date (yyyy-MM-dd): ");
        if (!DateOnly.TryParseExact(
                Console.ReadLine(), "yyyy-MM-dd",
                out DateOnly end) || start > end)
        {
            Console.WriteLine("Invalid end date or date range.");
            return;
        }

        var transactions =
            transactionService.GetTransactions(start, end);

        foreach (var t in transactions)
            Console.WriteLine(
                $"{t.Timestamp:yyyy-MM-dd} | {t.Type} | " +
                $"{t.Description} | {t.Amount:F2}"
            );

        decimal income = transactions
            .Where(t => t.Type == TransactionType.Income)
            .Sum(t => t.Amount);

        decimal expenses = transactions
            .Where(t => t.Type == TransactionType.Expense)
            .Sum(t => t.Amount);

        Console.WriteLine($"Income: {income:F2}");
        Console.WriteLine($"Expenses: {expenses:F2}");
        Console.WriteLine($"Balance: {income - expenses:F2}");
    }
}