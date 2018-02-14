using ECommerce.Core.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ECommerce.Core.Data.Contexts.Configs
{
    class OrderConfiguration : IEntityTypeConfiguration<Order>
    {
        public void Configure(EntityTypeBuilder<Order> builder)
        {
            builder.HasKey(c => c.Id);
            builder.Property(c => c.OrderNumber).IsRequired();
            builder.HasAlternateKey(c => c.OrderNumber);

            builder.HasOne(c => c.User).WithMany(c => c.OrderList).HasForeignKey(c => c.UserId);

            builder.HasMany(c => c.OrderItemList).WithOne(c => c.Order).HasForeignKey(c => c.OrderId);
        }
    }
}