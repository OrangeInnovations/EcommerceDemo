using ECommerce.Core.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ECommerce.Core.Data.Contexts.Configs
{
    class ProductConfiguration : IEntityTypeConfiguration<Product>
    {
        public void Configure(EntityTypeBuilder<Product> builder)
        {
            builder.HasKey(c => c.Id);

            builder.Property(c => c.Title).IsRequired();
            builder.Property(c => c.Description).HasMaxLength(1000);

            builder.HasOne(c => c.Category).WithMany(c => c.ProductList).HasForeignKey(c => c.CategoryId);

            builder.HasMany(c => c.ReviewList).WithOne(c => c.Product).HasForeignKey(c => c.ProductId);
        }
    }
}
