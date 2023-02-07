using MagicVilla_VillaAPI.Data;
using MagicVilla_VillaAPI.Repository.IRepository;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Linq.Expressions;

namespace MagicVilla_VillaAPI.Repository
{
    public class Repository<T> : IRepository<T> where T : class
    {
        private readonly ApplicationDbContext _db;
        private readonly DbSet<T> _dbSet;
        public Repository(ApplicationDbContext db)
        {
            _db = db;
            _dbSet = _db.Set<T>();
        }

        async public Task Create(T entity)
        {
            await _db.AddAsync(entity);
        }

        public void Remove(T entity)
        {
            _db.Remove(entity);
        }

        async public Task Save()
        {
            await _db.SaveChangesAsync();
        }

        async public Task<T> Get(Expression<Func<T, bool>> filter = null, bool isTracked = true)
        {
            IQueryable<T> query = _dbSet;
            if(filter != null)
            {
                query = query.Where(filter);
            }
            if(!isTracked) query = query.AsNoTracking();
            return await query.FirstOrDefaultAsync<T>();
        }

        async public Task<IEnumerable<T>> GetAll(Expression<Func<T, bool>> filter = null, bool isTracked = true)
        {
            IQueryable<T> query = _dbSet;
            if (filter != null)
            {
                query = query.Where(filter);
            }
            if (!isTracked) query = query.AsNoTracking();
            return await query.ToListAsync<T>();
        }
    }
}
