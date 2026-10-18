using DinnerSpinner.Domain.Shared;

namespace DinnerSpinner.Tests.Domain;

public class EnsureShould
{
    [Theory]
    [InlineData(null)]
    [InlineData("")]
    [InlineData(" \t\r\n")]
    [InlineData("\u2003")]
    public void RejectMissingText_On_NotNullOrWhiteSpace_WhenInvalid(string? value)
    {
        Should.Throw<ArgumentException>(() => Ensure.NotNullOrWhiteSpace(value));
    }

    [Theory]
    [InlineData("a")]
    [InlineData(" Soup ")]
    public void AcceptText_On_NotNullOrWhiteSpace_WhenPresent(string value)
    {
        Should.NotThrow(() => Ensure.NotNullOrWhiteSpace(value));
    }

    [Fact]
    public void ReportCallerArgument_On_NotNull_WhenNull()
    {
        object? missing = null;

        var exception = Should.Throw<ArgumentNullException>(() => Ensure.NotNull(missing));

        exception.ParamName.ShouldBe(nameof(missing));
    }

    [Fact]
    public void ReportExplicitParameter_On_NotNull_WhenNull()
    {
        var exception = Should.Throw<ArgumentNullException>(() => Ensure.NotNull(null, "category"));

        exception.ParamName.ShouldBe("category");
    }

    [Theory]
    [InlineData("")]
    [InlineData(" ")]
    [InlineData(0)]
    public void AcceptNonNullObjects_On_NotNull_WhenPresent(object value)
    {
        Should.NotThrow(() => Ensure.NotNull(value));
    }
}
