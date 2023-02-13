using MagicVilla_Web.Models.Dto.VillaDTO;

namespace MagicVilla_Web.Services.IServices
{
    public interface IVillaService : IBaseService
    {
        Task<T> GetAllAsync<T>(string? token = null);
        Task<T> GetAsync<T>(int id, string? token = null);
        Task<T> CreateAsync<T>(VillaCreateDTO villaDTO, string? token = null);
        Task<T> UpdateAsync<T>(VillaUpdateDTO villaUpdateDTO, string? token = null);
        Task<T> DeleteAsync<T>(int id, string? token = null);
    }
}
