using DinnerSpinner.Domain.Features.Dishes;
using DinnerSpinner.Domain.Features.Categories;
using DinnerSpinner.Domain.Features.Common;

namespace DinnerSpinner.Tests.Domain;

public partial class CategoryShould
{
    [Fact]
    public void RetainName_On_Create_WhenValid()
    {
        var name = Name.Create("Dinner").Value;

        var result = Category.Create(name);

        result.IsSuccess.ShouldBeTrue();
        result.Value.Name.ShouldBeSameAs(name);
        result.Value.Id.ShouldBe(0);
        result.Value.ToString().ShouldBe("Dinner");
    }

    [Fact]
    public void RejectMissingName_On_Create_WhenNull()
    {
        var result = Category.Create(null!);

        result.IsFailure.ShouldBeTrue();
        result.Error.ShouldBe(Errors.ValidCategoryRequired());
    }

    [Fact]
    public void ReturnSameEntity_On_ChangeName_WhenDifferent()
    {
        var category = Category.Create(Name.Create("Dinner").Value).Value;
        var replacement = Name.Create("Lunch").Value;

        var result = category.ChangeName(replacement);

        result.IsSuccess.ShouldBeTrue();
        result.Value.ShouldBeSameAs(category);
        category.Name.ShouldBeSameAs(replacement);
        category.ToString().ShouldBe("Lunch");
        category.Id.ShouldBe(0);
    }

    [Fact]
    public void PreserveState_On_ChangeName_WhenNull()
    {
        var name = Name.Create("Dinner").Value;
        var category = Category.Create(name).Value;

        var result = category.ChangeName(null!);

        result.IsFailure.ShouldBeTrue();
        result.Error.ShouldBe(Errors.ValidCategoryRequired());
        category.Name.ShouldBeSameAs(name);
        category.ChangeName(Name.Create("Lunch").Value).IsSuccess.ShouldBeTrue();
    }

    [Fact]
    public void PreserveNameReference_On_ChangeName_WhenEqual()
    {
        var name = Name.Create("Dinner").Value;
        var category = Category.Create(name).Value;

        var result = category.ChangeName(Name.Create(" Dinner ").Value);

        result.IsSuccess.ShouldBeTrue();
        result.Value.ShouldBeSameAs(category);
        category.Name.ShouldBeSameAs(name);
    }
}
