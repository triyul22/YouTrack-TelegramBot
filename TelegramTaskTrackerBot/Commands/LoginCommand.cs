using System;
using System.Collections.Generic;
using System.Text;
using Telegram.Bot.Types;
using Telegram.Bot;
using System.Threading.Tasks;
using Telegram.Bot.Types.Enums;
using System.Text.RegularExpressions;

namespace TelegramTaskTrackerBot.Commands
{
    class LoginCommand : Command
    {
        private string url = "";
        public override string Name => @"/login";

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
            url = "";
            var chatId = message.Chat.Id;

            await client.SendTextMessageAsync(chatId, "Enter url https://yourdomain.myjetbrains.com/youtrack");
            
            client.OnUpdate += Client_OnUpdate;
            client.StartReceiving(Array.Empty<UpdateType>());

            while (url == "") continue;
            if (Regex.IsMatch(url, @"https://\S+.myjetbrains.com/youtrack"))
            {
                AppSettings.YouTrackUrl = url;
                await client.SendTextMessageAsync(chatId, $"Your url matches! {url}. Press /token to enter your token.");
            }
            else
            {
                client.StopReceiving();
                await client.SendTextMessageAsync(chatId, $"You url doesn't match. Try again! Prress /login");
                await Execute(message, client);
            }
            client.StopReceiving();
            client.OnUpdate -= Client_OnUpdate;
            return;
        }

        private void Client_OnUpdate(object sender, Telegram.Bot.Args.UpdateEventArgs e)
        {
            var message = e.Update.Message;
            if (e.Update == null || message.Type != MessageType.Text) return;

            url = message.Text;
            Console.WriteLine(url);
        }
    }
}
