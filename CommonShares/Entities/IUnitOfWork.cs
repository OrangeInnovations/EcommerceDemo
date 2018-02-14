using System;
using System.Threading.Tasks;

namespace CommonShares.Entities
{
    public interface IUnitOfWork : IDisposable
    {
        //DbContext DbContext { get; }

        Task<int> SaveChangesAsync();
        int SaveChanges();

        IRepository<T, TId> Repository<T, TId>() where T : class, IEntity<TId>;

        //DbSet<T> GetDbSet<T>() where T : class;
    }
}
