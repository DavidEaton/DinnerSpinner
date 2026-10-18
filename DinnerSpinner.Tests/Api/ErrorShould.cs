using DinnerSpinner.Api.Common;
using DinnerSpinner.Domain.Abstractions;
using DinnerSpinner.Domain.Shared;
using Microsoft.AspNetCore.Http.HttpResults;

namespace DinnerSpinner.Tests.Api;

public class ErrorShould
{
    [Theory]
    [InlineData(ErrorType.Validation, 400, "Bad Request")]
    [InlineData(ErrorType.NotFound, 404, "Not Found")]
    [InlineData(ErrorType.Conflict, 409, "Conflict")]
    [InlineData(ErrorType.Failure, 500, "Server Failure")]
    public void PreserveError_On_ToProblemDetails(ErrorType type, int status, string title)
    {
        var error = new Error("Test.Error", "A useful description", type);
        var problem = error.ToProblemDetails().ShouldBeOfType<ProblemHttpResult>().ProblemDetails;
        problem.Status.ShouldBe(status);
        problem.Title.ShouldBe(title);
        problem.Detail.ShouldBe(error.Description);
        problem.Type.ShouldNotBeNull().ShouldContain("#section-");
        problem.Extensions["errors"].ShouldBeOfType<Error[]>().ShouldBe(new[] { error });
        Result.Failure(error).ToProblemDetails().ShouldBeOfType<ProblemHttpResult>()
            .ProblemDetails.Status.ShouldBe(status);
    }

    [Fact]
    public void RejectSuccess_On_ToProblemDetails()
    {
        Should.Throw<InvalidOperationException>(() => Result.Success().ToProblemDetails());
    }
}
