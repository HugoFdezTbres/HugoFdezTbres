using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace FairPlay.Models
{
    public class Match
    {
        [BsonId]
        [BsonElement("_id"), BsonRepresentation(BsonType.ObjectId)]
        public string? Id { get; set; }

        [BsonElement("courtId")]
        [BsonRepresentation(BsonType.ObjectId)]
        public string CourtId { get; set; } = null!;

        [BsonElement("date")]
        public DateTime Date { get; set; }

        [BsonElement("startTime")]
        public string StartTime { get; set; } = null!;

        [BsonElement("endTime")]
        public string EndTime { get; set; } = null!;

        [BsonElement("sport")]
        public string Sport { get; set; } = null!;

        [BsonElement("players")]
        public List<string> Players { get; set; } = new List<string>();

        [BsonElement("maxPlayers")]
        public int MaxPlayers { get; set; }

        [BsonElement("status")]
        public string Status { get; set; } = "Open"; // Open, Closed, Cancelled

        [BsonElement("createdBy")]
        [BsonRepresentation(BsonType.ObjectId)]
        public string CreatedBy { get; set; } = null!;

        [BsonElement("createdAt")]
        public DateTime CreatedAt { get; set; } = DateTime.Now;
    }
}