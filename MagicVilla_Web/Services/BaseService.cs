using MagicVilla_Utility;
using MagicVilla_Web.Models;
using MagicVilla_Web.Services.IServices;
using Newtonsoft.Json;
using System.Net;
using System.Text;

namespace MagicVilla_Web.Services
{
    public class BaseService : IBaseService
    {
        private readonly IHttpClientFactory _httpClientFactory;

        public BaseService(IHttpClientFactory httpClientFactory)
        {
            _httpClientFactory = httpClientFactory;
        }

        public async Task<T> SendAsync<T>(APIRequest apiRequest)
        {
            try
            {
                var client = _httpClientFactory.CreateClient("MagicVillaAPI");
                var requestMessage = new HttpRequestMessage();
                /* Request-Message includes 4 main properties: 
                    1. Url
                    2. Header
                    3. Body (content)
                    4. Method
                 */

                // Before call API
                requestMessage.Headers.Add("Accecpt", "application/json");
                requestMessage.RequestUri = new Uri(apiRequest.Url);
                if (apiRequest.Data != null)
                {
                    requestMessage.Content = new StringContent(
                        JsonConvert.SerializeObject(apiRequest.Data),
                        Encoding.UTF8,
                        "application/json");
                }
                switch (apiRequest.APIType)
                {
                    case SD.ApiType.POST:
                        requestMessage.Method = HttpMethod.Post;
                        break;
                    case SD.ApiType.PUT:
                        requestMessage.Method = HttpMethod.Put;
                        break;
                    case SD.ApiType.DELETE:
                        requestMessage.Method = HttpMethod.Delete;
                        break;
                    default:
                        requestMessage.Method = HttpMethod.Get;
                        break;
                }

                // Call API and After call
                /*
                 Content from response is the APIResponse class in the Server
                 APIResponse include:
                    1. Data
                    2. StatusCode
                    3. IsSuccess
                    4. ErrorMessages
                 */
                HttpResponseMessage responseMessage = null;
                responseMessage = await client.SendAsync(requestMessage);
                var responseContent = await responseMessage.Content.ReadAsStringAsync();
                return JsonConvert.DeserializeObject<T>(responseContent); // APIResponse
            }
            catch (Exception ex)
            {
                var response = new APIResponse()
                {
                    StatusCode = HttpStatusCode.InternalServerError,
                    IsSuccess = false,
                    ErrorMessages = new List<string> { Convert.ToString(ex.Message) }
                };
                // We cannot return directly APIResponse class because we use T class
                // Solution is Parse to JSON and then Reparse to T model
                // If T model exist in JSON => Ok()
                // but if it's not => APIResponse (here always result because we don't have any T model)
                return JsonConvert.DeserializeObject<T>((JsonConvert.SerializeObject(response)));
            }
        }
    }
}
