using System.Runtime.InteropServices.JavaScript;
using System.Threading;
using Microsoft.JSInterop;

namespace DentakuWebAppWasm;

internal sealed partial class ConsoleHost
{
    private const string DefaultConsoleElementId = "console-output";

    private readonly IJSRuntime _jsRuntime;
    private readonly SemaphoreSlim _startLock = new(1, 1);
    private Task? _runner;

    public ConsoleHost(IJSRuntime jsRuntime)
    {
        _jsRuntime = jsRuntime;
    }

    public async Task StartAsync(string? consoleElementId = null)
    {
        await _startLock.WaitAsync();
        try
        {
            if (_runner is not null)
            {
                return;
            }

            var targetElementId = string.IsNullOrWhiteSpace(consoleElementId)
                ? DefaultConsoleElementId
                : consoleElementId;

            // Load the JavaScript module for [JSImport] before running the app.
            await _jsRuntime.InvokeVoidAsync("import", "./app.js");
            await JSHost.ImportAsync("app", "./app.js");
            await InitializeConsoleAsync(targetElementId);

            _runner = Task.Run(async () =>
            {
                try
                {
                    await Program.RunAsync();
                }
                catch (Exception ex)
                {
                    await WriteLineAsync($"エラーが発生しました: {ex.Message}");
                }
            });
        }
        finally
        {
            _startLock.Release();
        }
    }

    [JSImport("console.initConsole", "app")]
    private static partial Task InitializeConsoleAsync(string elementId);

    [JSImport("console.writeLine", "app")]
    private static partial Task WriteLineAsync(string message);
}
