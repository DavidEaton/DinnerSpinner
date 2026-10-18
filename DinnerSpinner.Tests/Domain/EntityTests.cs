using DinnerSpinner.Domain.Abstractions;
using DinnerSpinner.Domain.Features.Categories;
using DinnerSpinner.Domain.Features.Common;
using DinnerSpinner.Domain.Features.Dishes;

namespace DinnerSpinner.Tests.Domain;

public class EntityTests
{
    [Fact]
    public void An_entity_equals_itself_even_before_persistence()
    {
        Entity entity = Category.Create(Name.Create("Dinner").Value).Value;
        Entity sameReference = entity;

        entity.Equals(entity).ShouldBeTrue();
        (entity == sameReference).ShouldBeTrue();
        (entity != sameReference).ShouldBeFalse();
    }

    [Fact]
    public void Distinct_transient_entities_are_not_equal_even_with_equal_names()
    {
        var name = Name.Create("Dinner").Value;
        Entity first = Category.Create(name).Value;
        Entity second = Category.Create(name).Value;

        first.Equals(second).ShouldBeFalse();
        second.Equals(first).ShouldBeFalse();
        (first == second).ShouldBeFalse();
        (first != second).ShouldBeTrue();
    }

    [Theory]
    [InlineData(1, 1, true)]
    [InlineData(int.MaxValue, int.MaxValue, true)]
    [InlineData(1, 2, false)]
    [InlineData(0, 1, false)]
    [InlineData(1, 0, false)]
    [InlineData(0, 0, false)]
    public void Equality_uses_nonzero_identity_for_the_same_runtime_type(int firstId, int secondId, bool equal)
    {
        Entity first = new StoredCategory(firstId, "Dinner");
        Entity second = new StoredCategory(secondId, "Lunch");

        first.Equals(second).ShouldBe(equal);
        second.Equals(first).ShouldBe(equal);
        (first == second).ShouldBe(equal);
        (first != second).ShouldBe(!equal);
        if (equal)
            first.GetHashCode().ShouldBe(second.GetHashCode());
    }

    [Fact]
    public void Different_runtime_types_are_not_equal_even_with_the_same_identity()
    {
        Entity category = new StoredCategory(1, "Dinner");
        Entity dish = new StoredDish(1);

        category.Equals(dish).ShouldBeFalse();
        dish.Equals(category).ShouldBeFalse();
        (category == dish).ShouldBeFalse();
        (category != dish).ShouldBeTrue();
    }

    [Fact]
    public void Equality_handles_null_and_unrelated_objects()
    {
        Entity entity = new StoredCategory(1, "Dinner");
        Entity? missing = null;
        Entity? alsoMissing = null;

        entity.Equals(null).ShouldBeFalse();
        entity.Equals("Dinner").ShouldBeFalse();
        (entity == missing).ShouldBeFalse();
        (missing == entity).ShouldBeFalse();
        (entity != missing).ShouldBeTrue();
        (missing != entity).ShouldBeTrue();
        (missing == alsoMissing).ShouldBeTrue();
        (missing != alsoMissing).ShouldBeFalse();
    }

    [Fact]
    public void Name_changes_do_not_change_persisted_identity_or_hash_code()
    {
        var category = new StoredCategory(1, "Dinner");
        var hashCode = category.GetHashCode();
        var equal = new StoredCategory(1, "Other");

        category.ChangeName(Name.Create("Lunch").Value).IsSuccess.ShouldBeTrue();

        category.Id.ShouldBe(1);
        category.Equals(equal).ShouldBeTrue();
        category.GetHashCode().ShouldBe(hashCode);
    }

    // Subclasses exercise the protected identity setter as a persistence adapter would.
    // No reflection is used to manufacture invalid value objects.
    private sealed class StoredCategory : Category
    {
        public StoredCategory(int id, string name)
        {
            Id = id;
            ChangeName(Name.Create(name).Value);
        }
    }

    private sealed class StoredDish : Dish
    {
        public StoredDish(int id)
        {
            Id = id;
            ChangeName(Name.Create("Soup").Value);
            ChangeCategory(CategoryId.Create(1).Value);
        }
    }
}
