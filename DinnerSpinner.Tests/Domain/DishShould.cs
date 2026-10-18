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
    public void RejectMissingNameFirst_On_Create_WhenNull(bool hasCategory)
    {
        var result = Dish.Create(null!, hasCategory ? CategoryId.Create(1).Value : null!);

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
        var category = dish.CategoryId;
        var name = Name.Create("Salad").Value;

        var result = dish.ChangeName(name);

        result.IsSuccess.ShouldBeTrue();
        result.Value.ShouldBeSameAs(dish);
        dish.Name.ShouldBeSameAs(name);
        dish.CategoryId.ShouldBeSameAs(category);
        dish.Id.ShouldBe(0);
        dish.ToString().ShouldBe("Salad");
    }

    [Fact]
    public void PreserveState_On_ChangeName_WhenNull()
    {
        var dish = CreateDish();
        var name = dish.Name;
        var category = dish.CategoryId;

        var result = dish.ChangeName(null!);

        result.IsFailure.ShouldBeTrue();
        result.Error.ShouldBe(Errors.ValidNameRequired());
        dish.Name.ShouldBeSameAs(name);
        dish.CategoryId.ShouldBeSameAs(category);
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

    [Theory]
    [InlineData(2)]
    [InlineData(int.MaxValue)]
    public void ReturnSameEntity_On_ChangeCategory_WhenDifferent(int value)
    {
        var dish = CreateDish();
        var name = dish.Name;
        var category = CategoryId.Create(value).Value;

        var result = dish.ChangeCategory(category);

        result.IsSuccess.ShouldBeTrue();
        result.Value.ShouldBeSameAs(dish);
        dish.CategoryId.ShouldBeSameAs(category);
        dish.Name.ShouldBeSameAs(name);
        dish.Id.ShouldBe(0);
    }

    [Fact]
    public void PreserveCategoryReference_On_ChangeCategory_WhenEqual()
    {
        var dish = CreateDish();
        var category = dish.CategoryId;

        var result = dish.ChangeCategory(CategoryId.Create(1).Value);

        result.IsSuccess.ShouldBeTrue();
        result.Value.ShouldBeSameAs(dish);
        dish.CategoryId.ShouldBeSameAs(category);
    }

    [Fact]
    public void PreserveState_On_ChangeCategory_WhenNull()
    {
        var dish = CreateDish();
        var category = dish.CategoryId;
        var name = dish.Name;

        var result = dish.ChangeCategory(null!);

        result.IsFailure.ShouldBeTrue();
        result.Error.ShouldBe(Errors.ValidCategoryRequired());
        dish.CategoryId.ShouldBeSameAs(category);
        dish.Name.ShouldBeSameAs(name);
        dish.ChangeCategory(CategoryId.Create(2).Value).IsSuccess.ShouldBeTrue();
    }

    private static Dish CreateDish() =>
        Dish.Create(Name.Create("Soup").Value, CategoryId.Create(1).Value).Value;
}
