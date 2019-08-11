using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BotService.Model.Line;
using BotService.Service.API;
using BotService.Service.Google;
using Microsoft.Extensions.Logging;

namespace BotService.Service.Line
{
    public interface ILineService
    {
        void InitServiceData(LineEvent lineEvent);
        Task HandleEventAsync();
    }

    public class LineService : ILineService
    {
        private readonly ILogger _logger;
        private readonly IAPIService _apiService;
        private readonly IGoogleService _googleAPIService;
        private readonly ILineDBService _lineDBService;
        private readonly ILineEmoji _lineEmoji;
        private List<ChatSetting> _chatSetting;
        private string _replyToken;
        private EventType _type;
        private SourceType _sourceType;
        private string _chatId;
        private Message _message;


        public LineService(ILogger<LineService> logger, IAPIService apiService, IGoogleService googleAPIService, ILineDBService lineDBService, ILineEmoji lineEmoji)
        {
            _logger = logger;
            _apiService = apiService;
            _googleAPIService = googleAPIService;
            _lineDBService = lineDBService;
            _lineEmoji = lineEmoji;
        }

        public void InitServiceData(LineEvent lineEvent)
        {
            //Set reply token                       
            _replyToken = lineEvent.ReplyToken;

            //Set type
            _type = lineEvent.Type;

            //Set source type and chat id(room id, group id, user id)
            SetChatData(lineEvent.Source);

            //Set message
            _message = lineEvent.Message;
        }

        public async Task HandleEventAsync()
        {
            switch (_type)
            {
                case EventType.Message:
                    bool isSet = await SetChatSetting();
                    if (isSet)
                        await HandleMessage();
                    else
                        await InsertChatData(true);
                    break;

                case EventType.Follow:
                case EventType.Join:
                    await InsertChatData();
                    break;

                case EventType.Unfollow:
                case EventType.Leave:
                    await _lineDBService.DeleteAsync(_chatId);
                    break;
                    //case EventType.Postback:
                    //    break;
                    //case EventType.Beacon:
                    //    break;                
            }
        }







        #region Private

        private async Task ReplyMessage<T>(List<T> messages)
        {
            var result = await _apiService.PostAsync<object>("reply", new { replyToken = _replyToken, messages }, "LineMessageAPI");

            if (!result.IsSuccess)
            {
                //傳送失敗後行為未實做 => 應該要push message
            }
        }

        private void SetChatData(Source source)
        {
            _sourceType = source.Type;
            switch (source.Type)
            {
                case SourceType.User:
                    _chatId = source.UserId;
                    break;
                case SourceType.Group:
                    _chatId = source.GroupId;
                    break;
                case SourceType.Room:
                    _chatId = source.RoomId;
                    break;
            }
        }

        private async Task InsertChatData(bool isDontKnow = false)
        {
            bool isInsert = await _lineDBService.InsertIfNotFoundAsync(_chatId, new ChatSetting()
            {
                Type = _sourceType,
                ChatId = _chatId
            });






            if (isInsert && isDontKnow)
            {
                var messages = new List<LineText>() {
                    new LineText() { Text = $"{_lineEmoji.Shark}(好像不認識你...)" },
                    new LineText() { Text = $"讓我想想..." },
                    new LineText() { Text = $"喔喔！老朋友～ 原來4ni啊{_lineEmoji.ShineEye}" },
                    new LineText() { Text = $"想知道怎麼玩我，快輸入 /指令 吧 " },
                    new LineText() { Text = $"歡迎回來{_lineEmoji.Kiss}{_lineEmoji.Kiss}{_lineEmoji.Kiss}" }
                };
                await ReplyMessage(messages);
            }
            else if (isInsert)
            {
                var messages = new List<LineText>() {
                    new LineText() { Text = $"Hey！新朋友{_lineEmoji.ShineEye}" },
                    new LineText() { Text = $"輸入 /指令 就能知道怎麼玩我喔" },
                    new LineText() { Text = $"盡情的玩我吧{_lineEmoji.Kiss}{_lineEmoji.Kiss}{_lineEmoji.Kiss}" },
                };
                await ReplyMessage(messages);
            }
        }

        private async Task<bool> SetChatSetting()
        {
            _chatSetting = await _lineDBService.GetAllAsync(_chatId);

            return _chatSetting?.Any() ?? false;
        }

        private async Task HandleMessage()
        {
            switch (GetCommandType(_message))
            {
                case CommandType.AddTalk:
                    await HandleAddTalkMessage(_message.Text);
                    break;
                default:
                    await HandleNormalMessage(_message.Text);
                    break;
            }
        }

        private async Task HandleNormalMessage(string message)
        {
            //先處理拉麵
            if (message.Equals("來家拉麵吧"))
            {
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

                var messages = new List<LineText>();


                var shop = noodleList.OrderBy(_ => Guid.NewGuid()).First();
                messages.Add(new LineText { Text = $"吃{shop}啦{_lineEmoji.Eat}{_lineEmoji.Kiss}{_lineEmoji.ShineEye}{_lineEmoji.Laugh}" });


                var googleSearchResult = await _googleAPIService.GoogleSearchKeyWord(shop);

                var link1 = googleSearchResult["items"][0]["link"].ToString();
                var link2 = googleSearchResult["items"][1]["link"].ToString();
                messages.Add(new LineText { Text = link1 });
                messages.Add(new LineText { Text = link2 });

                await ReplyMessage(messages);
            }
            else if (message.Equals("qk"))
            {
                var lineQuickReply = new LineQuickReply();
                var quickReply = new QuickReplyContent();
                var items = new List<Item>();

                items.Add(new Item() { Action = new ActionContent() { Text = "測試1", Label = "測試1" } });
                items.Add(new Item() { Action = new ActionContent() { Text = "測試2", Label = "測試2" } });
                items.Add(new Item() { Action = new ActionContent() { Text = "測試3", Label = "測試3" } });

                quickReply.Items = items;


                lineQuickReply.Text = "QuickReply測試";
                lineQuickReply.QuickReply = quickReply;



                await ReplyMessage(new List<LineQuickReply> { lineQuickReply });
            }
            else
            {
                var talk = string.Empty;
                _chatSetting.Find(c => c.ChatId == _chatId).Talk.TryGetValue(message, out talk);

                if (!string.IsNullOrEmpty(talk))
                    await ReplyMessage(new List<LineText> { new LineText { Text = talk } });
            }
        }

        private async Task HandleAddTalkMessage(string message)
        {
            var splitMessage = message.Split("|");
            var collection = await _lineDBService.GetAsync(_chatId);

            collection.Talk.Add(splitMessage[1], splitMessage[2]);
            await _lineDBService.Update(_chatId, collection);
            await ReplyMessage(new List<LineText> { new LineText { Text = $"筆記中{_lineEmoji.Pencil}{_lineEmoji.Pencil}{_lineEmoji.Pencil}" } });
        }

        private CommandType GetCommandType(Message message)
        {
            var isMessage = message.Type == MessageType.Text;

            if (isMessage)
            {
                var splitMesage = message.Text.Split("|");
                switch (splitMesage[0])
                {
                    case "/學說話":
                        return CommandType.AddTalk;
                    default:
                        return CommandType.Normal;
                }
            }
            return CommandType.Normal;
        }
        #endregion
    }
}
