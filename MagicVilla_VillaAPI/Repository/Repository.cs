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

        private IQueryable<T> GetIncludedProps(IQueryable<T> query, string includedProps)
        {
            var props = includedProps.Split(new char[] {','}, StringSplitOptions.RemoveEmptyEntries)
                .Select(x => x[0].ToString().ToUpper() + x.Substring(1).ToLower())
                .ToList();
            foreach (var prop in props)
            {
                query = query.Include(prop);
            }
            var test = query.ToList();
            return query;
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

        async public Task<T> Get(Expression<Func<T, bool>> filter = null, bool isTracked = true, string? includedProps = null)
        {
            IQueryable<T> query = _dbSet;
            if(filter != null)
            {
                query = query.Where(filter);
            }
            if(!isTracked) query = query.AsNoTracking();
            if(includedProps!= null)
            {
                query = GetIncludedProps(query, includedProps);
            }
            return await query.FirstOrDefaultAsync<T>();
        }

        async public Task<IEnumerable<T>> GetAll(Expression<Func<T, bool>> filter = null, bool isTracked = true, string? includedProps = null)
        {
            IQueryable<T> query = _dbSet;
            if (filter != null)
            {
                query = query.Where(filter);
            }
            if (!isTracked) query = query.AsNoTracking();
            if (includedProps != null)
            {
                query = GetIncludedProps(query, includedProps);
            }
            return await query.ToListAsync<T>();
        }
    }
}
