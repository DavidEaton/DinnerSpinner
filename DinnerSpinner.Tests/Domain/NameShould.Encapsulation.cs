using System.Reflection;
using DinnerSpinner.Domain.Features.Common;
using DinnerSpinner.Domain.Features.Categories;
using DinnerSpinner.Domain.Features.Dishes;

namespace DinnerSpinner.Tests.Domain;

public partial class NameShould
{
    [Fact]
    public void RequireFactory_On_Construct_WhenCalledPublicly()
    {
        typeof(Name).GetConstructors(BindingFlags.Public | BindingFlags.Instance).ShouldBeEmpty();
        typeof(Name).GetFields(BindingFlags.Public | BindingFlags.Instance).ShouldBeEmpty();
    }

    [Fact]
    public void PreventExternalMutation_On_Value()
    {
        var property = typeof(Name).GetProperty(nameof(Name.Value))!;

        property.GetMethod!.IsPublic.ShouldBeTrue();
        if (property.SetMethod is { } setter)
            setter.IsPrivate.ShouldBeTrue();
    }

    [Fact]
    public void PreventSubclassConstruction_On_Construct()
    {
        typeof(Name).IsSealed.ShouldBeTrue();
        foreach (var constructor in typeof(Name).GetConstructors(BindingFlags.NonPublic | BindingFlags.Instance))
            constructor.IsPrivate.ShouldBeTrue();
    }
}
