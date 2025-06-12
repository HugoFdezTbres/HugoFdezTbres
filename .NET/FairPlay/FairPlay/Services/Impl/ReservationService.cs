using FairPlay.Models;
using Microsoft.Extensions.Options;
using MongoDB.Driver;
using FairPlay.Data;
using System.Collections.Generic;
using System.Threading.Tasks;
using FairPlay.Services.Interface;

namespace FairPlay.Services.Impl
{
    public class ReservationService : IReservationService
    {
        private readonly IMongoCollection<Reservation> _reservationsCollection;

        public ReservationService(IOptions<MongoDBSettings> mongoDBSettings)
        {
            var client = new MongoClient(mongoDBSettings.Value.ConnectionString);
            var database = client.GetDatabase(mongoDBSettings.Value.DatabaseName);
            _reservationsCollection = database.GetCollection<Reservation>("Reservations");
        }

        public async Task<List<Reservation>> GetAllAsync() =>
            await _reservationsCollection.Find(_ => true).ToListAsync();

        public async Task<Reservation> GetByIdAsync(string id) =>
            await _reservationsCollection.Find(r => r.Id == id).FirstOrDefaultAsync();

        public async Task<List<Reservation>> GetByUserIdAsync(string userId) =>
            await _reservationsCollection.Find(r => r.UserId == userId).ToListAsync();

        public async Task<List<Reservation>> GetByCourtIdAsync(string courtId) =>
            await _reservationsCollection.Find(r => r.CourtId == courtId).ToListAsync();

        public async Task<List<Reservation>> GetByDateAsync(DateTime date) =>
            await _reservationsCollection.Find(r => r.Date.Date == date.Date).ToListAsync();

        public async Task<bool> IsTimeSlotAvailableAsync(string courtId, DateTime date, DateTime startTime, DateTime endTime)
        {
            // Verificar si ya existe una reserva para la misma pista, fecha y horario
            var existingReservation = await _reservationsCollection.Find(r =>
                r.CourtId == courtId &&
                r.Date.Date == date.Date &&
                r.Status != "Cancelled" &&
                (r.StartTime <= startTime && r.EndTime > startTime ||
                 r.StartTime < endTime && r.EndTime >= endTime ||
                 r.StartTime >= startTime && r.EndTime <= endTime))
                .FirstOrDefaultAsync();

            return existingReservation == null;
        }

        public async Task CreateAsync(Reservation reservation) =>
            await _reservationsCollection.InsertOneAsync(reservation);

        public async Task UpdateAsync(string id, Reservation reservation) =>
            await _reservationsCollection.ReplaceOneAsync(r => r.Id == id, reservation);

        public async Task CancelReservationAsync(string id)
        {
            var reservation = await GetByIdAsync(id);
            if (reservation != null)
            {
                reservation.Status = "Cancelled";
                await UpdateAsync(id, reservation);
            }
        }

        public async Task DeleteAsync(string id) =>
            await _reservationsCollection.DeleteOneAsync(r => r.Id == id);
    }
}