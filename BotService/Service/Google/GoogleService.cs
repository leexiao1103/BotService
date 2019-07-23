using BotService.Model.API;
using BotService.Service.API;
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

        public GoogleService(IAPIService apiService)
        {
            _apiService = apiService;
        }

        public async Task<JObject> GoogleSearchKeyWord(string keyword)
        {
            //Google回傳物件沒建
            //應該要從DB拿key
            string url = $"?key=AIzaSyCNX2TgNgk5_v7uoGlJT5GNGfeqHPbz0DM&cx=010662344843333467486:6uuqmv5wzkk&q={keyword}";
            var response = await _apiService.GetAsync<JObject>(url, "GoogleCustomSearchAPI");
            var resultItems = response.Result;

            return resultItems;
        }
    }
}
