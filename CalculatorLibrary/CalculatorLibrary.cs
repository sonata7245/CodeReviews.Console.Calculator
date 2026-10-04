using System.Diagnostics;
using Newtonsoft.Json;
namespace CalculatorLibrary
{
    public class Calculator
    {
        JsonWriter writer;
        StreamWriter logFile;

        public record Calculation(double Num1, double Num2, string Op, double Result)
        {
            public string Symbol => Op switch
            {
                "a" => "+",
                "s" => "-",
                "m" => "*",
                "d" => "/",
                _ => Op
            };
        }
        public List<Calculation> History { get; private set; } = new(); 
        public Calculator() 
        {
            StartLog();
        }
        public double DoOperation(double num1, double num2, string op)
        {
            double result = double.NaN; // Default value is "not-a-number" if an operation, such as division, could result in an error.
            writer.WriteStartObject();
            writer.WritePropertyName("Operand1");
            writer.WriteValue(num1);
            writer.WritePropertyName("Operand2");
            writer.WriteValue(num2);
            writer.WritePropertyName("Operation");
            // Use a switch statement to do the math.
            switch (op)
            {
                case "a":
                    result = num1 + num2;
                    writer.WriteValue("Add");
                    break;
                case "s":
                    result = num1 - num2;
                    writer.WriteValue("Subtract");
                    break;
                case "m":
                    result = num1 * num2;
                    writer.WriteValue("Multiply");
                    break;
                case "d":
                    // Ask the user to enter a non-zero divisor.
                    if (num2 != 0)
                    {
                        result = num1 / num2;
                        writer.WriteValue("Divide");
                    }
                    break;
                // Return text for an incorrect option entry.
                default:
                    break;
            }
            writer.WritePropertyName("Result");
            writer.WriteValue(result);
            writer.WriteEndObject();
            History.Add(new Calculation(num1, num2, op, result));

            return result;
        }
        public void Finish()
        {
            writer.WriteEndArray();
            writer.WriteEndObject();
            writer.Close();
        }

        public void ClearMemory()
        {
            History.Clear();
        }

        public void RecallMemory()
        {
            for (int i = 0; i < History.Count; i++)
            {
                Calculation calc = History[i];
                Console.WriteLine($"{i + 1}. {calc.Num1} {calc.Symbol} {calc.Num2} = {calc.Result}");
            }
        }

        public double RecallResult(int resultNum)
        {
            return History[resultNum - 1].Result;
        }
        private void StartLog()
        {
            logFile = File.CreateText("calculatorlog.json");
            logFile.AutoFlush = true;
            writer = new JsonTextWriter(logFile);
            writer.Formatting = Formatting.Indented;
            writer.WriteStartObject();
            writer.WritePropertyName("Operations");
            writer.WriteStartArray();
        }
    }
}
