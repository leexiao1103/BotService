using BotService.Model.API;
using BotService.Service.API;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BotService.Service.Google
{
    public interface IGoogleService
    {
        Task<JObject> GoogleSearchKeyWord(string keyword);
    }

    public class GoogleService : IGoogleService
    {
        private readonly IAPIService _apiService;
        private readonly IConfiguration _configuration;

        public GoogleService(IAPIService apiService, IConfiguration configuration)
        {
            _apiService = apiService;
            _configuration = configuration;
        }

        public async Task<JObject> GoogleSearchKeyWord(string keyword)
        {
            string appKey = _configuration.GetValue<string>("Google:APP_Key");
            string engineId = _configuration.GetValue<string>("Google:Engine_Id");
            //Google回傳物件沒建
            //應該要從DB拿key
            string url = $"?key={appKey}&cx={engineId}&q={keyword}";
            var response = await _apiService.GetAsync<JObject>(url, "GoogleCustomSearchAPI");
            var resultItems = response.Result;

            return resultItems;
        }
    }
}
