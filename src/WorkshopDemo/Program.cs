using Azure.Identity;
using Microsoft.ApplicationInsights.Extensibility;
using Serilog;
using Serilog.Events;
using Serilog.Sinks.SystemConsole.Themes;
using WorkshopDemo.Core.Common;
using WorkshopDemo.HealthChecks;

var builder = WebApplication.CreateBuilder(args);

builder.Configuration.AddAzureKeyVault(
    new Uri($"https://kv-raveller-{builder.Environment.EnvironmentName}.vault.azure.net/"),
    new DefaultAzureCredential());

Console.WriteLine($"My secret value is: {builder.Configuration.GetValue<string>("SomeSecret")}");

// Add services to the container.    
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddOpenApi();
builder.Services.AddControllers();
builder.Services.AddHealthChecks()
    .AddCheck<WorkshopDemoHealthCheck>(nameof(WorkshopDemoHealthCheck));
builder.Services.AddSingleton<IFileService, FileService>();
builder.Services.AddSingleton<IVersionService, VersionService>();

var app = builder.Build();
// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

app.UseHttpsRedirection();

app.MapHealthChecks("/api/healthz");

app.MapControllers();

app.Run();