using System.Net;
using System.Net.Http.Json;
using System.Text.Json;
using DinnerSpinner.Api.Data;
using DinnerSpinner.Domain.Features.Categories;
using DinnerSpinner.Domain.Features.Common;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;

namespace DinnerSpinner.Tests.Api;

public class CategoryShould : IAsyncLifetime
{
    private readonly SqliteConnection connection = new("Data Source=:memory:");
    private WebApplicationFactory<DinnerSpinner.Api.Features.Categories.Update.Endpoint> factory = null!;
    private HttpClient client = null!;
    private int dinnerId;

    public async Task InitializeAsync()
    {
        await connection.OpenAsync();
        factory = new WebApplicationFactory<DinnerSpinner.Api.Features.Categories.Update.Endpoint>()
            .WithWebHostBuilder(builder => builder.ConfigureServices(services =>
            {
                services.RemoveAll<DbContextOptions<AppDbContext>>();
                services.AddDbContext<AppDbContext>(options => options.UseSqlite(connection));
            }));
        client = factory.CreateClient(new WebApplicationFactoryClientOptions
        {
            BaseAddress = new Uri("https://localhost")
        });
        using var scope = factory.Services.CreateScope();
        var db = scope.ServiceProvider.GetRequiredService<AppDbContext>();
        await db.Database.EnsureCreatedAsync();
        var dinner = Category.Create(Name.Create("Dinner").Value).Value;
        db.Categories.AddRange(dinner, Category.Create(Name.Create("Lunch").Value).Value);
        await db.SaveChangesAsync();
        dinnerId = dinner.Id;
    }

    [Fact]
    public async Task ReturnNormalizedCategory_On_Create()
    {
        using var response = await client.PostAsJsonAsync("/api/categories", new { Name = "  Breakfast  " });
        response.StatusCode.ShouldBe(HttpStatusCode.Created);
        var envelope = await response.Content.ReadFromJsonAsync<JsonElement>();
        envelope.GetProperty("traceId").GetString().ShouldNotBeNullOrWhiteSpace();
        var category = envelope.GetProperty("data").GetProperty("category");
        var id = category.GetProperty("id").GetInt32();
        id.ShouldBeGreaterThan(0);
        category.GetProperty("name").GetString().ShouldBe("Breakfast");

        using var scope = factory.Services.CreateScope();
        var db = scope.ServiceProvider.GetRequiredService<AppDbContext>();
        (await db.Categories.SingleAsync(category => category.Id == id)).Name.Value.ShouldBe("Breakfast");
    }

    [Fact]
    public async Task ReturnCategoryCollection_On_List()
    {
        using var response = await client.GetAsync("/api/categories");
        response.StatusCode.ShouldBe(HttpStatusCode.OK);
        var envelope = await response.Content.ReadFromJsonAsync<JsonElement>();
        envelope.GetProperty("traceId").GetString().ShouldNotBeNullOrWhiteSpace();
        var categories = envelope.GetProperty("data").GetProperty("categories").EnumerateArray().ToArray();
        categories.Length.ShouldBe(2);
        var dinner = categories.Single(category => category.GetProperty("id").GetInt32() == dinnerId);
        dinner.GetProperty("name").GetString().ShouldBe("Dinner");
        categories.Select(category => category.GetProperty("name").GetString())
            .ShouldBe(new[] { "Dinner", "Lunch" }, ignoreOrder: true);
    }

    [Theory]
    [InlineData("  Supper  ", "Supper")]
    [InlineData("  Dinner  ", "Dinner")]
    public async Task ReturnNormalizedCategory_On_Update(string input, string expected)
    {
        using var response = await client.PutAsJsonAsync($"/api/categories/{dinnerId}", new { Id = int.MaxValue, Name = input });
        response.StatusCode.ShouldBe(HttpStatusCode.OK);
        var envelope = await response.Content.ReadFromJsonAsync<JsonElement>();
        envelope.GetProperty("traceId").GetString().ShouldNotBeNullOrWhiteSpace();
        var category = envelope.GetProperty("data").GetProperty("category");
        category.GetProperty("id").GetInt32().ShouldBe(dinnerId);
        category.GetProperty("name").GetString().ShouldBe(expected);
        using var scope = factory.Services.CreateScope();
        var db = scope.ServiceProvider.GetRequiredService<AppDbContext>();
        (await db.Categories.SingleAsync(category => category.Id == dinnerId)).Name.Value.ShouldBe(expected);
    }

    [Theory]
    [InlineData(null)]
    [InlineData("")]
    [InlineData("   ")]
    [InlineData("a")]
    public async Task ReturnValidationErrors_On_Update_WhenNameIsInvalid(string? name)
    {
        await AssertRejected(dinnerId, name, HttpStatusCode.BadRequest, "Validation");
    }

    [Fact]
    public async Task ReturnValidationErrors_On_Update_WhenNameIsTooLong()
    {
        await AssertRejected(dinnerId, new string('a', Name.MaximumLength + 1), HttpStatusCode.BadRequest, "Validation");
    }

    [Fact]
    public async Task ReturnNotFoundProblem_On_Update_WhenCategoryIsMissing()
    {
        await AssertRejected(int.MaxValue, "Supper", HttpStatusCode.NotFound, "NotFound");
    }

    [Fact]
    public async Task ReturnConflictProblem_On_Update_WhenNormalizedNameIsTaken()
    {
        await AssertRejected(dinnerId, "  Lunch  ", HttpStatusCode.Conflict, "Conflict");
    }

    private async Task AssertRejected(int id, string? name, HttpStatusCode status, string errorType)
    {
        using var response = await client.PutAsJsonAsync($"/api/categories/{id}", new { Name = name });
        response.StatusCode.ShouldBe(status);
        var payload = await response.Content.ReadFromJsonAsync<JsonElement>();
        if (status == HttpStatusCode.BadRequest)
        {
            payload.GetProperty("statusCode").GetInt32().ShouldBe(400);
            payload.GetProperty("errors").GetProperty("name")[0].GetString().ShouldNotBeNullOrWhiteSpace();
        }
        else
        {
            response.Content.Headers.ContentType!.MediaType.ShouldBe("application/problem+json");
            payload.GetProperty("status").GetInt32().ShouldBe((int)status);
            payload.GetProperty("detail").GetString().ShouldNotBeNullOrWhiteSpace();
            var error = payload.GetProperty("errors")[0];
            error.GetProperty("code").GetString()!.ShouldContain(errorType);
            error.GetProperty("description").GetString().ShouldBe(payload.GetProperty("detail").GetString());
        }
        using var scope = factory.Services.CreateScope();
        var db = scope.ServiceProvider.GetRequiredService<AppDbContext>();
        var names = await db.Categories.OrderBy(category => category.Id).Select(category => category.Name.Value).ToArrayAsync();
        names.ShouldBe(new[] { "Dinner", "Lunch" });
    }

    public async Task DisposeAsync()
    {
        client.Dispose();
        await factory.DisposeAsync();
        await connection.DisposeAsync();
    }
}
