using System.Reflection;
using DinnerSpinner.Domain.Features.Categories;

namespace DinnerSpinner.Tests.Domain;

public partial class CategoryIdShould
{
    [Fact]
    public void RequireFactory_On_Construct_WhenCalledPublicly()
    {
        typeof(CategoryId).GetConstructors(BindingFlags.Public | BindingFlags.Instance).ShouldBeEmpty();
        typeof(CategoryId).GetFields(BindingFlags.Public | BindingFlags.Instance).ShouldBeEmpty();
    }

    [Fact]
    public void PreventExternalMutation_On_Value()
    {
        var property = typeof(CategoryId).GetProperty(nameof(CategoryId.Value))!;

        property.GetMethod!.IsPublic.ShouldBeTrue();
        if (property.SetMethod is { } setter)
            setter.IsPrivate.ShouldBeTrue();
    }

    [Fact]
    public void PreventSubclassConstruction_On_Construct()
    {
        typeof(CategoryId).IsSealed.ShouldBeTrue();
        foreach (var constructor in typeof(CategoryId).GetConstructors(BindingFlags.NonPublic | BindingFlags.Instance))
            constructor.IsPrivate.ShouldBeTrue();
    }
}
