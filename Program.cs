// using System.Globalization;
// using System.Threading.Tasks;
// #if WASM
// using System.Runtime.InteropServices.JavaScript;
// #endif

// internal static partial class Program
// {
//     private static readonly CultureInfo Culture = CultureInfo.InvariantCulture;

// #if !WASM
//     private static async Task Main() => await RunAsync();
// #endif

//     internal static async Task RunAsync()
//     {
// #if !WASM
//         Console.OutputEncoding = System.Text.Encoding.UTF8;
// #endif
//         PrintLine("シンプル電卓へようこそ！\n");

//         while (true)
//         {
//             var operation = await PromptOperationAsync();
//             if (operation == Operation.Exit)
//             {
//                 PrintLine("ご利用ありがとうございました。");
//                 break;
//             }

//             var left = await PromptNumberAsync("1つ目の数値を入力してください: ");
//             var right = await PromptNumberAsync("2つ目の数値を入力してください: ");

//             try
//             {
//                 var result = Calculate(operation, left, right);
//                 PrintLine($"結果: {FormatNumber(left)} {GetSymbol(operation)} {FormatNumber(right)} = {FormatNumber(result)}\n");
//             }
//             catch (DivideByZeroException)
//             {
//                 PrintLine("0 で割ることはできません。別の数値を入力してください。\n");
//             }
//         }
//     }

//     private static async Task<Operation> PromptOperationAsync()
//     {
//         while (true)
//         {
//             PrintLine("操作を選んでください:");
//             PrintLine("1. 足し算");
//             PrintLine("2. 引き算");
//             PrintLine("3. かけ算");
//             PrintLine("4. 割り算");
//             PrintLine("5. 終了");
//             Print("番号: ");

//             var input = await ReadLineAsync();
//             if (Enum.TryParse(input, out Operation operation) && Enum.IsDefined(operation))
//             {
//                 return operation;
//             }

//             PrintLine("1〜5 の番号を入力してください。\n");
//         }
//     }

//     private static async Task<double> PromptNumberAsync(string message)
//     {
//         while (true)
//         {
//             Print(message);
//             var input = await ReadLineAsync();
//             if (double.TryParse(input, NumberStyles.Float, Culture, out var value))
//             {
//                 return value;
//             }

//             PrintLine("数値を入力してください。例: 3.14 または -2\n");
//         }
//     }

//     private static double Calculate(Operation operation, double left, double right)
//     {
//         return operation switch
//         {
//             Operation.Add => Calculator.Add(left, right),
//             Operation.Subtract => Calculator.Subtract(left, right),
//             Operation.Multiply => Calculator.Multiply(left, right),
//             Operation.Divide => Calculator.Divide(left, right),
//             _ => throw new ArgumentOutOfRangeException(nameof(operation), operation, "無効な操作です。")
//         };
//     }

//     private static string FormatNumber(double value) => value.ToString("G", Culture);

//     private static string GetSymbol(Operation operation) => operation switch
//     {
//         Operation.Add => "+",
//         Operation.Subtract => "-",
//         Operation.Multiply => "×",
//         Operation.Divide => "÷",
//         _ => string.Empty
//     };

//     private static void Print(string message)
//     {
// #if WASM
//         JSWrite(message);
// #else
//         Console.Write(message);
// #endif
//     }

//     private static void PrintLine(string message)
//     {
// #if WASM
//         JSWriteLine(message);
// #else
//         Console.WriteLine(message);
// #endif
//     }

//     private static Task<string> ReadLineAsync()
//     {
// #if WASM
//         return JSReadLineAsync();
// #else
//         return Task.FromResult(Console.ReadLine() ?? string.Empty);
// #endif
//     }

// #if WASM
//     [JSImport("console.write", "app")]
//     private static partial void JSWrite(string message);

//     [JSImport("console.writeLine", "app")]
//     private static partial void JSWriteLine(string message);

//     [JSImport("console.readLine", "app")]
//     private static partial Task<string> JSReadLineAsync();
// #endif

//     private enum Operation
//     {
//         Add = 1,
//         Subtract = 2,
//         Multiply = 3,
//         Divide = 4,
//         Exit = 5
//     }
// }

// internal static class Calculator
// {
//     public static double Add(double left, double right) => left + right;

//     public static double Subtract(double left, double right) => left - right;

//     public static double Multiply(double left, double right) => left * right;

//     public static double Divide(double left, double right) => right != 0
//         ? left / right
//         : throw new DivideByZeroException("Division by zero is not allowed.");
// }
