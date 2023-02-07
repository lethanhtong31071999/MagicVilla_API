using MagicVilla_VillaAPI.Data;
using MagicVilla_VillaAPI.Models;
using MagicVilla_VillaAPI.Repository.IRepository;

namespace MagicVilla_VillaAPI.Repository
{
    public class VillaRepo : Repository<Villa>, IVillaRepo
    {
        private readonly ApplicationDbContext _db;
        public VillaRepo(ApplicationDbContext db) : base(db)
        {
            _db= db;
        }

        public void Update(Villa entity)
        {
            _db.Update(entity);
        }
    }
}
