using System.Text.RegularExpressions;
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

            // Ask the user to type the first number.
            if (count > 0)
            {
                Console.WriteLine("Do you want to use a previous result as your first number? Y/N");
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
                    do
                    {
                        Console.Write($"Enter a number from 1 to {calculator.History.Count}: ");
                    } while (!int.TryParse(Console.ReadLine(), out choice) || choice < 1 || choice > calculator.History.Count);

                    numInput1 = calculator.RecallResult(choice - 1).ToString();
                }
                else
                {
                    Console.Write("Type a number, and then press Enter: ");
                    numInput1 = Console.ReadLine();
                }
            }
            else 
            {
                Console.Write("Type a number, and then press Enter: ");
                numInput1 = Console.ReadLine();
            }

            double cleanNum1 = 0;
            while (!double.TryParse(numInput1, out cleanNum1))
            {
                Console.Write("This is not valid input. Please enter a numeric value: ");
                numInput1 = Console.ReadLine();
            }

            // Ask the user to type the second number.
            Console.Write("Type another number, and then press Enter: ");
            numInput2 = Console.ReadLine();

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
    }
}