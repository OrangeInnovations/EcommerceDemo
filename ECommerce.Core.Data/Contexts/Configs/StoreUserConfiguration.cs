using ECommerce.Core.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ECommerce.Core.Data.Contexts.Configs
{
    class StoreUserConfiguration : IEntityTypeConfiguration<StoreUser>
    {
        public void Configure(EntityTypeBuilder<StoreUser> builder)
        {
            builder.HasKey(c => c.Id);
            builder.Property(c => c.FirstName).IsRequired();
            builder.Property(c => c.LastName).IsRequired();

            builder.Property(c => c.Email).IsRequired();
            builder.Property(c => c.UserName).IsRequired();

            builder.Ignore(c => c.FullName);
        }
    }
}