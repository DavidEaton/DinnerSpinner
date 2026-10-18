using DinnerSpinner.Domain.Features.Categories;
using DinnerSpinner.Domain.Features.Common;

namespace DinnerSpinner.Tests.Domain;

public class CategoryTests
{
    [Fact]
    public void Create_retains_a_valid_name_and_starts_without_persisted_identity()
    {
        var name = Name.Create("Dinner").Value;

        var result = Category.Create(name);

        result.IsSuccess.ShouldBeTrue();
        result.Value.Name.ShouldBeSameAs(name);
        result.Value.Id.ShouldBe(0);
        result.Value.ToString().ShouldBe("Dinner");
    }

    [Fact]
    public void Create_rejects_a_missing_name()
    {
        var result = Category.Create(null!);

        result.IsFailure.ShouldBeTrue();
        result.Error.ShouldBe(Name.RequiredMessage);
    }

    [Fact]
    public void ChangeName_updates_the_name_and_returns_the_same_entity()
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
    public void ChangeName_rejects_null_without_changing_state()
    {
        var name = Name.Create("Dinner").Value;
        var category = Category.Create(name).Value;

        var result = category.ChangeName(null!);

        result.IsFailure.ShouldBeTrue();
        result.Error.ShouldBe(Name.RequiredMessage);
        category.Name.ShouldBeSameAs(name);
        category.ChangeName(Name.Create("Lunch").Value).IsSuccess.ShouldBeTrue();
    }

    [Fact]
    public void ChangeName_with_an_equal_value_is_a_noop()
    {
        var name = Name.Create("Dinner").Value;
        var category = Category.Create(name).Value;

        var result = category.ChangeName(Name.Create(" Dinner ").Value);

        result.IsSuccess.ShouldBeTrue();
        result.Value.ShouldBeSameAs(category);
        category.Name.ShouldBeSameAs(name);
    }
}
