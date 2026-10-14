
using System.Text.Json;
using System.Text.Json.Serialization;
using BudgetTracker.Models;

namespace BudgetTracker.Services;

public class StorageService
{
    private readonly string _dataDirectory;

    private readonly JsonSerializerOptions _jsonOptions = new()
    {
        WriteIndented = true,
        Converters = { new JsonStringEnumConverter() }
    };

    public StorageService(string dataDirectory)
    {
        _dataDirectory = dataDirectory;
        Directory.CreateDirectory(_dataDirectory);
    }
    public void SaveTransaction(Transaction transaction)
    {
        string fileName = $"{transaction.Timestamp:yyyy-MM-dd}.json";
        string filePath = Path.Combine(_dataDirectory, fileName);

        List<Transaction> transactions = File.Exists(filePath)
            ? JsonSerializer.Deserialize<List<Transaction>>(
                File.ReadAllText(filePath), _jsonOptions) ?? new()
            : new();

        transactions.Add(transaction);

        string json = JsonSerializer.Serialize(transactions, _jsonOptions);
        File.WriteAllText(filePath, json);
    }


    public List<Transaction> LoadTransactions(
        DateOnly startDate,
        DateOnly endDate)
    {
        var transactions = new List<Transaction>();

        for (var date = startDate; date <= endDate; date = date.AddDays(1))
        {
            string fileName = $"{date:yyyy-MM-dd}.json";
            string filePath = Path.Combine(_dataDirectory, fileName);

            if (!File.Exists(filePath))
                continue;

            string json = File.ReadAllText(filePath);

            var dailyTransactions =
                JsonSerializer.Deserialize<List<Transaction>>(
                    json, _jsonOptions) ?? new();

            transactions.AddRange(dailyTransactions);
        }

        return transactions;
    }


    public bool RemoveTransaction(Guid id)
    {
        foreach (string filePath in Directory.GetFiles(_dataDirectory, "*.json"))
        {
            string json = File.ReadAllText(filePath);

            var transactions =
                JsonSerializer.Deserialize<List<Transaction>>(
                    json, _jsonOptions) ?? new();

            int removed = transactions.RemoveAll(t => t.Id == id);

            if (removed == 0)
                continue;

            string updatedJson =
                JsonSerializer.Serialize(transactions, _jsonOptions);

            File.WriteAllText(filePath, updatedJson);

            return true;
        }

        return false;
    }

};


