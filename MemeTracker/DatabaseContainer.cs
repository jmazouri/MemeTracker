using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LiteDB;
using MemeTracker.StoragePocos;

namespace MemeTracker
{
    public class DatabaseContainer
    {
        private LiteDatabase _db;

        private static DatabaseContainer _currentContainer;
        public static DatabaseContainer Current => _currentContainer ?? (_currentContainer = new DatabaseContainer("meme.db"));

        private DatabaseContainer(string pathToDatabase)
        {
            _db = new LiteDatabase(pathToDatabase);
        }

        public IEnumerable<DiscordMessage> GetMessages()
        {
            return _db.GetCollection<DiscordMessage>("messages").FindAll();
        }

        public async Task StoreMessage(DiscordMessage message)
        {
            await Task.Run(delegate
            {
                _db.GetCollection<DiscordMessage>("messages").Insert(message);
            });
        }
    }
}
