using System.Reflection;
using DinnerSpinner.Domain.Abstractions;
using DinnerSpinner.Domain.Features.Categories;
using DinnerSpinner.Domain.Features.Common;
using DinnerSpinner.Domain.Features.Dishes;

namespace DinnerSpinner.Tests.Domain;

public class DomainEncapsulationShould
{
    [Theory]
    [InlineData(typeof(Name))]
    [InlineData(typeof(CategoryId))]
    [InlineData(typeof(Category))]
    [InlineData(typeof(Dish))]
    public void RequireFactories_On_Construct_WhenCalledPublicly(Type type)
    {
        type.GetConstructors(BindingFlags.Public | BindingFlags.Instance).ShouldBeEmpty();
    }

    [Theory]
    [InlineData(typeof(Name))]
    [InlineData(typeof(CategoryId))]
    public void PreventSubclassConstruction_On_ValueObjects(Type type)
    {
        type.IsSealed.ShouldBeTrue();
        foreach (var constructor in type.GetConstructors(BindingFlags.NonPublic | BindingFlags.Instance))
            constructor.IsPrivate.ShouldBeTrue();
    }

    [Theory]
    [InlineData(typeof(Name), nameof(Name.Value))]
    [InlineData(typeof(CategoryId), nameof(CategoryId.Value))]
    [InlineData(typeof(Category), nameof(Category.Name))]
    [InlineData(typeof(Dish), nameof(Dish.Name))]
    [InlineData(typeof(Dish), nameof(Dish.CategoryId))]
    public void PreventExternalMutation_On_DomainProperties(Type type, string propertyName)
    {
        var property = type.GetProperty(propertyName)!;

        property.GetMethod!.IsPublic.ShouldBeTrue();
        if (property.SetMethod is { } setter)
            setter.IsPrivate.ShouldBeTrue();
    }

    [Fact]
    public void RestrictIdentityAssignment_On_Entity()
    {
        typeof(Entity).IsAbstract.ShouldBeTrue();
        var id = typeof(Entity).GetProperty(nameof(Entity.Id))!;

        id.GetMethod!.IsPublic.ShouldBeTrue();
        id.SetMethod!.IsFamily.ShouldBeTrue();
    }

    [Theory]
    [InlineData(typeof(Name))]
    [InlineData(typeof(CategoryId))]
    [InlineData(typeof(Category))]
    [InlineData(typeof(Dish))]
    public void PreventExternalMutation_On_InstanceFields(Type type)
    {
        type.GetFields(BindingFlags.Instance | BindingFlags.Public).ShouldBeEmpty();
    }

    [Theory]
    [InlineData(typeof(Category))]
    [InlineData(typeof(Dish))]
    public void AllowPersistenceConstruction_On_Entities(Type type)
    {
        var constructor = type.GetConstructor(
            BindingFlags.Instance | BindingFlags.NonPublic, null, Type.EmptyTypes, null);

        constructor.ShouldNotBeNull();
        constructor.IsFamily.ShouldBeTrue();
    }
}
