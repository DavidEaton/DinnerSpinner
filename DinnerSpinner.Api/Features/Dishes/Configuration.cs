using DinnerSpinner.Domain.Features.Dishes;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DinnerSpinner.Api.Features.Dishes
{
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

            builder
                .Property(dish => dish.CategoryId)
                .HasColumnType("int")
                .HasColumnName("CategoryId")
                .IsRequired();
                // .HasConversion(
                //     categoryId => categoryId.Value,
                //     value => CategoryId.Create(value).Value);
        }
    }
}
