using Game.API.Model;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using MongoDB.Driver;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;


namespace Game.API.Service
{
    public class UserService
    {
        private readonly MongoDBService dbService;

        private readonly IConfiguration _configuration;

        public UserService(IOptions<DatabaseSettings> settings, IConfiguration configuration)
        {
            dbService = new MongoDBService(settings);
            _configuration = configuration;
        }

        public async Task<User> GetUserByUsername(string username)
        {
            return await dbService.User.Find(x => x.Username == username).FirstOrDefaultAsync();
        }

        public async Task CreateUser(User user)
        {
            await dbService.User.InsertOneAsync(user);
        }
        public async Task UpdateUser(User user)
        {
            await dbService.User.ReplaceOneAsync(x => x.Id == user.Id, user);
        }

    }
}
