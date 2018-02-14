using System.ComponentModel.DataAnnotations;

namespace ECommerce.WebAPI.ViewModels
{
    public class ReviewCreateModel
    {
        //[Required]
        //public int ProductId { get; set; }

        [Required]
        [MaxLength(1024)]
        public string ReviewContent { get; set; }

        //[Required]
        //public string UserId { get; set; }
    }
}