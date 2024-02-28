using CarSnapScore.Services;
using CarSnapScore.Web;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;

WebAssemblyHostBuilder builder = WebAssemblyHostBuilder.CreateDefault(args);
builder.RootComponents.Add<App>("#app");
builder.RootComponents.Add<HeadOutlet>("head::after");

builder.Services.AddScoped(sp => new HttpClient { BaseAddress = new Uri(builder.HostEnvironment.BaseAddress) });

string fileNameUrl = builder.HostEnvironment.BaseAddress + "carNames.txt";
builder.Services.AddSingleton(new NameGenerator(fileNameUrl));
builder.Services.AddSingleton<CarDoesNotExist>();

await builder.Build().RunAsync();
