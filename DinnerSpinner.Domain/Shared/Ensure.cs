using System.Runtime.CompilerServices;

namespace DinnerSpinner.Domain.Shared;

public static class Ensure
{
    public static void NotNullOrWhiteSpace(
        string? value,
        [CallerArgumentExpression(nameof(value))] string? paramName = null)
    {
        if (string.IsNullOrWhiteSpace(value))
        {
            throw new ArgumentException("Value cannot be null, empty or consist only of white-space.");
        }
    }

    public static void NotNull(
        object? value,
        [CallerArgumentExpression(nameof(value))] string? paramName = null)
    {
        if (value is null)
        {
            throw new ArgumentNullException(paramName);
        }
    }
}