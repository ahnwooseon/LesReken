using LesReken.Application.Interfaces;
using LesReken.Web.Client.Interfaces;
using LesReken.Web.Client.Services;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using Refit;

var builder = WebAssemblyHostBuilder.CreateDefault(args);

// Configurer Refit
builder
    .Services.AddRefitClient<IStudentApi>()
    .ConfigureHttpClient(c => c.BaseAddress = new Uri(builder.HostEnvironment.BaseAddress));

// Enregistrer l'implémentation WASM du service
builder.Services.AddScoped<IStudentService, WasmStudentService>();

await builder.Build().RunAsync();
