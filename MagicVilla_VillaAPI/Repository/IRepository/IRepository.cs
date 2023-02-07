using MagicVilla_VillaAPI.Models;
using System.Linq.Expressions;

namespace MagicVilla_VillaAPI.Repository.IRepository
{
    public interface IRepository<T> where T : class
    {
        Task Create(T entity);
        void Remove(T entity);
        Task<T> Get(Expression<Func<T, bool>> filter = null, bool isTracked = true);
        Task<IEnumerable<T>> GetAll(Expression<Func<T, bool>> filter = null, bool isTracked = true);
        Task Save();
    }
}
