using System.Net;
using System.Net.Http.Json;
using System.Text.Json;
using DinnerSpinner.Api.Data;
using DinnerSpinner.Domain.Features.Categories;
using DinnerSpinner.Domain.Features.Common;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;

namespace DinnerSpinner.Tests.Api;

public class DishShould
{
    [Fact]
    public async Task ReturnCategoryDetails_On_CreateReadAndUpdate()
    {
        await using var connection = new SqliteConnection("Data Source=:memory:");
        await connection.OpenAsync();
        await using var factory = new WebApplicationFactory<DinnerSpinner.Api.Features.Dishes.Create.Endpoint>()
            .WithWebHostBuilder(builder => builder.ConfigureServices(services =>
            {
                services.RemoveAll<DbContextOptions<AppDbContext>>();
                services.AddDbContext<AppDbContext>(options => options.UseSqlite(connection));
            }));
        using var client = factory.CreateClient(new WebApplicationFactoryClientOptions
        {
            BaseAddress = new Uri("https://localhost")
        });
        int dinnerId;
        int lunchId;
        using (var scope = factory.Services.CreateScope())
        {
            var db = scope.ServiceProvider.GetRequiredService<AppDbContext>();
            await db.Database.EnsureCreatedAsync();
            var dinner = Category.Create(Name.Create("Dinner").Value).Value;
            var lunch = Category.Create(Name.Create("Lunch").Value).Value;
            db.Categories.AddRange(dinner, lunch);
            await db.SaveChangesAsync();
            dinnerId = dinner.Id;
            lunchId = lunch.Id;
        }

        var create = await client.PostAsJsonAsync("/api/dishes", new { Name = "Soup", CategoryId = dinnerId });
        create.StatusCode.ShouldBe(HttpStatusCode.Created);
        var created = await ReadDish(create);
        var id = created.GetProperty("id").GetInt32();
        AssertDish(created, id, "Soup", dinnerId, "Dinner");

        var update = await client.PutAsJsonAsync($"/api/dishes/{id}", new
        {
            Dish = new { Id = id, Name = "Salad", CategoryId = lunchId, CategoryName = "Ignored client value" }
        });
        update.StatusCode.ShouldBe(HttpStatusCode.OK);
        AssertDish(await ReadDish(update), id, "Salad", lunchId, "Lunch");

        var get = await client.GetAsync($"/api/dishes/{id}");
        get.StatusCode.ShouldBe(HttpStatusCode.OK);
        AssertDish(await ReadDish(get), id, "Salad", lunchId, "Lunch");

        var list = await client.GetFromJsonAsync<JsonElement>("/api/dishes");
        var listed = list.GetProperty("data").GetProperty("dishes").EnumerateArray().Single();
        AssertDish(listed, id, "Salad", lunchId, "Lunch");

        var missing = await client.PostAsJsonAsync("/api/dishes", new { Name = "Soup", CategoryId = int.MaxValue });
        missing.IsSuccessStatusCode.ShouldBeFalse();
        var duplicate = await client.PostAsJsonAsync("/api/dishes", new { Name = "Salad", CategoryId = lunchId });
        duplicate.IsSuccessStatusCode.ShouldBeFalse();
        using var verification = factory.Services.CreateScope();
        (await verification.ServiceProvider.GetRequiredService<AppDbContext>().Dishes.CountAsync()).ShouldBe(1);
    }

    private static async Task<JsonElement> ReadDish(HttpResponseMessage response)
    {
        var envelope = await response.Content.ReadFromJsonAsync<JsonElement>();
        return envelope.GetProperty("data").GetProperty("dish");
    }

    private static void AssertDish(JsonElement dish, int id, string name, int categoryId, string categoryName)
    {
        dish.GetProperty("id").GetInt32().ShouldBe(id);
        dish.GetProperty("name").GetString().ShouldBe(name);
        dish.GetProperty("categoryId").GetInt32().ShouldBe(categoryId);
        dish.GetProperty("categoryName").GetString().ShouldBe(categoryName);
    }
}
