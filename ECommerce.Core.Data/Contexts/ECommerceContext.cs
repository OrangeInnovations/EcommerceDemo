using System.Linq;
using ECommerce.Core.Data.Contexts.Configs;
using ECommerce.Core.Entities;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;

namespace ECommerce.Core.Data.Contexts
{
    public class ECommerceContext : IdentityDbContext<StoreUser>
    {
        private readonly string _connectionString = string.Empty;

        public ECommerceContext(DbContextOptions<ECommerceContext> options) : base(options)
        {
        }
        public ECommerceContext(string connectionString)
        {
            _connectionString = connectionString;
        }

        public ECommerceContext()
        {
            
        }

        public DbSet<Product> ProductSet { get; set; }
        public DbSet<Order> OrderSet { get; set; }

        public DbSet<OrderItem> OrderItemSet { get; set; }
        public DbSet<Category> CategorySet { get; set; }
        public DbSet<Review> ReviewSet { get; set; }


        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!string.IsNullOrEmpty(_connectionString))
            {
                optionsBuilder.UseSqlServer(_connectionString, options => options.EnableRetryOnFailure());
            }

            optionsBuilder.UseSqlServer(@"Server=(localdb)\MSSQLLocalDB;Database=ECommerceDB;Integrated Security=True;Connect Timeout=180;Encrypt=False;TrustServerCertificate=True;MultipleActiveResultSets=True");
            
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            foreach (var entityType in modelBuilder.Model.GetEntityTypes())
            {
                foreach (IMutableProperty property in entityType.GetProperties())
                {
                    property.SetMaxLength(256);
                }
            }

            foreach (var relationship in modelBuilder.Model.GetEntityTypes().SelectMany(e => e.GetForeignKeys()))
            {
                relationship.DeleteBehavior = DeleteBehavior.Restrict;
            }

            modelBuilder.ApplyConfiguration(new CategoryConfiguration());
            modelBuilder.ApplyConfiguration(new ProductConfiguration());
            modelBuilder.ApplyConfiguration(new ReviewConfiguration());
            modelBuilder.ApplyConfiguration(new OrderConfiguration());
            modelBuilder.ApplyConfiguration(new OrderItemConfiguration());
            modelBuilder.ApplyConfiguration(new StoreUserConfiguration());
        }
    }
}
