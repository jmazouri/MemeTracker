using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MemeTracker.StoragePocos;

namespace MemeTracker
{
    public class CommandHandler
    {
        public static async Task<string> HandleCommand(DiscordMessage msg)
        {
            return await Task.Run(delegate
            {
                if (msg.Message.StartsWith("!goodshit"))
                {
                    if (msg.Message.Length < 12)
                    {
                        return "Syntax is: /goodshit [first] [second]";
                    }

                    string[] parts = msg.Message.Substring(10).Split(new[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);

                    if (parts.Length < 2)
                    {
                        return "Syntax is: /goodshit [first] [second] [emoji] [emoji]";
                    }

                    if (parts.Length < 4)
                    {
                        return new Copypasta(parts[0] + " " + parts[1], "👌", "👀").TotalCopypasta;
                    }

                    return new Copypasta(parts[0] + " " + parts[1], parts[2], parts[3]).TotalCopypasta;
                }

                if (msg.Message.StartsWith("!topmemers"))
                {
                    StringBuilder builder = new StringBuilder();
                    builder.AppendLine();
                                        
                    int iterator = 1;
                    foreach (var result in DatabaseContainer.Current.GetMessages().GroupBy(d => d.Username).OrderByDescending(d=>d.Count()))
                    {
                        builder.Append(iterator);
                        builder.Append(". ");
                        builder.Append(result.Key);
                        builder.Append($" ({result.Count()})");
                        builder.AppendLine();

                        iterator++;
                    }

                    return builder.ToString();
                }

                if (msg.Message.StartsWith("!mph"))
                {
                    int memecount = DatabaseContainer.Current.GetMessages().Count();
                    double duration = (DatabaseContainer.Current.GetMessages().Last().Timestamp - DatabaseContainer.Current.GetMessages().First().Timestamp).TotalHours;

                    return "Average Total Memes per Hour: " + (memecount/duration);
                }

                return null;
            });
        }
    }
}
