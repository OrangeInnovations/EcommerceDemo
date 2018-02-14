using System.Threading.Tasks;
using ECommerce.Core.Entities;

namespace ECommerce.Core.Data
{
    public interface ITrackOrderService
    {
        Task ProcessNewOder(Order newOrder);
    }
}