using Core.Abstracts.Bases;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace Core.Utils
{
    public class Repository<T> : IRepository<T> where T : BaseEntity
    {
        private readonly DbContext _context;
        private readonly DbSet<T> _set;

        public Repository(DbContext context)
        {
            _context = context;
            _set = _context.Set<T>();
        }

        public async Task<bool> AnyAsync(Expression<Func<T, bool>>? where = null) => await _set.AnyAsync(where ?? (x => true));

        public async Task<int> CountAsync(Expression<Func<T, bool>>? where = null) => await _set.CountAsync(where ?? (x => true));

        public async Task CreateManyAsync(IEnumerable<T> entities) => await _set.AddRangeAsync(entities);

        public async Task CreateOneAsync(T entity) => await _set.AddAsync(entity);

        public void DeleteMany(IEnumerable<T> entities) => _set.RemoveRange(entities);

        public void DeleteMany(Expression<Func<T, bool>>? where = null)
        {
            var entities = _set.Where(where ?? (x => true));
            DeleteMany(entities);
        }

        public void DeleteOne(T entity) => _set.Remove(entity);

        public void DeleteOne(object key)
        {
            var entity = _set.Find(key);
            if (entity != null)
            {
                DeleteOne(entity);
            }
        }

        public IQueryable<T> Query() => _set.AsQueryable();

        public async Task<T?> ReadByIdAsync(object key) => await _set.FindAsync(key);

        public async Task<IEnumerable<T>> ReadManyAsync(Expression<Func<T, bool>>? where = null) => await _set.Where(where ?? (x => true)).ToListAsync();

        public void UpdateMany(IEnumerable<T> entities) => _set.UpdateRange(entities);

        public void UpdateOne(T entity) => _set.Update(entity);
    }
}
