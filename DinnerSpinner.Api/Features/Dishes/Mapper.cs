using DinnerSpinner.Api.Domain;
//using DinnerSpinner.Api.Features.Dishes.List;
using DinnerSpinner.Api.Features.Dishes.Create;
using FastEndpoints;
using System;

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
            return (IReadOnlyList<List.Response>)entities
                .OfType<Dish>()
                .Select(d => d.ToListResponse())
                .Where(r => r is not null)
                .Select(r => r!) // Ensure non-null for IReadOnlyList<Response>
                .ToList();
        }
    }
}