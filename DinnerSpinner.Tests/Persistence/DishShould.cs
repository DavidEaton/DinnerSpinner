using DinnerSpinner.Api.Data;
using DinnerSpinner.Domain.Features.Categories;
using DinnerSpinner.Domain.Features.Common;
using DinnerSpinner.Domain.Features.Dishes;
using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;

namespace DinnerSpinner.Tests.Persistence;

public class DishShould
{
    [Fact]
    public async Task LoadCategory_On_Query_AfterSaveAndChange()
    {
        await using var connection = new SqliteConnection("Data Source=:memory:");
        await connection.OpenAsync();
        var options = new DbContextOptionsBuilder<AppDbContext>().UseSqlite(connection).Options;
        int dishId;
        int dinnerId;
        await using (var db = new AppDbContext(options))
        {
            await db.Database.EnsureCreatedAsync();
            var dinner = Category.Create(Name.Create("Dinner").Value).Value;
            db.Categories.Add(dinner);
            await db.SaveChangesAsync();
            var dish = Dish.Create(Name.Create("Soup").Value, dinner).Value;
            db.Dishes.Add(dish);
            await db.SaveChangesAsync();
            dishId = dish.Id;
            dinnerId = dinner.Id;
        }

        await using (var db = new AppDbContext(options))
        {
            var dish = await db.Dishes.SingleAsync(dish => dish.Id == dishId);
            dish.Category.Id.ShouldBe(dinnerId);
            dish.Category.Name.Value.ShouldBe("Dinner");
            var lunch = Category.Create(Name.Create("Lunch").Value).Value;
            db.Categories.Add(lunch);
            dish.ChangeCategory(lunch).IsSuccess.ShouldBeTrue();
            await db.SaveChangesAsync();
        }

        await using (var db = new AppDbContext(options))
        {
            var dish = await db.Dishes.AsNoTracking().SingleAsync();
            dish.Id.ShouldBe(dishId);
            dish.Name.Value.ShouldBe("Soup");
            dish.Category.Id.ShouldNotBe(dinnerId);
            dish.Category.Name.Value.ShouldBe("Lunch");
            (await db.Categories.CountAsync()).ShouldBe(2);
            // Exercise the relationship predicates and projections used by endpoints.
            (await db.Dishes.AnyAsync(d => d.Name.Value == "Soup" && d.Category.Id == dish.Category.Id))
                .ShouldBeTrue();
            var row = await db.Dishes.AsNoTracking()
                .Select(d => new { d.Id, CategoryId = d.Category.Id, CategoryName = d.Category.Name.Value })
                .SingleAsync();
            row.CategoryId.ShouldBe(dish.Category.Id);
            row.CategoryName.ShouldBe("Lunch");
        }
    }

    [Fact]
    public async Task RejectMissingCategory_On_Save_WhenForeignKeyIsInvalid()
    {
        await using var connection = new SqliteConnection("Data Source=:memory:;Foreign Keys=True");
        await connection.OpenAsync();
        var options = new DbContextOptionsBuilder<AppDbContext>().UseSqlite(connection).Options;
        await using var db = new AppDbContext(options);
        await db.Database.EnsureCreatedAsync();
        await Should.ThrowAsync<SqliteException>(async () =>
            await db.Database.ExecuteSqlRawAsync(
                "INSERT INTO Dish (Name, CategoryId) VALUES ('Soup', 999)"));
    }
}
