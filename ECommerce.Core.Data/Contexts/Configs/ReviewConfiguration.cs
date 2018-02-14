using ECommerce.Core.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ECommerce.Core.Data.Contexts.Configs
{
    class ReviewConfiguration : IEntityTypeConfiguration<Review>
    {
        public void Configure(EntityTypeBuilder<Review> builder)
        {
            builder.HasKey(c => c.Id);

            builder.Property(c => c.ReviewContent).IsRequired().HasMaxLength(1024);


            builder.HasOne(c => c.Product).WithMany(c => c.ReviewList).HasForeignKey(c => c.ProductId);

            builder.HasOne(c => c.User).WithMany(c => c.ReviewList).HasForeignKey(c => c.UserId);
            
        }
    }
}