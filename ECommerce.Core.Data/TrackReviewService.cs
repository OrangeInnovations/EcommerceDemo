using System.Threading.Tasks;
using ECommerce.Core.Entities;

namespace ECommerce.Core.Data
{
    public class TrackReviewService : ITrackReviewService
    {
        //Send newReview to messaage queue such as Azure service bus queue for continue process
        //include profanity check
        public Task ProcessReview(Review newReview)
        {
            throw new System.NotImplementedException();
        }
    }
}