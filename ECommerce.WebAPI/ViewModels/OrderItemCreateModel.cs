using System.ComponentModel.DataAnnotations;

namespace ECommerce.WebAPI.ViewModels
{
    public class OrderItemCreateModel
    {

        [Required]
        public int ProductId { get; set; }
        

        [Required]
        public int Quantity { get; set; }

        [Required]
        public decimal UnitPrice { get; set; }



    }
}