namespace DinnerSpinner.Domain.Abstractions
{
    public interface IResult
    {
        bool IsFailure { get; }

        bool IsSuccess { get; }
    }
}