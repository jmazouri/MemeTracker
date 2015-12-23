using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Discord;

namespace MemeTracker
{
    class Program
    {
        static void Main(string[] args)
        {
            var client = new DiscordClient();

            client.LogMessage += (s, e) => Console.WriteLine($"[{e.Severity}] {e.Source}: {e.Message}");

            //Echo back any message received, provided it didn't come from the bot itself
            client.MessageReceived += async (s, e) =>
            {
                if (!e.Message.IsAuthor)
                    await client.SendMessage(e.Channel, e.Message.Text);
            };

            //Convert our sync method to an async one and block the Main function until the bot disconnects
            client.Run(async () =>
            {
                //Connect to the Discord server using our email and password
                await client.Connect("jmazouri+bot1@gmail.com", ConfigurationManager.AppSettings["email"]);
            });
        }
    }
}
