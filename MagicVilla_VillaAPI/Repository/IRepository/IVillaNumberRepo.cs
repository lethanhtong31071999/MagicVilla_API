using MagicVilla_VillaAPI.Data;
using MagicVilla_VillaAPI.Models;

namespace MagicVilla_VillaAPI.Repository.IRepository
{
    public interface IVillaNumberRepo : IRepository<VillaNumber>
    {
        public void Update(VillaNumber entity);
    }
}
