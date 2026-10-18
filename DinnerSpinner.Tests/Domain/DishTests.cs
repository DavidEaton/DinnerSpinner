using DinnerSpinner.Domain.Features.Categories;
using DinnerSpinner.Domain.Features.Common;
using DinnerSpinner.Domain.Features.Dishes;

namespace DinnerSpinner.Tests.Domain;

public class DishTests
{
    [Fact]
    public void Create_retains_valid_values_and_starts_without_persisted_identity()
    {
        var name = Name.Create("Soup").Value;
        var id = CategoryId.Create(1).Value;

        var result = Dish.Create(name, id);

        result.IsSuccess.ShouldBeTrue();
        result.Value.Name.ShouldBeSameAs(name);
        result.Value.CategoryId.ShouldBeSameAs(id);
        result.Value.Id.ShouldBe(0);
        result.Value.ToString().ShouldBe("Soup");
    }

    [Theory]
    [InlineData(true)]
    [InlineData(false)]
    public void Create_rejects_a_missing_name_before_validating_category(bool hasCategory)
    {
        var result = Dish.Create(null!, hasCategory ? CategoryId.Create(1).Value : null!);

        result.IsFailure.ShouldBeTrue();
        result.Error.ShouldBe(Name.RequiredMessage);
    }

    [Fact]
    public void Create_rejects_a_missing_category()
    {
        var result = Dish.Create(Name.Create("Soup").Value, null!);

        result.IsFailure.ShouldBeTrue();
        result.Error.ShouldBe("CategoryId must be a positive integer.");
    }

    [Fact]
    public void ChangeName_updates_only_the_name_and_returns_it()
    {
        var dish = CreateDish();
        var category = dish.CategoryId;
        var name = Name.Create("Salad").Value;

        var result = dish.ChangeName(name);

        result.IsSuccess.ShouldBeTrue();
        result.Value.ShouldBeSameAs(name);
        dish.Name.ShouldBeSameAs(name);
        dish.CategoryId.ShouldBeSameAs(category);
        dish.Id.ShouldBe(0);
        dish.ToString().ShouldBe("Salad");
    }

    [Fact]
    public void ChangeName_rejects_null_without_changing_state()
    {
        var dish = CreateDish();
        var name = dish.Name;
        var category = dish.CategoryId;

        var result = dish.ChangeName(null!);

        result.IsFailure.ShouldBeTrue();
        result.Error.ShouldBe(Name.RequiredMessage);
        dish.Name.ShouldBeSameAs(name);
        dish.CategoryId.ShouldBeSameAs(category);
        dish.ChangeName(Name.Create("Salad").Value).IsSuccess.ShouldBeTrue();
    }

    [Fact]
    public void ChangeName_with_an_equal_value_is_a_noop()
    {
        var dish = CreateDish();
        var name = dish.Name;

        var result = dish.ChangeName(Name.Create(" Soup ").Value);

        result.IsSuccess.ShouldBeTrue();
        result.Value.ShouldBeSameAs(name);
        dish.Name.ShouldBeSameAs(name);
    }

    [Theory]
    [InlineData(2)]
    [InlineData(int.MaxValue)]
    public void ChangeCategory_updates_only_the_category_and_returns_it(int value)
    {
        var dish = CreateDish();
        var name = dish.Name;
        var category = CategoryId.Create(value).Value;

        var result = dish.ChangeCategory(category);

        result.IsSuccess.ShouldBeTrue();
        result.Value.ShouldBeSameAs(category);
        dish.CategoryId.ShouldBeSameAs(category);
        dish.Name.ShouldBeSameAs(name);
        dish.Id.ShouldBe(0);
    }

    [Fact]
    public void ChangeCategory_with_an_equal_value_is_a_noop()
    {
        var dish = CreateDish();
        var category = dish.CategoryId;

        var result = dish.ChangeCategory(CategoryId.Create(1).Value);

        result.IsSuccess.ShouldBeTrue();
        result.Value.ShouldBeSameAs(category);
        dish.CategoryId.ShouldBeSameAs(category);
    }

    [Fact]
    public void ChangeCategory_rejects_null_without_changing_state()
    {
        var dish = CreateDish();
        var category = dish.CategoryId;
        var name = dish.Name;

        var result = dish.ChangeCategory(null!);

        result.IsFailure.ShouldBeTrue();
        result.Error.ShouldBe("CategoryId must be a positive integer.");
        dish.CategoryId.ShouldBeSameAs(category);
        dish.Name.ShouldBeSameAs(name);
        dish.ChangeCategory(CategoryId.Create(2).Value).IsSuccess.ShouldBeTrue();
    }

    private static Dish CreateDish() =>
        Dish.Create(Name.Create("Soup").Value, CategoryId.Create(1).Value).Value;
}
