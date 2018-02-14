using System.Collections.Generic;

namespace ECommerce.WebAPI.ViewModels
{
    public class OrderCreateModel
    {
        public List<OrderItemCreateModel> OrderItemList { get; set; } = new List<OrderItemCreateModel>();
    }
}