using System.Collections.Generic;
using Microsoft.AspNetCore.Identity;

namespace ECommerce.Core.Entities
{
    public class StoreUser : IdentityUser
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }

        public string FullName => $"{LastName}, {FirstName}";


        public List<Review> ReviewList { get; set; }=new List<Review>();

        public List<Order> OrderList { get; set; }=new List<Order>();
    }
}
