global using FluentValidation;
using DinnerSpinner.Api.Data;
using FastEndpoints;
using FastEndpoints.Swagger;
using Microsoft.AspNetCore.Builder;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
var builder = WebApplication.CreateBuilder(args);

builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseSqlite(builder.Configuration.GetConnectionString("DefaultConnection")));

builder.Services.AddFastEndpoints();
builder.Services.AddHealthChecks();
builder.Services.SwaggerDocument(options =>
{
    options.DocumentSettings = settings =>
    {
        settings.Title = "DinnerSpinner API";
        settings.Version = "v1";
        settings.Description = "DinnerSpinner Web API (FastEndpoints)";
    };
    options.EnableJWTBearerAuth = false;
});

// builder.Services.AddEndpointsApiExplorer();
var app = builder.Build();
app.UseHttpsRedirection();
app.UseHealthChecks("/health");
app.UseFastEndpoints();

if (app.Environment.IsDevelopment())
{
    // Define endpoints
    app.MapGet("/", () => "Hello from the DinnerSpinner api!");
    
    app.UseSwaggerGen(
        openApiDocumentSettings =>
        {
            openApiDocumentSettings.Path = "/swagger/{documentName}/swagger.json";
        },
        uiSettings =>
        {
            uiSettings.Path = "/swagger";
            uiSettings.DocumentPath = "/swagger/{documentName}/swagger.json";
        });
}

app.Run();

