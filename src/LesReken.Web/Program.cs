using FastEndpoints;
using LesReken.Application.Interfaces;
using LesReken.Application.Services;
using LesReken.Infrastructure.Data;
using LesReken.Infrastructure.Repositories;
using LesReken.Web.Client;
using LesReken.Web.Components;
using Scalar.AspNetCore;

var builder = WebApplication.CreateBuilder(args);

// Add service defaults & Aspire client integrations.
builder.AddServiceDefaults();

// Add services to the container.
builder
    .Services.AddRazorComponents()
    .AddInteractiveServerComponents()
    .AddInteractiveWebAssemblyComponents();

builder.Services.AddSqlite<ApplicationDbContext>(
    builder.Configuration.GetConnectionString("DefaultConnection") ?? "Data Source=lesreken.db"
);

// Repositories & Services (Server Implementation)
builder.Services.AddScoped<IStudentRepository, StudentRepository>();
builder.Services.AddScoped<IStudentService, StudentService>();
builder.Services.AddScoped<ISessionRepository, SessionRepository>();
builder.Services.AddScoped<ISessionService, SessionService>();

builder.Services.AddOpenApi();
builder.Services.AddProblemDetails();
builder.Services.AddFastEndpoints();

builder.Services.AddOutputCache();

var app = builder.Build();

app.MapOpenApi();
app.MapScalarApiReference();

using (var scope = app.Services.CreateScope())
{
    var context = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
    context.Database.EnsureCreated();
}

if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error", createScopeForErrors: true);
    app.UseHsts();
}

app.UseStatusCodePages();
app.UseHttpsRedirection();
app.UseAntiforgery();
app.UseOutputCache();
app.MapStaticAssets();

app.MapRazorComponents<App>()
    .AddInteractiveServerRenderMode()
    .AddInteractiveWebAssemblyRenderMode()
    .AddAdditionalAssemblies(typeof(LesReken.Web.Client._Imports).Assembly);

app.UseFastEndpoints();

app.MapDefaultEndpoints();

app.Run();
