using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace ECommerce.WebAPI.ViewModels
{
    public class OrderViewModel
    {
        public string OrderId { get; set; }

        public DateTimeOffset OrderDateTimeOffset { get; set; }

       
        [MinLength(4)]
        public string OrderNumber { get; set; }
        

        public List<OrderItemViewModel> OrderItemList { get; set; }=new List<OrderItemViewModel>();
    }
}
