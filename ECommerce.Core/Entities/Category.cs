using System.Collections.Generic;
using CommonShares.Entities;

namespace ECommerce.Core.Entities
{
    public class Category : Entity<int>
    {
        public string CategoryName { get; set; }

        public List<Product> ProductList { get; set; }=new List<Product>();
    }
}