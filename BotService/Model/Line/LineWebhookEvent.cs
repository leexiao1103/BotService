using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BotService.Model.Line
{
    public interface ILineWebhookEvent
    {
        List<LineEvent> Events { get; set; }
    }

    public class LineWebhookEvent : ILineWebhookEvent
    {
        public List<LineEvent> Events { get; set; }
    }

    public class LineEvent
    {
        public string ReplyToken { get; set; }
        public EventType Type { get; set; }
        public Source Source { get; set; }
        public Message Message { get; set; }
    }

    public class Source
    {
        public SourceType Type { get; set; }
        public string UserId { get; set; } = "";
        public string GroupId { get; set; } = "";
        public string RoomId { get; set; } = "";
    }

    public class Message
    {
        public string Id { get; set; }
        public MessageType Type { get; set; }
        public string Text { get; set; }
    }
}
