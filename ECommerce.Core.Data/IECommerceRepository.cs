using System.Collections.Generic;
using System.Threading.Tasks;
using ECommerce.Core.Entities;

namespace ECommerce.Core.Data
{
    public interface IECommerceRepository
    {
        IEnumerable<Product> GetAllProducts(bool includeReviews);
        Product GetProductById(int id, bool includeReviews);

        IEnumerable<Product> GetProductsByCategory(string category);

        IEnumerable<Order> GetAllOrders(bool includeItems);
        IEnumerable<Order> GetAllOrdersByUser(string username, bool includeItems);
        //IEnumerable<Order> GetAllOrdersByUserId(string userId, bool includeItems);
        Order GetOrderById(string username, string id);
        Order GetOrderByUserId(string userID, string id);

        void AddOrder(Order newOrder);

        bool SaveAll();
        void AddEntity(object model);

        Task<List<Review>> GetReviewsAsync(int productId);
        Task<Review> GetReviewAsync(string reviwId);
        void AddReview(Review review);
        Task<bool> SaveAllAsync();
    }
}