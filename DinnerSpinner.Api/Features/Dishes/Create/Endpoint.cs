﻿using CSharpFunctionalExtensions;
using DinnerSpinner.Api.Data;
using DinnerSpinner.Api.Features.Dishes.Read.GetById;
using FastEndpoints;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.EntityFrameworkCore;
using System.Numerics;
using static DinnerSpinner.Api.Common.Errors;

namespace DinnerSpinner.Api.Features.Dishes.Create
{
    internal class Endpoint(AppDbContext db) : Endpoint<Request, Response>
    {
        public override void Configure()
        {
            Post("/api/dishes/create");
            Validator<Validator>();
            AllowAnonymous();
            Summary(s => s.Summary = "Create a new dish");
        }
        public override async Task<Result<Dish, Error>> HandleAsync(Request request, CancellationToken cancellationToken)
        {
            var category = await db
                .Categories
                .FirstOrDefaultAsync(
                    category => category.Id == request.CategoryId,
                    cancellationToken);

            if (category is null)
            {
                await Send.NotFoundAsync(cancellationToken);
                //return Result.Failure<Dish, Error>(new Error(ErrorCode.NotFound, "Category not found"));
            }

            var entity = new Dish
            {
                Name = request.Name,
                Category = category
            };

            db.Dishes.Add(entity);
            await db.SaveChangesAsync(cancellationToken);

            await Send.CreatedAtAsync<Read.GetById.Endpoint>(
                routeValues: new { id = entity.Id },
                responseBody: entity.ToCreateResponse(),
                cancellation: cancellationToken);

            return Result.Success<Dish, Error>(entity);
        }
    }
}
