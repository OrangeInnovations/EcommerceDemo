using System;
using System.Collections.Generic;
using CommonShares.Entities;

namespace ECommerce.Core.Entities
{
    public class Order: Entity<string>
    {
        public override string Id { get; set; } = Guid.NewGuid().ToString();

        public DateTimeOffset OrderDateTimeOffset { get; set; } = DateTimeOffset.UtcNow;


        public string OrderNumber { get; set; } = DateTimeOffset.UtcNow.ToString("yyyyMMddHHmmssfffffff");

        public List<OrderItem> OrderItemList { get; set; }=new List<OrderItem>();

        public string UserId { get; set; } 
        public StoreUser User { get; set; }
    }

}
