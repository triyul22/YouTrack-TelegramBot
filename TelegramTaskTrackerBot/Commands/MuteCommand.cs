using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Telegram.Bot;
using Telegram.Bot.Types;

namespace TelegramTaskTrackerBot.Commands
{
    class MuteCommand : Command
    {
        public override string Name => @"/mute";

        public override bool Contains(Message message)
        {
            if (message.Type != Telegram.Bot.Types.Enums.MessageType.Text)
            {
                return false;
            }

            return message.Text.Contains(Name);
        }

        public override async Task Execute(Message message, TelegramBotClient client)
        {

            var chatId = message.Chat.Id;

            Bot.notificating = false;

            await client.SendTextMessageAsync(chatId, "You have muted notifications! To start receiving it again send /unmute");
        }
    }
}
