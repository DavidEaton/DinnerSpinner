using CSharpFunctionalExtensions;
using DinnerSpinner.Api.Common;
using DinnerSpinner.Api.Data;
using Microsoft.EntityFrameworkCore;
using static DinnerSpinner.Api.Common.Errors;

namespace DinnerSpinner.Api.Features.Dishes
{
    public class DishService : IDishService
    {
        private readonly AppDbContext db;

        public DishService(AppDbContext db) => this.db = db;

        public Task<Result<Create.Response, Error>> CreateAsync(Create.Request request, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }

        public Task<UnitResult<Error>> DeleteAsync(int id, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }

        public Task<Result<Maybe<GetById.Response>, Error>> GetByIdAsync(int id, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }

        public async Task<IReadOnlyList<List.Response>> ListAsync(CancellationToken cancellationToken)
        {
            // Pass the token into EF Core; it will cancel the DB call if the request aborts.
            var dishes = await db.Dishes
                .AsNoTracking()
                .Select(dish => new List.Response
                {
                    Id = dish.Id,
                    Name = dish.Name,
                    Category = dish.Category.Name
                })
                .ToListAsync(cancellationToken);

            return dishes;
        }

        public Task<Result<Create.Response, Error>> UpdateAsync(int id, Create.Request request, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }
    }
}
