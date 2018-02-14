using System.Collections.Generic;
using CommonShares.Entities;

namespace ECommerce.Core.Entities
{
    public class Product : Entity<int>
    {
        public string Title { get; set; }
        public string Description { get; set; } 
        public decimal Price { get; set; }

        public int CategoryId { get; set; }
        public Category Category { get; set; } 

        public List<Review> ReviewList { get; set; }=new List<Review>();

    }
}
