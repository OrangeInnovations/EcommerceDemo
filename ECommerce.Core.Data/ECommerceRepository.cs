using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ECommerce.Core.Data.Contexts;
using ECommerce.Core.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace ECommerce.Core.Data
{
    public class ECommerceRepository : IECommerceRepository
    {
        private readonly ECommerceContext _ctx;
        private readonly ILogger<ECommerceRepository> _logger;

        public ECommerceRepository(ECommerceContext ctx, ILogger<ECommerceRepository> logger)
        {
            _ctx = ctx;
            _logger = logger;
        }

        public void AddEntity(object model)
        {
            _ctx.Add(model);
        }


        public void AddOrder(Order newOrder)
        {

            // Convert new products to lookup of product
            //foreach (var item in newOrder.OrderItemList)
            //{
            //    item.Product = _ctx.ProductSet.Find(item.Product.Id);
            //}

            _ctx.OrderSet.Add(newOrder);
            //AddEntity(newOrder);
        }

        public IEnumerable<Order> GetAllOrders(bool includeItems)
        {
            if (includeItems)
            {

                return _ctx.OrderSet
                           .Include(o => o.OrderItemList)
                           .ThenInclude(i => i.Product).ThenInclude(c => c.Category)
                           .ToList();

            }
            else
            {
                return _ctx.OrderSet
                           .ToList();
            }
        }

        //public IEnumerable<Order> GetAllOrdersByUserId(string userId, bool includeItems)
        //{
        //    if (includeItems)
        //    {

        //        return _ctx.OrderSet
        //            .Where(o => o.UserId == userId)
        //            .Include(o => o.OrderItemList)
        //            .ThenInclude(i => i.Product).ThenInclude(c => c.Category)
        //            .ToList();

        //    }
        //    else
        //    {
        //        return _ctx.OrderSet
        //            .Where(o => o.UserId == userId)
        //            .ToList();
        //    }
        //}


        public IEnumerable<Order> GetAllOrdersByUser(string username, bool includeItems)
        {
            if (includeItems)
            {

                return _ctx.OrderSet
                           .Where(o => o.User.UserName == username)
                           .Include(o => o.OrderItemList)
                           .ThenInclude(i => i.Product).ThenInclude(c=>c.Category)
                           .ToList();

            }
            else
            {
                return _ctx.OrderSet
                           .Where(o => o.User.UserName == username)
                           .ToList();
            }
        }

        public Product GetProductById(int id, bool includeReviews)
        {
            try
            {
                if (includeReviews)
                {
                    return _ctx.ProductSet.Include(c => c.Category)
                        .Include(c => c.ReviewList).ThenInclude(o => o.User)
                        .FirstOrDefault(c => c.Id == id);
                }
                else
                {
                    return _ctx.ProductSet.Include(c => c.Category)
                        .FirstOrDefault(c => c.Id == id);
                }
            }
            catch (Exception ex)
            {
                _logger.LogError($"Failed to get product: {ex}");
                return null;
            }
        }
        public IEnumerable<Product> GetAllProducts(bool includeReviews)
        {
            try
            {
                //_logger.LogInformation("GetAllProducts was called");
                if (includeReviews)
                {
                    return _ctx.ProductSet.Include(c => c.Category)
                        .Include(c => c.ReviewList).ThenInclude(o => o.User)
                        .OrderBy(p => p.Title)
                        .ToList();
                }
                else
                {
                    return _ctx.ProductSet.Include(c => c.Category)
                        .OrderBy(p => p.Title)
                        .ToList();
                }
                
            }
            catch (Exception ex)
            {
                _logger.LogError($"Failed to get all products: {ex}");
                return null;
            }
        }

        public Order GetOrderById(string username, string id)
        {
            return _ctx.OrderSet
                       .Include(o => o.OrderItemList)
                       .ThenInclude(i => i.Product).ThenInclude(c => c.Category)
                       .Where(o => o.Id == id && o.User.UserName == username)
                       .FirstOrDefault();
        }

        public Order GetOrderByUserId(string userID, string id)
        {
            return _ctx.OrderSet
                .Include(o => o.OrderItemList)
                .ThenInclude(i => i.Product).ThenInclude(c => c.Category)
                .Where(o => o.Id == id && o.UserId==userID)
                .FirstOrDefault();
        }

        public IEnumerable<Product> GetProductsByCategory(string category)
        {
            return _ctx.ProductSet
                       .Where(p => p.Category.CategoryName == category)
                       .ToList();
        }


        public async Task<List<Review>> GetReviewsAsync(int productId)
        {
            return await _ctx.ReviewSet.Where(c => c.ProductId == productId)
                .Include(c=>c.Product).Include(c=>c.User)
                .OrderByDescending(c=>c.CreateDateTimeOffset).ToListAsync();
        }



        public async Task<Review> GetReviewAsync(string reviwId)
        {
            return await _ctx.ReviewSet.Include(c => c.Product).Include(c => c.User)
                .FirstOrDefaultAsync(c => c.Id == reviwId);

        }


        public void AddReview(Review review)
        {
            _ctx.ReviewSet.Add(review);
        }

        public async Task<bool> SaveAllAsync()
        {
            return  await _ctx.SaveChangesAsync() > 0;
        }

        public bool SaveAll()
        {
            return _ctx.SaveChanges() > 0;
        }
    }
}