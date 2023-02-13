using MagicVilla_Utility;
using MagicVilla_Web.Models;
using MagicVilla_Web.Models.Dto.ApplicationUserDTO;
using MagicVilla_Web.Services.IServices;

namespace MagicVilla_Web.Services
{
    public class AuthService : BaseService, IAuthService
    {
        private readonly IHttpClientFactory _httpClientFactory;
        private readonly string url;
        public AuthService(IHttpClientFactory httpClientFactory, IConfiguration configuration) : base(httpClientFactory)
        {
            _httpClientFactory = httpClientFactory;
            url = configuration.GetValue<string>("ServiceUrls:VillaAPI") ?? "";
        }

        public async Task<T> Login<T>(LoginRequestDTO obj)
        {
            var apiRequest = new APIRequest()
            {
                APIType = SD.ApiType.POST,
                Data = obj,
                Url = $"{url}/api/UsersAuth/login",
            };
            return await base.SendAsync<T>(apiRequest);
        }

        public async Task<T> Register<T>(RegisterRequestDTO obj)
        {
            var apiRequest = new APIRequest()
            {
                APIType = SD.ApiType.POST,
                Data = obj,
                Url = $"{url}/api/UsersAuth/register",
            };
            return await base.SendAsync<T>(apiRequest);
        }
    }
}
