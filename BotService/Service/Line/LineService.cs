
using BotService.Model.Line;
using BotService.Service.API;
using BotService.Service.Google;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
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
        private readonly IAPIService _apiService;
        private readonly IGoogleService _googleAPIService;

        public LineService(ILogger<LineService> logger, IAPIService apiService, IGoogleService googleAPIService)
        {
            _logger = logger;
            _apiService = apiService;
            _googleAPIService = googleAPIService;
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


                var googleSearchResult = await _googleAPIService.GoogleSearchKeyWord(shop);

                var link1 = googleSearchResult["items"][0]["link"].ToString();
                var link2 = googleSearchResult["items"][1]["link"].ToString();
                messages.Add(new { type = "text", text = link1 });
                messages.Add(new { type = "text", text = link2 });

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
