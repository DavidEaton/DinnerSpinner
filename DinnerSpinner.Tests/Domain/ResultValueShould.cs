using DinnerSpinner.Domain.Abstractions;
using DinnerSpinner.Domain.Shared;

namespace DinnerSpinner.Tests.Domain;

public class ResultValueShould
{
    [Theory]
    [InlineData(0)]
    [InlineData(int.MinValue)]
    [InlineData(int.MaxValue)]
    public void PreserveValue_On_Create_WhenValueType(int value)
    {
        var result = Result.Create(value);

        result.IsSuccess.ShouldBeTrue();
        result.Value.ShouldBe(value);
        result.Error.ShouldBe(Error.None);
    }

    [Theory]
    [InlineData("")]
    [InlineData("Soup")]
    public void PreserveValue_On_Create_WhenNonNull(string value)
    {
        var result = Result.Create(value);

        result.IsSuccess.ShouldBeTrue();
        result.Value.ShouldBeSameAs(value);
    }

    [Fact]
    public void RejectNull_On_Create()
    {
        var result = Result.Create<string>(null);

        result.IsFailure.ShouldBeTrue();
        result.Error.ShouldBe(Error.NullValue);
        Should.Throw<InvalidOperationException>(() => { _ = result.Value; });
    }

    [Fact]
    public void PreserveValue_On_Success_WhenProvided()
    {
        var value = new object();

        var result = Result.Success(value);

        result.IsSuccess.ShouldBeTrue();
        result.Value.ShouldBeSameAs(value);
        result.Error.ShouldBe(Error.None);
    }

    [Fact]
    public void PreventValueAccess_On_Failure_WhenErrorProvided()
    {
        var error = Error.Validation("Invalid", "Invalid input");

        var result = Result.Failure<string>(error);

        result.IsFailure.ShouldBeTrue();
        result.Error.ShouldBe(error);
        Should.Throw<InvalidOperationException>(() => { _ = result.Value; });
    }

    [Fact]
    public void RejectMissingError_On_Failure_WhenNone()
    {
        Should.Throw<InvalidOperationException>(() => Result.Failure<string>(Error.None));
    }

    [Theory]
    [InlineData(null)]
    [InlineData("")]
    [InlineData("Soup")]
    public void ValidateNull_On_ImplicitConversion_WhenNullableInput(string? value)
    {
        Result<string> result = value;

        result.IsSuccess.ShouldBe(value is not null);
        if (value is null)
            result.Error.ShouldBe(Error.NullValue);
        else
            result.Value.ShouldBeSameAs(value);
    }
}
