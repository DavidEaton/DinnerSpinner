using DinnerSpinner.Domain.Features.Dishes;
using DinnerSpinner.Domain.Features.Categories;

namespace DinnerSpinner.Tests.Domain;

public class CategoryIdShould
{
    [Theory]
    [InlineData(int.MinValue)]
    [InlineData(-1)]
    [InlineData(0)]
    public void RejectInvalidCategory_On_Create_WhenValueIsNonpositive(int value)
    {
        var result = CategoryId.Create(value);

        result.IsFailure.ShouldBeTrue();
        result.Error.ShouldBe(Errors.ValidCategoryRequired());
    }

    [Theory]
    [InlineData(1)]
    [InlineData(2)]
    [InlineData(int.MaxValue)]
    public void PreserveValue_On_Create_WhenPositive(int value)
    {
        var result = CategoryId.Create(value);

        result.IsSuccess.ShouldBeTrue();
        result.Value.Value.ShouldBe(value);
        result.Value.ToString().ShouldBe(value.ToString());
    }

    [Fact]
    public void CompareIntegerValues_On_Equals()
    {
        var id = CategoryId.Create(1).Value;
        var equal = CategoryId.Create(1).Value;
        var different = CategoryId.Create(2).Value;

        (id == equal).ShouldBeTrue();
        id.Equals(equal).ShouldBeTrue();
        id.GetHashCode().ShouldBe(equal.GetHashCode());
        (id != different).ShouldBeTrue();
        id.Equals(null).ShouldBeFalse();
        id.Equals((object)1).ShouldBeFalse();
        var copy = id with { };
        copy.ShouldBe(id);
        copy.Value.ShouldBe(1);
    }
}
