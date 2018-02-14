using System;
using CommonShares.Entities;

namespace ECommerce.Core.Entities
{
  public class OrderItem : Entity<string>
    {
        public override string Id { get; set; } = Guid.NewGuid().ToString();

        public int ProductId { get; set; }

        public Product Product { get; set; }
        public int Quantity { get; set; }
        public decimal UnitPrice { get; set; }

        public string OrderId { get; set; } 
        public Order Order { get; set; }
  }
}