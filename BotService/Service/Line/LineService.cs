
using BotService.Model.Line;
using BotService.Service.API;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace BotService.Service.Line
{
    public interface ILineService
    {
        Task HandleEventAsync(LineWebhookEvent hookEvent);
    }

    public class LineService : ILineService
    {
        private readonly ILogger _logger;
        private readonly IHttpClientFactory _clientFactory;
        private readonly IAPIService _apiService;

        public LineService(IHttpClientFactory clientFactory, ILogger<LineService> logger, IAPIService apiService)
        {
            _logger = logger;
            _clientFactory = clientFactory;
            _apiService = apiService;
        }

        public async Task HandleEventAsync(LineWebhookEvent hookEvent)
        {
            var messageContent = hookEvent.Events[0].Message;
            bool isReply = messageContent.Type.Equals("text") && messageContent.Text.Equals("來家拉麵吧");

            if (isReply)
            {
                //之後要拉出去
                var emojiEat = char.ConvertFromUtf32(0x100093);
                var emojiKiss = char.ConvertFromUtf32(0x100096);
                var emojiShineEye = char.ConvertFromUtf32(0x10007A);
                var emojiLaugh = char.ConvertFromUtf32(0x10009D);
                var noodleList = new List<string>() {
                    "鬼金棒", "壹之穴", "豚人", "麵屋輝", "麵屋緣", "半熟堂", "油組",
                    "Soba Shinee & 柑橘", "美濃屋", "勝王", "麵屋壹慶", "悠然", "鷹流",
                    "蘭丸", "極匠", "小櫻", "涼風庵", "勝千代", "吉天元", "你回來啦",
                    "雞吉君", "雞二", "小川", "真登", "真劍", "神神神神神", "誠屋",
                    "道樂屋台", "羽X食堂", "大和家", "麵屋武藏", "一幻", "NAGI",
                    "花月嵐", "麵屋一登", "旺味麵場", "武藤", "山嵐拉麵", "特濃屋",
                    "麵屋山茶", " 麵屋一騎", "小山拉麵", "通堂", "屯京拉麵", "山頭火",
                    "霸嗎", "双豚", "森住康二", "一番星", "北一家", "熊越岳", "DUE ITALIAN",
                    "太陽番茄麵", "玩笑亭", "博多幸籠", "初", "一風堂"
                };

                var messages = new List<object>();
                var shop = noodleList.OrderBy(_ => Guid.NewGuid()).First();
                messages.Add(new { type = "text", text = $"吃{shop}啦{emojiEat}{emojiKiss}{emojiShineEye}{emojiLaugh}" });


                var request = new HttpRequestMessage(HttpMethod.Get, $"?key=AIzaSyCNX2TgNgk5_v7uoGlJT5GNGfeqHPbz0DM&cx=010662344843333467486:6uuqmv5wzkk&q={shop}");
                var client = _clientFactory.CreateClient("GoogleCustomSearchAPI");
                var response = await client.SendAsync(request);

                if (response.IsSuccessStatusCode)
                {
                    var result = await response.Content.ReadAsStringAsync();
                    var resultItems = ((JObject)JsonConvert.DeserializeObject(result))["items"];
                    var link1 = resultItems[0]["link"].ToString();
                    var link2 = resultItems[1]["link"].ToString();
                    messages.Add(new { type = "text", text = link1 });
                    messages.Add(new { type = "text", text = link2 });
                }
                else
                {
                    var result = await response.Content.ReadAsStringAsync();
                    _logger.LogInformation($"Error: {result}");
                }

                await ReplyMessage(hookEvent.Events[0].ReplyToken, messages);
            }
        }

        public async Task ReplyMessage(string replyToken, List<object> messages)
        {
            //Line 回傳物件還沒建
            var result = await _apiService.PostAsync<object>("reply", new { replyToken, messages }, "LineMessageAPI");

            if (!result.IsSuccess)
            {
                //傳送失敗後行為未實做 => 應該要push message
            }
        }
    }
}
