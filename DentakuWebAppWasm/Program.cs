using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using Microsoft.JSInterop;
using DentakuWebAppWasm;

var builder = WebAssemblyHostBuilder.CreateDefault(args);
builder.RootComponents.Add<App>("#app");
builder.RootComponents.Add<HeadOutlet>("head::after");

builder.Services.AddScoped(sp => new HttpClient { BaseAddress = new Uri(builder.HostEnvironment.BaseAddress) });

// ★ Consoleアプリの依存関係注入
builder.Services.AddSingleton<ConsoleHost>();

var host = builder.Build();

// ★ WASM用 Console の初期化 → C# Main() をバックグラウンド実行
_ = Task.Run(async () =>
{
    var console = host.Services.GetRequiredService<ConsoleHost>();
    await console.StartAsync();
});

await host.RunAsync();
