using MagicVilla_Web.Models.Dto.VillaNumberDTO;

namespace MagicVilla_Web.Services.IServices
{
    public interface IVillaNumberService : IBaseService
    {
        public Task<T> GetAllAsync<T>(string? token = null);
        public Task<T> GetAsync<T>(int villaNo, string? token = null);
        public Task<T> CreateAsync<T>(VillaNumberCreateDTO villaNumberCreateDTO, string? token = null);
        public Task<T> UpdateAsync<T>(VillaNumberUpdateDTO villaNumberUpdateDTO, string? token = null);
        public Task<T> DeleteAsync<T>(int villNo, string? token = null);
    }
}
