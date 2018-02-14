using System.Threading.Tasks;
using ECommerce.Core.Entities;

namespace ECommerce.Core.Data
{
    public interface ITrackReviewService
    {
        Task ProcessReview(Review newReview);
    }
}