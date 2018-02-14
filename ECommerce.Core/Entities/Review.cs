using System;
using CommonShares.Entities;

namespace ECommerce.Core.Entities
{
    public class Review : Entity<string>
    {
        public override string Id { get; set; } = Guid.NewGuid().ToString();

        public string ReviewContent { get; set; }

        public int ProductId { get; set; }

        public Product Product { get; set; }

        public string UserId { get; set; } 
        public StoreUser User { get; set; }

        public DateTimeOffset CreateDateTimeOffset { get; set; } = DateTimeOffset.UtcNow;
    }


}