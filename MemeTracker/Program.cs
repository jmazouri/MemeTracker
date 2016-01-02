using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Discord;
using MemeTracker.StoragePocos;

namespace MemeTracker
{
    class Program
    {
        static void Main(string[] args)
        {
            var client = new DiscordClient(new DiscordClientConfig
            {
                VoiceMode = DiscordVoiceMode.Outgoing,
                EnableVoiceMultiserver = false,
                EnableVoiceEncryption = true,
                AckMessages = true
            });

            client.LogMessage += (s, e) =>
            {
                Console.ForegroundColor = ConsoleColor.White;
                Console.BackgroundColor = ConsoleColor.DarkBlue;

                Console.WriteLine($"[{e.Severity}] {e.Source}: {e.Message}");
            };

            //Echo back any message received, provided it didn't come from the bot itself
            client.MessageReceived += async (s, e) =>
            {
                if (e.Message.IsAuthor || (e.Channel.Name != "memes" && e.Channel.Name != "bot_tests"))
                {
                    return;
                }

                var convertedMessage = new DiscordMessage(e.Message);

                if (!e.Message.Text.StartsWith("!"))
                {
                    await DatabaseContainer.Current.StoreMessage(convertedMessage);
                }

                string result = await CommandHandler.HandleCommand(client, convertedMessage);

                if (result != null)
                {
                    await client.SendMessage(e.Channel, result);
                }

                Console.WriteLine($"[{e.Message.Timestamp.ToShortTimeString()}] {e.User.Name}: {e.Message.Text}");
            };

            //Convert our sync method to an async one and block the Main function until the bot disconnects
            client.Run(async () =>
            {
                //Connect to the Discord server using our email and password
                await client.Connect(ConfigurationManager.AppSettings["email"], ConfigurationManager.AppSettings["password"]);
            });
        }
    }
}
