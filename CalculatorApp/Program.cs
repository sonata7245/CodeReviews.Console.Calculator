using System.Text.RegularExpressions;
using System.Xml.Serialization;
using CalculatorLibrary;

class Program
{
    static void Main(string[] args)
    {
        bool endApp = false;
        int count = 0;
        // Display title as the C# console calculator app.
        Console.WriteLine("Console Calculator in C#\r");
        Console.WriteLine("------------------------\n");

        Calculator calculator = new Calculator();

        while (!endApp)
        {
            // Declare variables and set to empty.
            // Use Nullable types (with ?) to match type of System.Console.ReadLine
            string? numInput1 = "";
            string? numInput2 = "";
            double result = 0;

            endApp = ShowMenu();
            if (endApp == true)
            { break; }

            // Ask the user to type the first number.
            Console.WriteLine("Collecting your first number...");
            numInput1 = GetNumInput();

            double cleanNum1 = 0;
            while (!double.TryParse(numInput1, out cleanNum1))
            {
                Console.Write("This is not valid input. Please enter a numeric value: ");
                numInput1 = Console.ReadLine();
            }

            // Ask the user to type the second number.
            Console.WriteLine("Collecting your second number...");
            numInput2 = GetNumInput();

            double cleanNum2 = 0;
            while (!double.TryParse(numInput2, out cleanNum2))
            {
                Console.Write("This is not valid input. Please enter a numeric value: ");
                numInput2 = Console.ReadLine();
            }

            string? op = GetOperand();
            // Validate input is not null, and matches the pattern
            while (op == null || op == "exit")
            {
                if (op == "exit")
                {
                    Console.WriteLine("You've chosen to close the app, press any key to close.");
                    Console.ReadKey();
                  return;
                }
                Console.WriteLine("Error: Unrecognized input. Try Again or type 'exit' to exit.");
                op = GetOperand();


            }
                try
                {
                    result = calculator.DoOperation(cleanNum1, cleanNum2, op);
                if (double.IsNaN(result))
                {
                    Console.WriteLine("This operation will result in a mathematical error.\n");
                }
                else
                {

                    Console.WriteLine("Your result: {0:0.##}\n", result);
                    count++;
                    Console.WriteLine($"You have used the calculator {count} times.");
                }
                }
                catch (Exception e)
                {
                    Console.WriteLine("Oh no! An exception occurred trying to do the math.\n - Details: " + e.Message);
                }
            Console.WriteLine("------------------------\n");

            // Wait for the user to respond before closing.
            Console.Write("Press 'n' and Enter to close the app, or press any other key and Enter to continue: ");
            if (Console.ReadLine() == "n") endApp = true;

            Console.WriteLine("\n"); // Friendly linespacing.
        }
        calculator.Finish();
        return;

        string GetOperand()
        {

            // Ask the user to choose an operator.
            Console.WriteLine("Choose an operator from the following list:");
            Console.WriteLine("\ta - Add");
            Console.WriteLine("\ts - Subtract");
            Console.WriteLine("\tm - Multiply");
            Console.WriteLine("\td - Divide");
            Console.Write("Your option? ");

            string? input = Console.ReadLine()?.Trim()?.ToLower() switch
            {
                "a" or "add" => "a",
                "s" or "subtract" => "s",
                "m" or "multiply" => "m",
                "d" or "divide" => "d",
                "exit" => "exit",
                _ => null
            };
            return input;
        }


        string GetNumInput()
        {
            if (calculator.History.Count > 0)
            {
                Console.WriteLine("Do you want to use a previous result as your number? Y/N");
                bool? decision;
                do
                {
                    decision = Console.ReadLine()?.ToLower()?.Trim() switch
                    {
                        "y" => true,
                        "n" => false,
                        _ => null
                    };
                } while (decision == null);

                if (decision == true)
                {
                    calculator.RecallMemory();
                    int choice;
                    while (true)
                    {
                        Console.Write($"Pick an entry (from 1 to {calculator.History.Count}) or 'c' to cancel: ");
                        string? input = Console.ReadLine()?.Trim().ToLower();

                        if (input == "c")
                        {
                            Console.WriteLine("Cancelled.");
                            Console.Write("Type a number, and then press Enter: ");
                            return Console.ReadLine();
                        }

                        if (int.TryParse(input, out choice) && choice >= 1 && choice <= calculator.History.Count)
                        {
                            return calculator.RecallResult(choice - 1).ToString();
                        }

                        Console.WriteLine("Invalid input, try again.");
                    }

                }
                else
                {
                    Console.Write("Type a number, and then press Enter: ");
                    return Console.ReadLine();
                }
            }
            else
            {
                Console.Write("Type a number, and then press Enter: ");
                return Console.ReadLine();
            }
        }

        bool ShowMenu()
        {
            while (true)
            {
                Console.Clear();
                Console.WriteLine("What would you like to do?");
                Console.WriteLine("1. Perform Calculation");
                Console.WriteLine("2. Display Number of calculations performed");
                Console.WriteLine("3. Display History");
                Console.WriteLine("4. Clear History");
                Console.WriteLine("5. Exit");
                Console.Write("Enter the # for your choice: ");

                switch (Console.ReadLine()?.Trim())
                {
                    case "1":
                        return false;
                    case "2":
                        Console.WriteLine($"Calculations performed: {count}");
                        break;
                    case "3":
                        if (calculator.History.Count > 0)
                            calculator.RecallMemory();
                        else
                            Console.WriteLine("No history to display.");
                        break;
                    case "4":
                        if (calculator.History.Count > 0)
                        {
                            calculator.History.Clear();
                            Console.WriteLine("History has been cleared.");
                        }
                        else
                        {
                            Console.WriteLine("No history to clear.");
                        }
                        break;
                    case "5":
                        return true;
                    default:
                        Console.WriteLine("Invalid choice, please try again.");
                        break;
                }

                Console.WriteLine("Press any key to continue...");
                Console.ReadKey();
            }
        }
    }
}