using DinnerSpinner.Api.Features.Categories.Create;
using DinnerSpinner.Api.Features.Common;
using DinnerSpinner.Domain.Features.Common;

namespace DinnerSpinner.Tests.Api;

public class NameValidatorShould
{
    [Theory]
    [InlineData(null)]
    [InlineData("")]
    [InlineData("   ")]
    [InlineData("a")]
    public void ReturnValidationFailure_On_Validate_WhenNameIsInvalid(string? name)
    {
        var result = new NameValidator().Validate(new Request { Name = name! });

        result.IsValid.ShouldBeFalse();
        result.Errors.ShouldHaveSingleItem().ErrorMessage.ShouldNotBeNullOrWhiteSpace();
    }

    [Fact]
    public void ReturnValidationFailure_On_Validate_WhenNameIsTooLong()
    {
        var result = new NameValidator().Validate(new Request
        {
            Name = new string('a', Name.MaximumLength + 1)
        });

        result.IsValid.ShouldBeFalse();
        result.Errors.ShouldHaveSingleItem().ErrorMessage.ShouldBe(
            Name.Create(new string('a', Name.MaximumLength + 1)).Error.ToString());
    }

    [Theory]
    [InlineData(Name.MinimumLength)]
    [InlineData(Name.MaximumLength)]
    public void AcceptNormalizedName_On_Validate_WhenLengthIsWithinBounds(int length)
    {
        var result = new NameValidator().Validate(new Request
        {
            Name = $"  {new string('a', length)}  "
        });

        result.IsValid.ShouldBeTrue();
    }
}
