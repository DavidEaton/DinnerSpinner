global using FluentValidation;
using DinnerSpinner.Api.Data;
using FastEndpoints;
using Microsoft.EntityFrameworkCore;
var builder = WebApplication.CreateBuilder(args);

// Add services to the container.


builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseSqlite(builder.Configuration.GetConnectionString("DefaultConnection")));

// Configure the HTTP request pipeline.

builder.Services.AddFastEndpoints(); 

var app = builder.Build();
app.UseFastEndpoints();
app.UseHttpsRedirection();
app.Run();

