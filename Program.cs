
using BudgetTracker.Services;
using BudgetTracker.UI;

var storageService = new StorageService("data");
var transactionService = new TransactionService(storageService);
var loggerService = new LoggerService(transactionService);
var ui = new Interface(transactionService);

while (true)
{
    ui.ShowMenu();

    switch (Console.ReadLine())
    {
        case "1":
            ui.AddTransaction();
            break;

        case "2":
            ui.RemoveTransaction();
            break;

        case "3":
            ui.ShowReport();
            break;

        case "4":
            Console.WriteLine("May the Budget be with you. 🪙");
            return;

        default:
            Console.WriteLine("Invalid option. Please try again.");
            break;
    }
}
