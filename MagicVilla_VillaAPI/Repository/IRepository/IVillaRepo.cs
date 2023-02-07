using MagicVilla_VillaAPI.Models;

namespace MagicVilla_VillaAPI.Repository.IRepository
{
    public interface IVillaRepo : IRepository<Villa>
    {
        void Update(Villa entity);

    }
}
