using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Telegram.Bot;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;

namespace TelegramTaskTrackerBot.Commands
{
    class TokenCommand : Command
    {
        private string token = "";
        public override string Name => @"/token";

        public override bool Contains(Message message)
        {
            if (message.Type != MessageType.Text)
            {
                return false;
            }

            return message.Text.Contains(Name);
        }

        public override async Task Execute(Message message, TelegramBotClient client)
        {
            token = "";
            var chatId = message.Chat.Id;

            await client.SendTextMessageAsync(chatId, "Enter your token!");

            client.OnUpdate += Client_OnUpdate;
            client.StartReceiving(Array.Empty<UpdateType>());

            while (token == "") continue;
            AppSettings.YouTrackToken = token;
            client.StopReceiving();
            try
            {
                var account = await YouTrackAccount.create(AppSettings.YouTrackUrl, AppSettings.YouTrackToken);
                await client.SendTextMessageAsync(chatId, $"Your url {AppSettings.YouTrackUrl} your token {AppSettings.YouTrackToken}. Success! Now you are receiving notifications from YouTracker! To mute press /mute");
                while (true)
                {
                    if (Bot.notificating)
                    {
                        var length = account.changesList.Count;
                        await account.GetIssuesUpdates();
                        await account.GetIssueStatusUpdate();
                        account.GetCommentsUpdates();
                        if (length < account.changesList.Count)
                        {
                            await client.SendTextMessageAsync(chatId, account.changesList[account.changesList.Count - 1].Message);
                        }
                        continue;
                    }
                }
            }
            catch
            {
                await client.SendTextMessageAsync(chatId, $"Check your data cautiously! Try again! Press /login");
            }
        }

        private void Client_OnUpdate(object sender, Telegram.Bot.Args.UpdateEventArgs e)
        {
            var message = e.Update.Message;
            if (e.Update == null || message.Type != MessageType.Text) return;

            token = message.Text;
        }
    }
}
