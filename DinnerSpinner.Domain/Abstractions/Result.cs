namespace DinnerSpinner.Domain.Abstractions
{
    public readonly partial struct Result : IResult
    {
        public bool IsFailure { get; }
        public bool IsSuccess => !IsFailure;
        private readonly string _error;

        private Result(bool isFailure, string error)
        {
            IsFailure = ErrorStateGuard(isFailure, error);
            _error = error;
        }

        private bool ErrorStateGuard(bool isFailure, string error)
        {
            
            if (isFailure && string.IsNullOrEmpty(error))
            {
                throw new ArgumentNullException(nameof(error), "Error message must be provided for failure results.");
            }

            if (!isFailure && !string.IsNullOrEmpty(error))
            {
                throw new ArgumentException("Error message should not be provided for success results.", nameof(error));
            }

            return isFailure;
        }
    }
}