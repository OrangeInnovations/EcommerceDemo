using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ECommerce.WebAPI.ViewModels
{
    public class ProductViewModel 
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public decimal Price { get; set; }
        public int CategoryId { get; set; }
        public string CategoryName { get; set; }

        public List<ReviewViewModel> ReviewList { get; set; } = new List<ReviewViewModel>();
    }
}
