using MagicVilla_Utility;
using MagicVilla_Web.Models;
using MagicVilla_Web.Models.Dto.VillaNumberDTO;
using MagicVilla_Web.Services.IServices;
using Microsoft.AspNetCore.Http.HttpResults;

namespace MagicVilla_Web.Services
{
    public class VillaNumberService : BaseService, IVillaNumberService
    {
        private readonly IHttpClientFactory _httpClientFactory;
        private readonly string url;
        public VillaNumberService(IHttpClientFactory httpClientFactory, IConfiguration configuration) : base(httpClientFactory)
        {
            _httpClientFactory = httpClientFactory;
            url = configuration.GetValue<string>("ServiceUrls:VillaAPI");
        }

        public async Task<T> CreateAsync<T>(VillaNumberCreateDTO villaNumberCreateDTO)
        {
            var apiRequest = new APIRequest()
            {
                Data = villaNumberCreateDTO,
                APIType = SD.ApiType.POST,
                Url = $"{url}/api/VillaNumberAPI",
            };
            return await base.SendAsync<T>(apiRequest);
        }

        public async Task<T> DeleteAsync<T>(int villNo)
        {
            var apiRequest = new APIRequest()
            {
                APIType = SD.ApiType.DELETE,
                Url = $"{url}/api/VillaNumberAPI/{villNo}",
            };
            return await base.SendAsync<T>(apiRequest);
        }

        public async Task<T> GetAllAsync<T>()
        {
            var apiRequest = new APIRequest()
            {
                APIType = SD.ApiType.GET,
                Url = $"{url}/api/VillaNumberAPI",
            };
            return await base.SendAsync<T>(apiRequest);
        }

        public async Task<T> GetAsync<T>(int villaNo)
        {
            var apiRequest = new APIRequest()
            {
                APIType = SD.ApiType.GET,
                Url = $"{url}/api/VillaNumberAPI/{villaNo}",
            };
            return await base.SendAsync<T>(apiRequest);
        }

        public async Task<T> UpdateAsync<T>(VillaNumberUpdateDTO villaNumberUpdateDTO)
        {
            var apiRequest = new APIRequest()
            {
                Data = villaNumberUpdateDTO,
                APIType = SD.ApiType.PUT,
                Url = $"{url}/api/VillaNumberAPI/{villaNumberUpdateDTO.VillaNo}",
            };
            return await base.SendAsync<T>(apiRequest);
        }
    }
}
