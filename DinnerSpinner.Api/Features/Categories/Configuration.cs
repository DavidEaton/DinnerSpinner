using DinnerSpinner.Domain.Features.Categories;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DinnerSpinner.Api.Features.Categories
{
    public class Configuration : EntityConfiguration<Category>
    {
        public override void Configure(EntityTypeBuilder<Category> builder)
        {
            base.Configure(builder);
            builder.ToTable("Category", "dbo");

            builder.OwnsOne(category => category.Name)
               .Property(name => name.Value)
               .HasColumnName("Name")
               .HasMaxLength(255)
               .IsRequired();
        }
    }
}
