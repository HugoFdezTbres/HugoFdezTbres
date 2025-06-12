using FairPlay.Models;
using Microsoft.Extensions.Options;
using MongoDB.Driver;
using FairPlay.Data;
using System.Collections.Generic;
using System.Threading.Tasks;
using FairPlay.Services.Interface;

namespace FairPlay.Services.Impl
{
    public class MatchService : IMatchService
    {
        private readonly IMongoCollection<Match> _matchesCollection;

        public MatchService(IOptions<MongoDBSettings> mongoDBSettings)
        {
            var client = new MongoClient(mongoDBSettings.Value.ConnectionString);
            var database = client.GetDatabase(mongoDBSettings.Value.DatabaseName);
            _matchesCollection = database.GetCollection<Match>("Matches");
        }

        public async Task<List<Match>> GetAllAsync() =>
            await _matchesCollection.Find(_ => true).ToListAsync();

        public async Task<Match> GetByIdAsync(string id) =>
            await _matchesCollection.Find(m => m.Id == id).FirstOrDefaultAsync();

        public async Task<List<Match>> GetByCourtIdAsync(string courtId) =>
            await _matchesCollection.Find(m => m.CourtId == courtId).ToListAsync();

        public async Task<List<Match>> GetByUserIdAsync(string userId) =>
            await _matchesCollection.Find(m => m.Players.Contains(userId) || m.CreatedBy == userId).ToListAsync();

        public async Task<List<Match>> GetAvailableMatchesAsync() =>
            await _matchesCollection.Find(m => m.Status == "Open" && m.Players.Count < m.MaxPlayers).ToListAsync();

        public async Task CreateAsync(Match match) =>
            await _matchesCollection.InsertOneAsync(match);

        public async Task UpdateAsync(string id, Match match) =>
            await _matchesCollection.ReplaceOneAsync(m => m.Id == id, match);

        public async Task DeleteAsync(string id) =>
            await _matchesCollection.DeleteOneAsync(m => m.Id == id);

        public async Task<bool> JoinMatchAsync(string matchId, string userId)
        {
            var match = await GetByIdAsync(matchId);

            if (match == null || match.Status != "Open" || match.Players.Count >= match.MaxPlayers || match.Players.Contains(userId))
                return false;

            match.Players.Add(userId);
            await UpdateAsync(matchId, match);
            return true;
        }

        public async Task<bool> LeaveMatchAsync(string matchId, string userId)
        {
            var match = await GetByIdAsync(matchId);

            if (match == null || !match.Players.Contains(userId))
                return false;

            match.Players.Remove(userId);
            await UpdateAsync(matchId, match);
            return true;
        }
    }
}