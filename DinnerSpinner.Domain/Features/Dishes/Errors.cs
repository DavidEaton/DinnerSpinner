using DinnerSpinner.Domain.Shared;

namespace DinnerSpinner.Domain.Features.Dishes
{
    public static class Errors
    {
        public static Error
            Empty() => Error
            .Validation(
                code: "Dish.Errors.Empty",
                description: "Name and Category are required.");
        public static Error
            NameRequired() => Error
            .Validation(
                code: "Dish.Errors.NameRequired",
                description: "Name is required.");
        public static Error
            CategoryRequired() => Error
            .Validation(
                code: "Dish.Errors.CategoryRequired",
                description: "Category is required.");
        public static Error
            NotFound(int id) => Error
            .NotFound(
                code: "Dish.Errors.NotFound",
                description: $"Dish with Id '{id}' was not found");
        public static Error
            Conflict(string name, string category) => Error
            .Conflict(
                code: "Dish.Errors.Conflict",
                description: $"Dish name '{name}' in category '{category}' is taken.");
    }
}