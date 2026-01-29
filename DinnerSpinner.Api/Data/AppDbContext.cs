using DinnerSpinner.Domain.Features.Categories;
using DinnerSpinner.Domain.Features.Dishes;
using Microsoft.EntityFrameworkCore;

namespace DinnerSpinner.Api.Data;

public class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }
    public DbSet<Dish> Dishes => Set<Dish>();
    public DbSet<Category> Categories => Set<Category>();
}
