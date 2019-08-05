using BotService.Model.Line;
using Microsoft.Extensions.Configuration;
using MongoDB.Driver;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BotService.Service.Line
{
    public interface ILineDBService
    {
        List<ChatSetting> GetAll(string chatId);
        Task<ChatSetting> GetAsync(string chatId);
        Task<List<ChatSetting>> GetAllAsync();
        Task<List<ChatSetting>> GetAllAsync(string chatId);
        Task InsertAsync(ChatSetting chatSetting);
        Task InsertAsync(IEnumerable<ChatSetting> chatSetting);
        Task<bool> InsertIfNotFoundAsync(string chatId, ChatSetting chatSetting);
        Task Update(string chatId, ChatSetting chatSetting);
        Task DeleteAsync(string chatId);
    }

    public class LineDBService : ILineDBService
    {
        private readonly IMongoCollection<ChatSetting> _lineDB;

        public LineDBService(IConfiguration config)
        {
            var client = new MongoClient(config["LineDatabaseSetting:ConnectionString"]);
            var database = client.GetDatabase(config["LineDatabaseSetting:DatabaseName"]);
            _lineDB = database.GetCollection<ChatSetting>(config["LineDatabaseSetting:CollectionName"]);
        }

        public ChatSetting Get(string chatId) =>
            _lineDB.Find(context => context.ChatId == chatId).FirstOrDefault();

        public async Task<ChatSetting> GetAsync(string chatId)
        {
            var result = await _lineDB.FindAsync(context => context.ChatId == chatId);

            return result.FirstOrDefault();
        }

        public List<ChatSetting> GetAll(string chatId) =>
            _lineDB.Find(context => context.ChatId == chatId).ToList();

        public async Task<List<ChatSetting>> GetAllAsync(string chatId)
        {
            var result = await _lineDB.FindAsync(context => context.ChatId == chatId);

            return result.ToList();
        }

        public async Task<List<ChatSetting>> GetAllAsync()
        {
            var result = await _lineDB.FindAsync(context => true);

            return result.ToList();
        }

        public async Task InsertAsync(ChatSetting chatSetting) =>
            await _lineDB.InsertOneAsync(chatSetting);

        public async Task InsertAsync(IEnumerable<ChatSetting> chatSetting) =>
            await _lineDB.InsertManyAsync(chatSetting);

        public async Task<bool> InsertIfNotFoundAsync(string chatId, ChatSetting chatSetting)
        {
            var found = _lineDB.Find(context => context.ChatId == chatId).FirstOrDefault();
            if (found == null)
            {
                await _lineDB.InsertOneAsync(chatSetting);
                return true;
            }
            return false;
        }

        public async Task Update(string chatId, ChatSetting chatSetting) =>
            await _lineDB.ReplaceOneAsync(context => context.ChatId == chatId, chatSetting);

        public async Task DeleteAsync(string chatId) =>
            await _lineDB.DeleteOneAsync(context => context.ChatId == chatId);
    }
}
