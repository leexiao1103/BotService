using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BotService.Model.Line
{
    public interface ILineReply
    {

    }



    public class LineText
    {
        public string Type { get; private set; } = "text";

        public string Text { get; set; }
    }

    #region Quick reply

    public class LineQuickReply
    {
        public string Type { get; private set; } = "text";

        public string Text { get; set; }

        public QuickReplyContent QuickReply { get; set; }
    }

    public class QuickReplyContent
    {
        public List<Item> Items { get; set; }
    }

    public class Item
    {
        public string Type { get; private set; } = "action";
        public ActionContent Action { get; set; }
    }

    public class ActionContent
    {
        public string Type { get; private set; } = "message";
        public string Label { get; set; }
        public string Text { get; set; }
    }

    #endregion
}










