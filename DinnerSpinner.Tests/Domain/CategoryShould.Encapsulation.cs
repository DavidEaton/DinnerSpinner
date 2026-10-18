using System.Reflection;
using DinnerSpinner.Domain.Features.Common;
using DinnerSpinner.Domain.Features.Categories;
using DinnerSpinner.Domain.Features.Dishes;

namespace DinnerSpinner.Tests.Domain;

public partial class CategoryShould
{
    [Fact]
    public void RequireFactory_On_Construct_WhenCalledPublicly()
    {
        typeof(Category).GetConstructors(BindingFlags.Public | BindingFlags.Instance).ShouldBeEmpty();
        typeof(Category).GetFields(BindingFlags.Public | BindingFlags.Instance).ShouldBeEmpty();
    }

    [Fact]
    public void PreventExternalMutation_On_Name()
    {
        var property = typeof(Category).GetProperty(nameof(Category.Name))!;

        property.GetMethod!.IsPublic.ShouldBeTrue();
        if (property.SetMethod is { } setter)
            setter.IsPrivate.ShouldBeTrue();
    }

    [Fact]
    public void AllowPersistenceConstruction_On_Construct()
    {
        var constructor = typeof(Category).GetConstructor(
            BindingFlags.Instance | BindingFlags.NonPublic, null, Type.EmptyTypes, null);

        constructor.ShouldNotBeNull();
        constructor.IsFamily.ShouldBeTrue();
    }
}
