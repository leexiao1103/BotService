using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BotService.Model.Line
{
    public interface ILineservice {
        List<LineEvent> Events { get;set;}
    }

    public class LineWebhookEvent: ILineservice
    {
        public List<LineEvent> Events { get; set; }
    }

    public class LineEvent
    {
        public string ReplyToken { get; set; }
        public string Type { get; set; }
        public Source Source { get; set; }
        public Message Message { get; set; }
    }

    public class Source
    {
        public string Type { get; set; }
        public string UserId { get; set; }
    }

    public class Message
    {
        public string Id { get; set; }
        public string Type { get; set; }
        public string Text { get; set; }
    }
}
