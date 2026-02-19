global using FluentValidation;
using DinnerSpinner.Api.Data;
using FastEndpoints;
using FastEndpoints.Swagger;
using Microsoft.AspNetCore.Builder;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
var builder = WebApplication.CreateBuilder(args);

builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseSqlite(builder.Configuration.GetConnectionString("DefaultConnection")));

builder.Services.AddFastEndpoints(); 
builder.Services.SwaggerDocument();

var app = builder.Build();
app.UseFastEndpoints();
app.UseSwaggerGen();
app.UseHttpsRedirection();
app.Run();

