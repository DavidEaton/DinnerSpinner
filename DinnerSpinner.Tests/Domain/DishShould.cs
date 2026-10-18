using DinnerSpinner.Domain.Features.Categories;
using DinnerSpinner.Domain.Features.Common;
using DinnerSpinner.Domain.Features.Dishes;

namespace DinnerSpinner.Tests.Domain;

public partial class DishShould
{
    [Fact]
    public void RetainValues_On_Create_WhenValid()
    {
        var name = Name.Create("Soup").Value;
        var category = CreateCategory();

        var result = Dish.Create(name, category);

        result.IsSuccess.ShouldBeTrue();
        result.Value.Name.ShouldBeSameAs(name);
        result.Value.Category.ShouldBeSameAs(category);
        result.Value.Id.ShouldBe(0);
        result.Value.ToString().ShouldBe("Soup");
    }

    [Theory]
    [InlineData(true)]
    [InlineData(false)]
    public void RejectMissingNameFirst_On_Create_WhenNull(bool hasCategory)
    {
        var result = Dish.Create(null!, hasCategory ? CreateCategory() : null!);

        result.IsFailure.ShouldBeTrue();
        result.Error.ShouldBe(Errors.ValidNameRequired());
    }

    [Fact]
    public void RejectMissingCategory_On_Create_WhenNull()
    {
        var result = Dish.Create(Name.Create("Soup").Value, null!);

        result.IsFailure.ShouldBeTrue();
        result.Error.ShouldBe(Errors.ValidCategoryRequired());
    }

    [Fact]
    public void ReturnSameEntity_On_ChangeName_WhenDifferent()
    {
        var dish = CreateDish();
        var category = dish.Category;
        var name = Name.Create("Salad").Value;

        var result = dish.ChangeName(name);

        result.IsSuccess.ShouldBeTrue();
        result.Value.ShouldBeSameAs(dish);
        dish.Name.ShouldBeSameAs(name);
        dish.Category.ShouldBeSameAs(category);
        dish.Id.ShouldBe(0);
        dish.ToString().ShouldBe("Salad");
    }

    [Fact]
    public void PreserveState_On_ChangeName_WhenNull()
    {
        var dish = CreateDish();
        var name = dish.Name;
        var category = dish.Category;

        var result = dish.ChangeName(null!);

        result.IsFailure.ShouldBeTrue();
        result.Error.ShouldBe(Errors.ValidNameRequired());
        dish.Name.ShouldBeSameAs(name);
        dish.Category.ShouldBeSameAs(category);
        dish.ChangeName(Name.Create("Salad").Value).IsSuccess.ShouldBeTrue();
    }

    [Fact]
    public void PreserveNameReference_On_ChangeName_WhenEqual()
    {
        var dish = CreateDish();
        var name = dish.Name;

        var result = dish.ChangeName(Name.Create(" Soup ").Value);

        result.IsSuccess.ShouldBeTrue();
        result.Value.ShouldBeSameAs(dish);
        dish.Name.ShouldBeSameAs(name);
    }

    [Fact]
    public void ReturnSameEntity_On_ChangeCategory_WhenDifferent()
    {
        var dish = CreateDish();
        var name = dish.Name;
        var category = CreateCategory("Lunch");

        var result = dish.ChangeCategory(category);

        result.IsSuccess.ShouldBeTrue();
        result.Value.ShouldBeSameAs(dish);
        dish.Category.ShouldBeSameAs(category);
        dish.Name.ShouldBeSameAs(name);
        dish.Id.ShouldBe(0);
    }

    [Fact]
    public void PreserveCategoryReference_On_ChangeCategory_WhenSameEntity()
    {
        var dish = CreateDish();
        var category = dish.Category;

        var result = dish.ChangeCategory(category);

        result.IsSuccess.ShouldBeTrue();
        result.Value.ShouldBeSameAs(dish);
        dish.Category.ShouldBeSameAs(category);
    }

    [Fact]
    public void PreserveState_On_ChangeCategory_WhenNull()
    {
        var dish = CreateDish();
        var category = dish.Category;
        var name = dish.Name;

        var result = dish.ChangeCategory(null!);

        result.IsFailure.ShouldBeTrue();
        result.Error.ShouldBe(Errors.ValidCategoryRequired());
        dish.Category.ShouldBeSameAs(category);
        dish.Name.ShouldBeSameAs(name);
        dish.ChangeCategory(CreateCategory("Lunch")).IsSuccess.ShouldBeTrue();
    }

    [Fact]
    public void ReplaceCategory_On_ChangeCategory_WhenTransientNamesMatch()
    {
        var dish = CreateDish();
        var differentCategory = CreateCategory();

        dish.ChangeCategory(differentCategory).IsSuccess.ShouldBeTrue();

        dish.Category.ShouldBeSameAs(differentCategory);
    }

    [Fact]
    public void PreserveCategoryReference_On_ChangeCategory_WhenIdentityMatches()
    {
        var category = new StoredCategory(1, "Dinner");
        var dish = Dish.Create(Name.Create("Soup").Value, category).Value;

        var result = dish.ChangeCategory(new StoredCategory(1, "Renamed"));

        result.IsSuccess.ShouldBeTrue();
        result.Value.ShouldBeSameAs(dish);
        dish.Category.ShouldBeSameAs(category);
    }

    private sealed class StoredCategory : Category
    {
        public StoredCategory(int id, string name)
        {
            Id = id;
            ChangeName(Name.Create(name).Value);
        }
    }

    private static Category CreateCategory(string name = "Dinner") =>
        Category.Create(Name.Create(name).Value).Value;

    private static Dish CreateDish() =>
        Dish.Create(Name.Create("Soup").Value, CreateCategory()).Value;
}
