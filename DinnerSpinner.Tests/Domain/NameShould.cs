using DinnerSpinner.Domain.Features.Dishes;
using DinnerSpinner.Domain.Features.Common;

namespace DinnerSpinner.Tests.Domain;

public partial class NameShould
{
    [Theory]
    [InlineData(null)]
    [InlineData("")]
    [InlineData(" ")]
    [InlineData("\t\r\n")]
    [InlineData("\u2003\u00a0")]
    public void RejectInvalidInput_On_Create_WhenNameIsMissing(string? input)
    {
        var result = Name.Create(input!);

        result.IsFailure.ShouldBeTrue();
        result.Error.ShouldBe(input is null
            ? Errors.ValidNameRequired()
            : Errors.ValidLengthName(Name.MinimumLength, Name.MaximumLength));
    }

    [Theory]
    [InlineData(1)]
    [InlineData(256)]
    [InlineData(10000)]
    public void RejectInvalidLength_On_Create_WhenOutsideBounds(int length)
    {
        var result = Name.Create(new string('a', length));

        result.IsFailure.ShouldBeTrue();
        result.Error.ShouldBe(Errors.ValidLengthName(Name.MinimumLength, Name.MaximumLength));
    }

    [Theory]
    [InlineData(2)]
    [InlineData(3)]
    [InlineData(254)]
    [InlineData(255)]
    public void PreserveNormalizedValue_On_Create_WhenLengthIsValid(int length)
    {
        var input = new string('a', length);

        var result = Name.Create($" \t{input}\r\n");

        result.IsSuccess.ShouldBeTrue();
        result.Value.Value.ShouldBe(input);
        result.Value.ToString().ShouldBe(input);
    }

    [Theory]
    [InlineData(" a ")]
    [InlineData("\tZ\n")]
    public void RejectInvalidLength_On_Create_AfterTrimming(string input)
    {
        var result = Name.Create(input!);

        result.IsFailure.ShouldBeTrue();
        result.Error.ShouldBe(Errors.ValidLengthName(Name.MinimumLength, Name.MaximumLength));
    }

    [Theory]
    [InlineData("  Mac  & Cheese  ", "Mac  & Cheese")]
    [InlineData("\u2003Crème brûlée\u00a0", "Crème brûlée")]
    [InlineData("  Ab  ", "Ab")]
    public void PreserveContent_On_Create_WhenTrimmingWhitespace(string input, string expected)
    {
        Name.Create(input).Value.Value.ShouldBe(expected);
    }

    [Fact]
    public void CompareNormalizedValues_On_Equals_WhenCaseDiffers()
    {
        var name = Name.Create("Soup").Value;
        var equal = Name.Create(" Soup ").Value;
        var different = Name.Create("soup").Value;

        (name == equal).ShouldBeTrue();
        name.Equals(equal).ShouldBeTrue();
        name.GetHashCode().ShouldBe(equal.GetHashCode());
        (name != different).ShouldBeTrue();
        name.Equals(null).ShouldBeFalse();
        name.Equals((object)"Soup").ShouldBeFalse();
        var copy = name with { };
        copy.ShouldBe(name);
        copy.Value.ShouldBe("Soup");
    }
}
