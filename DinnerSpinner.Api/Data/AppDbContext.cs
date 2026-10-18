using DinnerSpinner.Domain.Abstractions;
using DinnerSpinner.Domain.Features.Categories;
using DinnerSpinner.Domain.Features.Dishes;
using Microsoft.EntityFrameworkCore;

namespace DinnerSpinner.Api.Data;

public class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);
        modelBuilder.Entity<Entity>().HasKey(entity => entity.Id);
        modelBuilder.Entity<Entity>().Property(entity => entity.Id).ValueGeneratedOnAdd();
        modelBuilder.Ignore<Entity>();

        modelBuilder.ApplyConfiguration(new Features.Categories.Configuration());
        modelBuilder.ApplyConfiguration(new Features.Dishes.Configuration());

        // modelBuilder.ApplyConfigurationsFromAssembly(typeof(AppDbContext).Assembly);
    }

    public DbSet<Dish> Dishes => Set<Dish>();
    public DbSet<Category> Categories => Set<Category>();
}
