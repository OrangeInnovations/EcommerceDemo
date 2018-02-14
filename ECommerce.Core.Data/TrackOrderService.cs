using System.Threading.Tasks;
using ECommerce.Core.Entities;

namespace ECommerce.Core.Data
{
    public class TrackOrderService : ITrackOrderService
    {
        public Task ProcessNewOder(Order newOrder)
        {
            //Send new order to messaage queue such as Azure service bus queue for continue process
            //include check for low stock, and etc.
            
            return Task.CompletedTask;
        }
    }
}