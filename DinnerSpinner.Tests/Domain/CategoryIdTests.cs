using DinnerSpinner.Domain.Features.Categories;

namespace DinnerSpinner.Tests.Domain;

public class CategoryIdTests
{
    [Theory]
    [InlineData(int.MinValue)]
    [InlineData(-1)]
    [InlineData(0)]
    public void Create_rejects_nonpositive_values(int value)
    {
        var result = CategoryId.Create(value);

        result.IsFailure.ShouldBeTrue();
        result.Error.ShouldBe("CategoryId must be a positive integer.");
    }

    [Theory]
    [InlineData(1)]
    [InlineData(2)]
    [InlineData(int.MaxValue)]
    public void Create_preserves_positive_values(int value)
    {
        var result = CategoryId.Create(value);

        result.IsSuccess.ShouldBeTrue();
        result.Value.Value.ShouldBe(value);
        result.Value.ToString().ShouldBe(value.ToString());
    }

    [Fact]
    public void Equality_uses_the_integer_value()
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
