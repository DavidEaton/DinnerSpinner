using DinnerSpinner.Api.Domain;
using DinnerSpinner.Api.Features.Dishes.Create;
using FastEndpoints;

namespace DinnerSpinner.Api.Features.Dishes
{
    internal class Mapper : Mapper<Request, Response, Dish>
    {
        public override Response FromEntity(Dish entity)
        {
            if (entity is not Dish dish)
            {
                return new Response();
            }
            var response = dish.ToCreateResponse();
            if (response is null)
            {
                return new Response();
            }
            return response;
        }
        public IReadOnlyList<List.Response> FromEntities(IReadOnlyList<Dish> entities)
        {
            return entities
                .OfType<Dish>()
                .Select(dish => dish.ToListResponse())
                .Where(response => response is not null)
                .Select(response => response!) // Ensure non-null for IReadOnlyList<Response>
                .ToList();
        }
    }
}