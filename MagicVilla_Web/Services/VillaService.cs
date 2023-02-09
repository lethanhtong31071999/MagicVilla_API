﻿using MagicVilla_Utility;
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

        public async Task<T> CreateAsync<T>(VillaDTO villaDTO)
        {
            var apiRequest = new APIRequest()
            {
                APIType = SD.ApiType.POST,
                Url = $"{url}:7001/api/VillaAPI",
                Data = villaDTO
            };
            return await base.SendAsync<T>(apiRequest);
        }

        public async Task<T> DeleteAsync<T>(int id)
        {
            var apiRequest = new APIRequest()
            {
                APIType = SD.ApiType.DELETE,
                Url = $"{url}:7001/api/VillaAPI/{id}",
            };
            return await base.SendAsync<T>(apiRequest);
        }

        public async Task<T> GetAllAsync<T>()
        {
            var apiRequest = new APIRequest()
            {
                APIType = SD.ApiType.GET,
                Url = $"{url}:7001/api/VillaAPI",
            };
            return await base.SendAsync<T>(apiRequest);
        }

        public async Task<T> GetAsync<T>(int id)
        {
            var apiRequest = new APIRequest()
            {
                APIType = SD.ApiType.GET,
                Url = $"{url}:7001/api/VillaAPI/{id}",
            };
            return await base.SendAsync<T>(apiRequest);
        }

        public async Task<T> UpdateAsync<T>(VillaUpdateDTO villaUpdateDTO)
        {
            var apiRequest = new APIRequest()
            {
                APIType = SD.ApiType.PUT,
                Url = $"{url}:7001/api/VillaAPI/{villaUpdateDTO.Id}",
                Data = villaUpdateDTO
            };
            return await base.SendAsync<T>(apiRequest);
        }
    }
}
