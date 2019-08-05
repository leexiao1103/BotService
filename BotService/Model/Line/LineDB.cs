using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using System.Collections.Generic;

namespace BotService.Model.Line
{
    public class ChatSetting
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string Id { get; set; }

        public SourceType Type { get; set; }

        public string ChatId { get; set; }

        public Dictionary<string, string> Talk { get; set; } = new Dictionary<string, string>() {
            { "/指令", "/學說話|聽到|要回答".Replace("\n", "%0D%0A") }
        };
    }
}
