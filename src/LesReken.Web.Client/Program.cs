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

builder
    .Services.AddRefitClient<ISessionApi>()
    .ConfigureHttpClient(c => c.BaseAddress = new Uri(builder.HostEnvironment.BaseAddress));

builder
    .Services.AddRefitClient<IPaymentApi>()
    .ConfigureHttpClient(c => c.BaseAddress = new Uri(builder.HostEnvironment.BaseAddress));

// Enregistrer les implémentations WASM des services
builder.Services.AddScoped<IStudentService, WasmStudentService>();
builder.Services.AddScoped<ISessionService, WasmSessionService>();
builder.Services.AddScoped<IPaymentService, WasmPaymentService>();

await builder.Build().RunAsync();
