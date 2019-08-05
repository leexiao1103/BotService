using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BotService.Model.Line
{
    public enum SourceType
    {
        User, Group, Room
    }

    public enum MessageType
    {
        Text, Image, Video, Audio, File, Location, Sticker
    }

    public enum EventType
    {
        Message, Follow, Unfollow, Join, Leave, Postback, Beacon
    }    

    public enum CommandType
    {
        Normal, AddTalk
    }
}
