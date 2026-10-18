using DinnerSpinner.Domain.Abstractions;

namespace DinnerSpinner.Tests.Domain;

public class IntegerShould
{
    [Theory]
    [InlineData(1, 2, 255, false)]
    [InlineData(2, 2, 255, true)]
    [InlineData(255, 2, 255, true)]
    [InlineData(256, 2, 255, false)]
    [InlineData(int.MinValue, int.MinValue, int.MaxValue, true)]
    [InlineData(int.MaxValue, int.MinValue, int.MaxValue, true)]
    [InlineData(0, 0, 0, true)]
    [InlineData(1, 2, 0, false)]
    public void IncludeOnlyRangeMembers_On_IsWithin_WhenBoundsProvided(int value, int minimum, int maximum, bool expected)
    {
        value.IsWithin(minimum, maximum).ShouldBe(expected);
    }
}
