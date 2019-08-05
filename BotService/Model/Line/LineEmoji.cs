using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BotService.Model.Line
{
    public interface ILineEmoji
    {
        string Pencil { get; set; }
        string Eat { get; set; }
        string Kiss { get; set; }
        string ShineEye { get; set; }
        string Laugh { get; set; }
        string Shark { get; set; }
    }

    public class LineEmoji: ILineEmoji
    {
        public string Pencil { get; set; } = char.ConvertFromUtf32(0x100041);
        public string Eat { get; set; } = char.ConvertFromUtf32(0x100093);
        public string Kiss { get; set; } = char.ConvertFromUtf32(0x100096);
        public string ShineEye { get; set; } = char.ConvertFromUtf32(0x10007A);
        public string Laugh { get; set; } = char.ConvertFromUtf32(0x10009D);
        public string Shark { get; set; } = char.ConvertFromUtf32(0x100089);
    }
}
