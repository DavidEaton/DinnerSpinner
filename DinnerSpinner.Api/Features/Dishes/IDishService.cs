using CSharpFunctionalExtensions;
using static DinnerSpinner.Api.Common.Errors;

namespace DinnerSpinner.Api.Features.Dishes
{
    public interface IDishService
    {
        Task<Result<Create.Response, Error>> CreateAsync(Create.Request request, CancellationToken cancellationToken);
        Task<Result<Maybe<GetById.Response>, Error>> GetByIdAsync(int id, CancellationToken cancellationToken);
        Task<IReadOnlyList<List.Response>> ListAsync(CancellationToken cancellationToken);
        Task<Result<Update.Response, Error>> UpdateAsync(int id, Update.Request request, CancellationToken cancellationToken);
        Task<UnitResult<Error>> DeleteAsync(int id, CancellationToken cancellationToken);
    }
}