using System;
using CSharpFunctionalExtensions;
using DinnerSpinner.Domain.Abstractions;
using Entity = DinnerSpinner.Domain.Abstractions.Entity;

namespace DinnerSpinner.Api.Features.Common
{
    public static class ApplicationValidators
    {
        // Extension method for validating value objects using FluentValidation. Our domain objects are responsible for enforcing all validation rules/domain invariants; we leverage that in our API validators to avoid duplication of validation logic. Many thanks to Vladimir Khorikov for this approach, which he describes in his Pluralsight course "FluentValidation Fundamentals" (https://www.pluralsight.com/courses/fluentvalidation-fundamentals).
        public static IRuleBuilderOptions<T, TElement> MustBeValueObject<T, TValueObject, TElement>(
            this IRuleBuilder<T, TElement> ruleBuilder,
            Func<TElement, Result<TValueObject>> factoryMethod)
            where TValueObject : IValueObject => (IRuleBuilderOptions<T, TElement>)ruleBuilder.Custom((value, context) =>
                {
                    var result = factoryMethod(value);

                    if (result.IsFailure)
                    {
                        context.AddFailure(result.Error);
                    }
                });

        // Extension method for validating entities using FluentValidation. Our domain objects are responsible for enforcing all validation rules/domain invariants; we leverage that in our API validators to avoid duplication of validation logic. Many thanks to Vladimir Khorikov for this approach, which he describes in his Pluralsight course "FluentValidation Fundamentals" (https://www.pluralsight.com/courses/fluentvalidation-fundamentals).
        public static IRuleBuilderOptions<T, TElement> MustBeEntity<T, TElement, TEntity>(
            this IRuleBuilder<T, TElement> ruleBuilder,
            Func<TElement, Result<TEntity>> factoryMethod)
            where TEntity : Entity => (IRuleBuilderOptions<T, TElement>)ruleBuilder.Custom((value, context) =>
                {
                    var result = factoryMethod(value);

                    if (result.IsFailure)
                    {
                        context.AddFailure(result.Error);
                    }
                });
    }
}