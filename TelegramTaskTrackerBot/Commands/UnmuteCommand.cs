using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Telegram.Bot;
using Telegram.Bot.Types;

namespace TelegramTaskTrackerBot.Commands
{
    class UnmuteCommand : Command
    {
        public override string Name => @"/unmute";

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

            Bot.notificating = true;

            await client.SendTextMessageAsync(chatId, "You have unmuted notifications! To stop receiving notifications press /mute");
        }
    }
}
