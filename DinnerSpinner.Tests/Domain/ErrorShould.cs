using DinnerSpinner.Domain.Shared;

namespace DinnerSpinner.Tests.Domain;

public class ErrorShould
{
    [Theory]
    [InlineData(ErrorType.Validation)]
    [InlineData(ErrorType.NotFound)]
    [InlineData(ErrorType.Conflict)]
    [InlineData(ErrorType.Failure)]
    [InlineData(ErrorType.Required)]
    public void PreserveDetails_On_Factory_WhenTypeSelected(ErrorType type)
    {
        var error = type switch
        {
            ErrorType.Validation => Error.Validation("Code", "Description"),
            ErrorType.NotFound => Error.NotFound("Code", "Description"),
            ErrorType.Conflict => Error.Conflict("Code", "Description"),
            ErrorType.Failure => Error.Failure("Code", "Description"),
            ErrorType.Required => Error.Required("Code", "Description"),
            _ => throw new ArgumentOutOfRangeException(nameof(type))
        };

        error.Code.ShouldBe("Code");
        error.Description.ShouldBe("Description");
        error.Type.ShouldBe(type);
        error.ShouldBe(new Error("Code", "Description", type));
    }

    [Fact]
    public void CompareAllDetails_On_Equals_WhenValuesDiffer()
    {
        var error = Error.Validation("Code", "Description");
        var equal = Error.Validation("Code", "Description");

        (error == equal).ShouldBeTrue();
        error.GetHashCode().ShouldBe(equal.GetHashCode());
        error.ShouldNotBe(Error.Validation("Other", "Description"));
        error.ShouldNotBe(Error.Validation("Code", "Other"));
        error.ShouldNotBe(Error.Failure("Code", "Description"));
        Error.None.ShouldNotBe(Error.NullValue);
    }
}
