using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using MudBlazor.Services;
using dotnet_blazor_website.Components;

var builder = WebAssemblyHostBuilder.CreateDefault(args);

builder.RootComponents.Add<App>("#app");
builder.RootComponents.Add<HeadOutlet>("head::after");

builder.Services.AddMudServices();
builder.Services.AddScoped(sp => new HttpClient { BaseAddress = new Uri(builder.HostEnvironment.BaseAddress) });

// Named client for the local GE backend API (run the Api project on http://localhost:5041)
builder.Services.AddHttpClient("GeApi", client =>
{
    client.BaseAddress = new Uri("http://localhost:5041/");
});

await builder.Build().RunAsync();
