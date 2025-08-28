using Game.API.Model;
using Microsoft.Extensions.Options;
using MongoDB.Driver;

namespace Game.API.Service
{
    public class MongoDBService
    {
        private readonly IMongoCollection<User> _user;
        private readonly IMongoCollection<Player> _player;

        public IMongoCollection<User> User => _user;
        public IMongoCollection<Player> Player => _player;

        public MongoDBService(IOptions<DatabaseSettings> mongoDBSettings)
        {

            MongoClient client = new MongoClient(mongoDBSettings.Value.ConnectionString);
            IMongoDatabase database = client.GetDatabase(mongoDBSettings.Value.DatabaseName);

            _user = database.GetCollection<User>(mongoDBSettings.Value.UserCollection);
            _player = database.GetCollection<Player>(mongoDBSettings.Value.PlayerCollection);
        }
    }
}
