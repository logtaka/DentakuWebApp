using System.Runtime.InteropServices.JavaScript;
using System.Threading;

namespace DentakuWebAppWasm;

internal sealed partial class ConsoleHost
{
    private const string DefaultConsoleElementId = "console-output";

    private readonly SemaphoreSlim _startLock = new(1, 1);
    private Task? _runner;

    public async Task StartAsync(string? consoleElementId = null)
    {
        await _startLock.WaitAsync();
        try
        {
            if (_runner is not null)
                return;

            await JSHost.ImportAsync("app", "./app.js");

            var targetElementId = string.IsNullOrWhiteSpace(consoleElementId)
                ? DefaultConsoleElementId
                : consoleElementId;

            await InitializeConsoleAsync(targetElementId);

            _runner = Task.Run(async () =>
            {
                try
                {
                  await ConsoleProgram.RunAsync();
                }
                catch (Exception ex)
                {
                    await WriteLineAsync($"エラー: {ex.Message}");
                }
            });
        }
        finally
        {
            _startLock.Release();
        }
    }

    [JSImport("initConsole", "app")]
    internal static partial Task InitializeConsoleAsync(string elementId);

    [JSImport("writeLine", "app")]
    internal static partial Task WriteLineAsync(string message);
}
