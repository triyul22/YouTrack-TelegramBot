using System;
using System.Collections.Generic;
using System.Text;
using Telegram.Bot.Types;

namespace TelegramTaskTrackerBot
{
    public class Change
    {
        public List<Change> changesList;
        public string Message { get; set; }
        public Change(string changeMessage)
        {
            Message = changeMessage;
        }
    }
}
