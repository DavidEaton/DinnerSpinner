global using FluentValidation;
using DinnerSpinner.Api.Data;
using FastEndpoints;
using Microsoft.EntityFrameworkCore;
var builder = WebApplication.CreateBuilder(args);

builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseSqlite(builder.Configuration.GetConnectionString("DefaultConnection")));

builder.Services.AddFastEndpoints(); 

var app = builder.Build();
app.UseFastEndpoints();
app.UseHttpsRedirection();
app.Run();

