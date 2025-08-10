using DinnerSpinner.Domain;
using Microsoft.EntityFrameworkCore;

namespace DinnerSpinner.Data
{
    public class AppDbContext : DbContext
    {   public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {
        }
        public DbSet<Category> Categories { get; set; }
        public DbSet<Dish> Dishes { get; set; }
    }
}
