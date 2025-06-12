using FairPlay.Models;
using Microsoft.AspNetCore.Cors.Infrastructure;
using Microsoft.Extensions.Options;
using MongoDB.Driver;
using MongoDB.Bson;
using FairPlay.Data;
using FairPlay.Services.Interface;

namespace FairPlay.Services.Impl
{
    public class CourtService : ICourtService
    {
        private readonly IMongoCollection<Court> _courtsCollection;

        public CourtService(IOptions<MongoDBSettings> mongoDBSettings)
        {
            var client = new MongoClient(mongoDBSettings.Value.ConnectionString);
            var database = client.GetDatabase(mongoDBSettings.Value.DatabaseName);
            _courtsCollection = database.GetCollection<Court>(mongoDBSettings.Value.CollectionName);
        }

        public async Task<List<Court>> GetAllAsync() =>
            await _courtsCollection.Find(_ => true).ToListAsync();

        public async Task<Court> GetByIdAsync(string id) =>
            await _courtsCollection.Find(p => p.id_court == id).FirstOrDefaultAsync();

        public async Task<Court> CreateAsync(Court court)
        {
            await _courtsCollection.InsertOneAsync(court);
            return court;
        }

        public async Task UpdateAsync(string id, Court court) =>
            await _courtsCollection.ReplaceOneAsync(p => p.id_court == id, court);

        public async Task DeleteAsync(string id) =>
            await _courtsCollection.DeleteOneAsync(p => p.id_court == id);
    }
}
