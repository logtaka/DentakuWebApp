using System.Globalization;

internal static class Program
{
    private static readonly CultureInfo Culture = CultureInfo.InvariantCulture;

    private static void Main()
    {
        Console.OutputEncoding = System.Text.Encoding.UTF8;
        Console.WriteLine("シンプル電卓へようこそ！\n");

        while (true)
        {
            var operation = PromptOperation();
            if (operation == Operation.Exit)
            {
                Console.WriteLine("ご利用ありがとうございました。");
                break;
            }

            var left = PromptNumber("1つ目の数値を入力してください: ");
            var right = PromptNumber("2つ目の数値を入力してください: ");

            try
            {
                var result = Calculate(operation, left, right);
                Console.WriteLine($"結果: {FormatNumber(left)} {GetSymbol(operation)} {FormatNumber(right)} = {FormatNumber(result)}\n");
            }
            catch (DivideByZeroException)
            {
                Console.WriteLine("0 で割ることはできません。別の数値を入力してください。\n");
            }
        }
    }

    private static Operation PromptOperation()
    {
        while (true)
        {
            Console.WriteLine("操作を選んでください:");
            Console.WriteLine("1. 足し算");
            Console.WriteLine("2. 引き算");
            Console.WriteLine("3. かけ算");
            Console.WriteLine("4. 割り算");
            Console.WriteLine("5. 終了");
            Console.Write("番号: ");

            var input = Console.ReadLine();
            if (Enum.TryParse(input, out Operation operation) && Enum.IsDefined(operation))
            {
                return operation;
            }

            Console.WriteLine("1〜5 の番号を入力してください。\n");
        }
    }

    private static double PromptNumber(string message)
    {
        while (true)
        {
            Console.Write(message);
            var input = Console.ReadLine();
            if (double.TryParse(input, NumberStyles.Float, Culture, out var value))
            {
                return value;
            }

            Console.WriteLine("数値を入力してください。例: 3.14 または -2\n");
        }
    }

    private static double Calculate(Operation operation, double left, double right)
    {
        return operation switch
        {
            Operation.Add => Calculator.Add(left, right),
            Operation.Subtract => Calculator.Subtract(left, right),
            Operation.Multiply => Calculator.Multiply(left, right),
            Operation.Divide => Calculator.Divide(left, right),
            _ => throw new ArgumentOutOfRangeException(nameof(operation), operation, "無効な操作です。")
        };
    }

    private static string FormatNumber(double value) => value.ToString("G", Culture);

    private static string GetSymbol(Operation operation) => operation switch
    {
        Operation.Add => "+",
        Operation.Subtract => "-",
        Operation.Multiply => "×",
        Operation.Divide => "÷",
        _ => string.Empty
    };

    private enum Operation
    {
        Add = 1,
        Subtract = 2,
        Multiply = 3,
        Divide = 4,
        Exit = 5
    }
}

internal static class Calculator
{
    public static double Add(double left, double right) => left + right;

    public static double Subtract(double left, double right) => left - right;

    public static double Multiply(double left, double right) => left * right;

    public static double Divide(double left, double right) => right != 0
        ? left / right
        : throw new DivideByZeroException("Division by zero is not allowed.");
}
