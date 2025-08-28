using Game.API.Model;
using Microsoft.Extensions.Options;
using MongoDB.Driver;

namespace Game.API.Service
{
    public class PlayerService 
    {
        private readonly MongoDBService dbService;

        public PlayerService(IOptions<DatabaseSettings> settings)
        {
            dbService = new MongoDBService(settings);
        }

        public async Task CreatePlayer(User user)
        {
            Player player = new Player();
            player.Id = user.Id;
            await dbService.Player.InsertOneAsync(player);
        }

        public async Task<Player> GetPlayerById(string id)
        {
            var filter = Builders<Player>.Filter.Eq(p => p.Id, id);

            return await dbService.Player.Find(filter).FirstOrDefaultAsync();
        }

        public async Task ReplacePlayer(Player newPlayer)
        {
            var filter = Builders<Player>.Filter.Eq(p => p.Id, newPlayer.Id);
            await dbService.Player.ReplaceOneAsync(filter, newPlayer);
        }


        public async Task UpdateInventoryPlayer(string playerId, Inventory newInventory)
        {
            var filter = Builders<Player>.Filter.Eq(p => p.Id, playerId);
            var update = Builders<Player>.Update.Set(p => p.Inventory, newInventory);

            await dbService.Player.UpdateOneAsync(filter, update);
        }

    }
}
