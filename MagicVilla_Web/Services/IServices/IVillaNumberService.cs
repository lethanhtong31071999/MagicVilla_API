using MagicVilla_Web.Models.Dto.VillaNumberDTO;

namespace MagicVilla_Web.Services.IServices
{
    public interface IVillaNumberService : IBaseService
    {
        public Task<T> GetAllAsync<T>();
        public Task<T> GetAsync<T>(int villaNo);
        public Task<T> CreateAsync<T>(VillaNumberCreateDTO villaNumberCreateDTO);
        public Task<T> UpdateAsync<T>(VillaNumberUpdateDTO villaNumberUpdateDTO);
        public Task<T> DeleteAsync<T>(int villNo);
    }
}
