using Atlassian.Jira;
using Newtonsoft.Json;
using System;
using System.IO;
using System.Net;
using System.Net.Http;
using System.Text.Json;
using System.Threading.Tasks;
using System.Net.Http.Headers;
using System.Text;
using System.Linq;
using Telegram.Bot;
using TelegramTaskTrackerBot.Commands;
using System.Collections.Generic;
using System.Threading;

namespace TelegramTaskTrackerBot
{
    class Program
    {
        static TelegramBotClient bot;
        static TelegramBotClient client;
        static List<Command> commandList;

        public async static Task Main()
        {

            //var client = await YouTrackAccount.create("https://triyul.myjetbrains.com/youtrack/", "perm:cm9vdA==.NDUtMA==.pQFX0ftsPE0v8hznQJvtwGB9HVi409");
            client = await Bot.GetBotClientAsync();
            commandList = Bot.commandList;
            client.OnMessage += Client_OnMessage;
            client.StartReceiving();
            Console.ReadKey();
            client.StopReceiving();

            /*while (true)
            {
                //var newItems = await client.GetIssuesUpdates();
                await client.GetIssueStatusUpdate();

                *//*if(newItems != null && newItems.ToList().Count != 0)
                {
                    foreach (var item in newItems)
                    {
                        Console.WriteLine(item);
                    }
                }*//*
            }*/
            //var users = await client.GetUsers();

            /*foreach(var pr in client.Projects)
            {
                Console.WriteLine(pr.Name);
            }
            foreach (var u in users)
            {
                Console.WriteLine(u.Username);
            }*/
        }

        private static async void Client_OnMessage(object sender, Telegram.Bot.Args.MessageEventArgs e)
        {
            var message = e?.Message?.Text;
            if (message == null)
            {
                return;
            }

            foreach (var command in commandList)
            {
                if (command.Contains(e.Message))
                {
                    await command.Execute(e.Message, client);
                    break;
                }
            }
        }

        private static async void Bot_OnMessage(object sender, Telegram.Bot.Args.MessageEventArgs e)
        {
            var message = e?.Message?.Text;
            if (message == null)
            {
                return;
            }

            foreach (var command in commandList)
            {
                if (command.Contains(e.Message))
                {
                    await command.Execute(e.Message, bot);
                    break;
                }
            }
        }
    }
}
