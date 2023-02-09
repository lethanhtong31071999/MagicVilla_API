using MagicVilla_Web.Models.Dto.VillaDTO;

namespace MagicVilla_Web.Services.IServices
{
    public interface IVillaService : IBaseService
    {
        Task<T> GetAllAsync<T>();
        Task<T> GetAsync<T>(int id);
        Task<T> CreateAsync<T>(VillaDTO villaDTO);
        Task<T> UpdateAsync<T>(VillaUpdateDTO villaUpdateDTO);
        Task<T> DeleteAsync<T>(int id);
    }
}
