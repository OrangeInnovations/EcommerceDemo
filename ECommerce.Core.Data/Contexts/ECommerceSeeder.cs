using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using CommonShares.Extentions;
using ECommerce.Core.Entities;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Newtonsoft.Json;

namespace ECommerce.Core.Data.Contexts
{
    public class ECommerceSeeder
    {
        public static Dictionary<string,int> DictCategory=new Dictionary<string, int>()
        {
            {"Mug" ,1},{"Poster",2},{"T-Shirt",3}
        };

        private readonly ECommerceContext _ctx;
       
        private readonly UserManager<StoreUser> _userManager;

        public ECommerceSeeder(ECommerceContext ctx,UserManager<StoreUser> userManager)
        {
            _ctx = ctx;
            _userManager = userManager;
        }

        public async Task Seed()
        {
            _ctx.Database.EnsureCreated();

            var user = await _userManager.FindByEmailAsync("david.chen@startinnovations.com");

            if (user == null)
            {
                user = new StoreUser()
                {
                    FirstName = "David",
                    LastName = "Chen",
                    UserName = "david.chen@startinnovations.com",
                    Email = "david.chen@startinnovations.com"
                };

                var result = await _userManager.CreateAsync(user, "P@ssw0rd!");
                if (result != IdentityResult.Success)
                {
                    throw new InvalidOperationException("Failed to create default user");
                }
            }

            if (!_ctx.CategorySet.Any())
            {
                _ctx.CategorySet.AddRange(DictCategory.Select(c=>new Category(){CategoryName = c.Key}));
                _ctx.SaveChanges();
            }

            if (!_ctx.ProductSet.Any())
            {
                // Need to create sample data

                var path = PathHelper.AssemblyDirectory;

                var sourceFilePath = Path.Combine(path, @"Data\Products.json");

                var json = File.ReadAllText(sourceFilePath);

                var products = JsonConvert.DeserializeObject<IEnumerable<Product>>(json);

                _ctx.ProductSet.AddRange(products);

                var firstProduct = products.First();

                var order=new Order()
                {
                    User = user,
                    OrderItemList = new List<OrderItem>()
                    {
                        new OrderItem()
                        {
                            Product = firstProduct,
                            Quantity = 10,
                            UnitPrice = firstProduct.Price
                        }
                    }
                };
                _ctx.OrderSet.Add(order);
                _ctx.SaveChanges();
                
            }
        }
    }

    
}
