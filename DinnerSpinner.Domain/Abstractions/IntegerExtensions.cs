namespace DinnerSpinner.Domain.Abstractions
{
    public static class IntegerExtensions
    {
        public static bool IsWithin(this int value, int minimum, int maximum) =>
            value >= minimum && value <= maximum;
    }
}