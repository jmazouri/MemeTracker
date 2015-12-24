using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Discord;
using LiteDB;

namespace MemeTracker.StoragePocos
{
    public class DiscordMessage
    {
        private static readonly Regex UrlRegex = new Regex(@"(http|ftp|https)://([\w+?\.\w+])+([a-zA-Z0-9\~\!\@\#\$\%\^\&\*\(\)_\-\=\+\\\/\?\.\:\;\'\,]*)?", RegexOptions.Compiled);

        public string Username { get; set; }
        public DateTime Timestamp { get; set; }

        private string _message;

        public string Message
        {
            get { return _message; }
            set
            {
                if (value == _message)
                {
                    return;
                }

                _message = value;
                _urlCache = null;
            }
        }

        [BsonIgnore]
        public Message OriginalMessage { get; set; }

        private List<string> _urlCache; 
        public List<string> Urls
        {
            get
            {
                if (_urlCache != null)
                {
                    return _urlCache;
                }

                var results = UrlRegex.Matches(Message);
                var ret = new List<string>();

                foreach (Match result in results)
                {
                    ret.Add(result.Value);
                }

                _urlCache = ret;

                return ret;
            }
        } 

        /// <summary>
        /// Create an empty Discord message for storage and retrieval.
        /// </summary>
        public DiscordMessage()
        {
            //Just set the properties
        }

        /// <summary>
        /// Create a Discord message from an API response
        /// </summary>
        /// <param name="origMessage">The message to be converted</param>
        public DiscordMessage(Message origMessage)
        {
            Username = origMessage.User.Name;
            Timestamp = origMessage.Timestamp;
            Message = origMessage.Text;

            OriginalMessage = origMessage;
        }
    }
}
