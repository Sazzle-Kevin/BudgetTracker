using BudgetTracker.UI;
var ui = new Interface();

while (true)
{
    ui.ShowMenu();

    switch (Console.ReadLine())
    {
        case "1":
            ui.AddTransaction();
            continue;

        case "2":
            ui.RemoveTransaction();
            continue;

        case "3":
            ui.ShowReport();
            continue;

        case "4":
            Console.WriteLine("May the Budget be with you. 🪙");
            return;

        default:
            Console.WriteLine(">>> Error: Please enter the number of your option <<<");
            continue;
    }
}
