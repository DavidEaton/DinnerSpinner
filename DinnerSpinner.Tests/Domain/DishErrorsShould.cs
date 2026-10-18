using DinnerSpinner.Domain.Features.Dishes;
using DinnerSpinner.Domain.Shared;

namespace DinnerSpinner.Tests.Domain;

public class DishErrorsShould
{
    [Fact]
    public void DescribeMissingValues_On_Empty()
    {
        Errors.Empty().ShouldBe(Error.Validation("Dish.Errors.Empty", "Name and Category are required."));
    }

    [Fact]
    public void DescribeMissingName_On_ValidNameRequired()
    {
        Errors.ValidNameRequired().ShouldBe(Error.Validation("Dish.Errors.ValidNameRequired", "Name is required."));
    }

    [Fact]
    public void DescribeMissingCategory_On_ValidCategoryRequired()
    {
        Errors.ValidCategoryRequired().ShouldBe(Error.Validation("Dish.Errors.ValidCategoryRequired", "Category is required."));
    }

    [Theory]
    [InlineData(1)]
    [InlineData(int.MaxValue)]
    public void IncludeIdentity_On_NotFound_WhenIdProvided(int id)
    {
        Errors.NotFound(id).ShouldBe(Error.NotFound("Dish.Errors.NotFound", $"Dish with Id '{id}' was not found"));
    }

    [Fact]
    public void IncludeNameAndCategory_On_Conflict_WhenProvided()
    {
        Errors.Conflict("Soup", "Lunch").ShouldBe(Error.Conflict(
            "Dish.Errors.Conflict", "Dish name 'Soup' in category 'Lunch' is taken."));
    }

    [Fact]
    public void IncludeBounds_On_ValidLengthName_WhenProvided()
    {
        Errors.ValidLengthName(2, 255).ShouldBe(Error.Validation(
            "Dish.Errors.ValidLengthName", "Name must be between 2 and 255 characters in length"));
    }
}
