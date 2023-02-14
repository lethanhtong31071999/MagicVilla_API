using MagicVilla_Utility;
using MagicVilla_Web.Models;
using MagicVilla_Web.Models.Dto.VillaDTO;
using MagicVilla_Web.Services.IServices;

namespace MagicVilla_Web.Services
{
    public class VillaService : BaseService, IVillaService
    {
        private readonly IHttpClientFactory _httpClientFactory;
        private readonly string url;
        public VillaService(IHttpClientFactory httpClientFactory, IConfiguration configuration) : base(httpClientFactory)
        {
            _httpClientFactory = httpClientFactory;
            url = configuration.GetValue<string>("ServiceUrls:VillaAPI");
        }

        public async Task<T> CreateAsync<T>(VillaCreateDTO villaCreateDTO, string? token = null)
        {
            var apiRequest = new APIRequest()
            {
                APIType = SD.ApiType.POST,
                Url = $"{url}/api/VillaAPI",
                Data = villaCreateDTO,
                Token = token
            };
            return await base.SendAsync<T>(apiRequest);
        }

        public async Task<T> DeleteAsync<T>(int id, string? token = null)
        {
            var apiRequest = new APIRequest()
            {
                APIType = SD.ApiType.DELETE,
                Url = $"{url}/api/VillaAPI/{id}",
                Token = token
            };
            return await base.SendAsync<T>(apiRequest);
        }

        public async Task<T> GetAllAsync<T>(string? token = null)
        {
            var apiRequest = new APIRequest()
            {
                APIType = SD.ApiType.GET,
                Url = $"{url}/api/VillaAPI",
                Token = token
            };
            return await base.SendAsync<T>(apiRequest);
        }

        public async Task<T> GetAsync<T>(int id, string? token = null)
        {
            var apiRequest = new APIRequest()
            {
                APIType = SD.ApiType.GET,
                Url = $"{url}/api/VillaAPI/{id}",
                Token = token
            };
            return await base.SendAsync<T>(apiRequest);
        }

        public async Task<T> UpdateAsync<T>(VillaUpdateDTO villaUpdateDTO, string? token = null)
        {
            var apiRequest = new APIRequest()
            {
                APIType = SD.ApiType.PUT,
                Url = $"{url}/api/VillaAPI/{villaUpdateDTO.Id}",
                Data = villaUpdateDTO,
                Token = token
            };
            return await base.SendAsync<T>(apiRequest);
        }
    }
}
