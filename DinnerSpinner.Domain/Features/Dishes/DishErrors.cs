using DinnerSpinner.Domain.Features.Categories;
using DinnerSpinner.Domain.Features.Common;
using DinnerSpinner.Domain.Shared;

namespace DinnerSpinner.Domain.Features.Dishes
{
    public static class DishErrors
    {
        public static Error NameRequired() => Error.Validation("Dish.NameRequired", Name.RequiredMessage);
        public static Error CategoryRequired() => Error.Validation("Dish.CategoryRequired", CategoryId.RequiredMessage);
        public static Error NotFound(int id) => Error.NotFound("Dish.NotFound", $"Dish with Id '{id}' was not found");
        public static Error NameConflict(string name) => Error.Conflict("Dish.NameConflict", $"Dish named '{name}' already exists in this category.");
    }
}