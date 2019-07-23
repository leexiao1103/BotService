using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using BotService.Model.API;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;

namespace BotService.Service.API
{
    public interface IAPIService
    {
        Task<APIResult<T>> PostAsync<T>(string url, object postData, string clientName = "");
    }

    public class APIService : IAPIService
    {
        private readonly ILogger _logger;
        private readonly IHttpClientFactory _clientFactory;

        public APIService(ILogger<APIService> logger, IHttpClientFactory clientFactory)
        {
            _logger = logger;
            _clientFactory = clientFactory;
        }

        public async Task<APIResult<T>> GetAsync<T>(string url, string clientName = "")
        {
            var result = new APIResult<T>();
            

            return result;
        }

        public async Task<APIResult<T>> PostAsync<T>(string url, object postData, string clientName = "")
        {
            var result = new APIResult<T>();
            if ("/".Equals(url.First()))
                url.Substring(1);

            var request = new HttpRequestMessage(HttpMethod.Post, url);
            request.Content = new StringContent(JsonConvert.SerializeObject(postData), Encoding.UTF8, "application/json");

            var client = _clientFactory.CreateClient(clientName);
            var response = await client.SendAsync(request);
            var content = await response.Content.ReadAsStringAsync();

            result.StatusCode = response.StatusCode;
            result.IsSuccess = response.IsSuccessStatusCode;
            result.Result = JsonConvert.DeserializeObject<T>(content);

            //log機制還沒做，先隨便弄
            _logger.LogInformation(content);

            return result;
        }
    }
}
