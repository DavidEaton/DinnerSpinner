using DinnerSpinner.Api.Features.Categories.Update;
using DinnerSpinner.Domain.Features.Common;

namespace DinnerSpinner.Tests.Api;

public class ValidatorShould
{
    [Theory]
    [InlineData(null)]
    [InlineData("")]
    [InlineData("   ")]
    [InlineData("a")]
    public void ReturnDomainError_On_Validate_WhenNameIsInvalid(string? name)
    {
        var expected = Name.Create(name!).Error;
        var result = new Validator().Validate(new Request(1, name!));

        result.IsValid.ShouldBeFalse();
        var error = result.Errors.ShouldHaveSingleItem();
        error.PropertyName.ToLowerInvariant().ShouldBe("name");
        error.ErrorCode.ShouldBe(expected.Code);
        error.ErrorMessage.ShouldBe(expected.Description);
    }

    [Theory]
    [InlineData(Name.MinimumLength)]
    [InlineData(Name.MaximumLength)]
    public void AcceptNormalizedName_On_Validate_WhenLengthIsWithinBounds(int length)
    {
        var result = new Validator().Validate(new Request(1, $"  {new string('a', length)}  "));

        result.IsValid.ShouldBeTrue();
    }

    [Fact]
    public void RejectName_On_Validate_WhenLengthExceedsMaximum()
    {
        var name = new string('a', Name.MaximumLength + 1);
        var result = new Validator().Validate(new Request(1, name));

        result.IsValid.ShouldBeFalse();
        result.Errors.ShouldHaveSingleItem().ErrorCode.ShouldBe(Name.Create(name).Error.Code);
    }
}
