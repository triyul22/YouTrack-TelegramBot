using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using System.Threading;
using Telegram.Bot.Types;
using Telegram.Bot;


namespace TelegramTaskTrackerBot.Commands
{
    class StartCommand : Command
    {
        public override string Name => @"/start";

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

            await client.SendTextMessageAsync(chatId, "Hi! I am a YouTrack Helper bot! To start working log into your account! Press /login");
        }
    }
}
