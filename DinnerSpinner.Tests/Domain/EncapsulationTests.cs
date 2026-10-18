using System.Reflection;
using DinnerSpinner.Domain.Abstractions;
using DinnerSpinner.Domain.Features.Categories;
using DinnerSpinner.Domain.Features.Common;
using DinnerSpinner.Domain.Features.Dishes;

namespace DinnerSpinner.Tests.Domain;

public class EncapsulationTests
{
    [Theory]
    [InlineData(typeof(Name))]
    [InlineData(typeof(CategoryId))]
    [InlineData(typeof(Category))]
    [InlineData(typeof(Dish))]
    public void Domain_types_have_no_public_constructors(Type type)
    {
        type.GetConstructors(BindingFlags.Public | BindingFlags.Instance).ShouldBeEmpty();
    }

    [Theory]
    [InlineData(typeof(Name))]
    [InlineData(typeof(CategoryId))]
    public void Value_objects_are_sealed_and_their_constructors_are_private(Type type)
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
    public void Domain_properties_cannot_be_set_outside_the_type(Type type, string propertyName)
    {
        var property = type.GetProperty(propertyName)!;

        property.GetMethod!.IsPublic.ShouldBeTrue();
        if (property.SetMethod is { } setter)
            setter.IsPrivate.ShouldBeTrue();
    }

    [Fact]
    public void Entity_identity_can_only_be_assigned_by_the_entity_or_a_subclass()
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
    public void Domain_types_expose_no_mutable_public_instance_fields(Type type)
    {
        type.GetFields(BindingFlags.Instance | BindingFlags.Public).ShouldBeEmpty();
    }

    [Theory]
    [InlineData(typeof(Category))]
    [InlineData(typeof(Dish))]
    public void Entity_materialization_constructors_remain_protected(Type type)
    {
        var constructor = type.GetConstructor(
            BindingFlags.Instance | BindingFlags.NonPublic, null, Type.EmptyTypes, null);

        constructor.ShouldNotBeNull();
        constructor.IsFamily.ShouldBeTrue();
    }
}
