using DinnerSpinner.Domain.Features.Common;

namespace DinnerSpinner.Tests.Domain;

public class NameTests
{
    [Theory]
    [InlineData(null)]
    [InlineData("")]
    [InlineData(" ")]
    [InlineData("\t\r\n")]
    [InlineData("\u2003\u00a0")]
    public void Create_rejects_missing_names(string? input)
    {
        var result = Name.Create(input);

        result.IsFailure.ShouldBeTrue();
        result.Error.ShouldBe(Name.RequiredMessage);
    }

    [Theory]
    [InlineData(1)]
    [InlineData(256)]
    [InlineData(10000)]
    public void Create_rejects_lengths_outside_the_allowed_range(int length)
    {
        var result = Name.Create(new string('a', length));

        result.IsFailure.ShouldBeTrue();
        result.Error.ShouldBe(Name.InvalidLengthMessage);
    }

    [Theory]
    [InlineData(2)]
    [InlineData(3)]
    [InlineData(254)]
    [InlineData(255)]
    public void Create_accepts_valid_lengths_after_trimming(int length)
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
    public void Create_validates_normalized_length(string input)
    {
        var result = Name.Create(input);

        result.IsFailure.ShouldBeTrue();
        result.Error.ShouldBe(Name.InvalidLengthMessage);
    }

    [Theory]
    [InlineData("  Mac  & Cheese  ", "Mac  & Cheese")]
    [InlineData("\u2003Crème brûlée\u00a0", "Crème brûlée")]
    [InlineData("  Ab  ", "Ab")]
    public void Create_preserves_case_internal_spaces_and_unicode(string input, string expected)
    {
        Name.Create(input).Value.Value.ShouldBe(expected);
    }

    [Fact]
    public void Equality_uses_the_normalized_case_sensitive_value()
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
