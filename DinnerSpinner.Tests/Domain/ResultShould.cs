using DinnerSpinner.Domain.Abstractions;
using DinnerSpinner.Domain.Shared;

namespace DinnerSpinner.Tests.Domain;

public class ResultShould
{
    [Fact]
    public void ReturnSuccessWithoutError_On_Success()
    {
        var result = Result.Success();

        result.IsSuccess.ShouldBeTrue();
        result.IsFailure.ShouldBeFalse();
        result.Error.ShouldBe(Error.None);
    }

    [Fact]
    public void PreserveError_On_Failure_WhenProvided()
    {
        var error = Error.Validation("Invalid", "Invalid input");

        var result = Result.Failure(error);

        result.IsSuccess.ShouldBeFalse();
        result.IsFailure.ShouldBeTrue();
        result.Error.ShouldBe(error);
    }

    [Fact]
    public void RejectMissingError_On_Failure_WhenNone()
    {
        Should.Throw<InvalidOperationException>(() => Result.Failure(Error.None));
    }
}
