using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace FairPlay.Models
{
    public class Reservation
    {
        [BsonId]
        [BsonElement("_id"), BsonRepresentation(BsonType.ObjectId)]
        public string? Id { get; set; }

        [BsonElement("courtId")]
        [BsonRepresentation(BsonType.ObjectId)]
        public string CourtId { get; set; } = null!;

        [BsonElement("userId")]
        [BsonRepresentation(BsonType.ObjectId)]
        public string UserId { get; set; } = null!;

        [BsonElement("date")]
        public DateTime Date { get; set; }

        [BsonElement("startTime")]
        public DateTime StartTime { get; set; }

        [BsonElement("endTime")]
        public DateTime EndTime { get; set; }

        [BsonElement("sport")]
        public string Sport { get; set; } = null!;

        [BsonElement("facilityName")]
        public string FacilityName { get; set; } = null!;

        [BsonElement("facilityAddress")]
        public string FacilityAddress { get; set; } = null!;

        [BsonElement("courtImage")]
        public string? CourtImage { get; set; }

        [BsonElement("canModify")]
        public bool CanModify { get; set; } = true;

        [BsonElement("status")]
        public string Status { get; set; } = "Confirmed"; // Confirmed, Cancelled, Completed

        [BsonElement("createdAt")]
        public DateTime CreatedAt { get; set; } = DateTime.Now;

        [BsonElement("price")]
        public decimal Price { get; set; }

        [BsonElement("paymentStatus")]
        public string PaymentStatus { get; set; } = "Pending"; // Pending, Paid, Refunded
    }
}