using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace CommonShares.Entities
{
    public interface IRepository<T, TId> : IDisposable
     where T : class, IEntity<TId>
    {



        #region syn functions
       // DbSet<T> DbSet { get; }

        bool Any(Expression<Func<T, bool>> predicate);

        T FindById(TId id);

        IEnumerable<T> FindAll();

        IEnumerable<T> Find(Expression<Func<T, bool>> predicate);
        T FirstOrDefault(Expression<Func<T, bool>> match);

        int Count();

        IQueryable<T> FindQueryable(Expression<Func<T, bool>> predicate);

        void Add(T item);

        void AddRange(IEnumerable<T> items);

        void TrackItem(T item);
        void Modify(T item);
        void Remove(TId id);
        void Remove(T item);
        void Merge(T persisted, T current);

        Task<int> CountAsync();
        int SaveChanges();
        int ExecuteCommand(string sql, params Object[] parameters);

        void RemoveRange(Expression<Func<T, bool>> predicate);
        #endregion
        #region asyn functions
        Task<int> ExecuteCommandAsync(string sql, params Object[] parameters);

        Task<bool> AnyAsync(Expression<Func<T, bool>> predicate);

        Task<T> FindByIdAsync(TId id);

        Task<T> FirstOrDefaultAsync(Expression<Func<T, bool>> match);

        Task<ICollection<T>> GetAllAsync();
        Task<ICollection<T>> FindAsync(Expression<Func<T, bool>> predicate);

        Task RemoveAsync(TId id);

        Task<int> SaveChangesAsync();

        #endregion


    }
}
