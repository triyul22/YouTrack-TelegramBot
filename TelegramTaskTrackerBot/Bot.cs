using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Policy;
using System.Threading.Tasks;
using Telegram.Bot;
using TelegramTaskTrackerBot.Commands;

namespace TelegramTaskTrackerBot
{
    public static class Bot
    {
        private static TelegramBotClient client;

        public static List<Command> commandList;
        public static bool notificating;
        public static string url;


        public static async Task<TelegramBotClient> GetBotClientAsync()
        {
            if (client != null)
            {
                return client;
            }
            notificating = true;
            commandList = new List<Command>();
            commandList.Add(new StartCommand());
            commandList.Add(new MuteCommand());
            commandList.Add(new LoginCommand());
            commandList.Add(new UnmuteCommand());
            commandList.Add(new TokenCommand());

            client = new TelegramBotClient(AppSettings.Token);

            return client;
        }

    }
}

