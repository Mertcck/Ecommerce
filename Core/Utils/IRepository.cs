using Core.Abstracts.Bases;
using System.Linq.Expressions;

namespace Core.Utils
{
    public interface IRepository<T> where T : BaseEntity
    {
        Task CreateOneAsync(T entity);
        Task CreateManyAsync(IEnumerable<T> entities);

        Task<T?> ReadByIdAsync(object key); // key: Entity/Primary Key
        Task<IEnumerable<T>> ReadManyAsync(Expression<Func<T, bool>>? where = null);
        IQueryable<T> Query();

        void UpdateOne(T entity);
        void UpdateMany(IEnumerable<T> entities);

        void DeleteOne(T entity);
        void DeleteOne(object key);
        void DeleteMany(IEnumerable<T> entities);
        void DeleteMany(Expression<Func<T, bool>>? where = null);

        Task<bool> AnyAsync(Expression<Func<T, bool>>? where = null);
        Task<int> CountAsync(Expression<Func<T, bool>>? where = null);
    }
}
