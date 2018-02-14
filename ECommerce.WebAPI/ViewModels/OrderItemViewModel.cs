using System.ComponentModel.DataAnnotations;

namespace ECommerce.WebAPI.ViewModels
{
    public class OrderItemViewModel
    {
        public string Id { get; set; }

        
        public int ProductId { get; set; }
        public string ProductTitle { get; set; }

        public string CategoryName { get; set; }

        [Required]
        public int Quantity { get; set; }

        [Required]
        public decimal UnitPrice { get; set; }
    }
}