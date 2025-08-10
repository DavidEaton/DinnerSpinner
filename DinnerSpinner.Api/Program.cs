using DinnerSpinner.Api.Data;
using Microsoft.EntityFrameworkCore;
var builder = WebApplication.CreateBuilder(args);

// Add services to the container.


builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseSqlite(builder.Configuration.GetConnectionString("DefaultConnection")));
// Configure the HTTP request pipeline.

var app = builder.Build();
app.UseHttpsRedirection();
app.Run();

