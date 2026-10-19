using DinnerSpinner.Domain.Features.Dishes;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DinnerSpinner.Api.Features.Dishes;

public class Configuration : EntityConfiguration<Dish>
{
    public override void Configure(EntityTypeBuilder<Dish> builder)
    {
        base.Configure(builder);
        builder.ToTable("Dish", "dbo");

        builder.OwnsOne(dish => dish.Name)
           .Property(name => name.Value)
           .HasColumnName("Name")
           .HasMaxLength(255)
           .IsRequired();

        builder.HasOne(dish => dish.Category)
            .WithMany()
            .HasForeignKey("CategoryId")
            .IsRequired();

        builder.Navigation(dish => dish.Category).AutoInclude();
    }
}
