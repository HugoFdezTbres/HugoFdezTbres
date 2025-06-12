using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;


namespace FairPlay.Models
{
    public class Court
    {
        [BsonId]
        [BsonElement("_id"), BsonRepresentation(BsonType.ObjectId)]
        public string? id_court { get; set; }

        [BsonElement("name")]
        public string Name { get; set; } = null!;

        [BsonElement("Sport")]
        public List<SportInfo> Sport { get; set; } = new List<SportInfo>();

        [BsonElement("address")]
        public Address address  { get; set; } = null!;

        [BsonElement("openingHour")]
        public string OpeningHour { get; set; } = null!;

        [BsonElement("closingHour")]
        public string ClosingHour { get; set; } = null!;

        [BsonElement("courts")]
        public List<CourtInfo> Courts { get; set; } = new List<CourtInfo>();

        [BsonElement("facilityImage")]
        public string? FacilityImage { get; set; }

        [BsonElement("price")]
        public decimal Price { get; set; }
    }

    public class SportInfo
    {
        [BsonElement("name")]
        public string Name { get; set; } = null!;

        [BsonElement("image")]
        public string? Image { get; set; }
    }

    public class CourtInfo
    {
        [BsonId]
        [BsonElement("_id"), BsonRepresentation(BsonType.ObjectId)]
        public string? Id { get; set; }

        [BsonElement("name")]
        public string Name { get; set; } = null!;

        [BsonElement("image")]
        public string? Image { get; set; }

        [BsonElement("availableDates")]
        public List<DateTime> AvailableDates { get; set; } = new List<DateTime>();
    }

    public class Address
    {
        [BsonElement("street")]
        public string street { get; set; } = null!;

        [BsonElement("number")]
        public string number { get; set; } = null!;

        [BsonElement("town")]
        public string town { get; set; } = null!;

        [BsonElement("city")]
        public string city { get; set; } = null!;

        [BsonElement("country")]
        public string country { get; set; } = null!;

        [BsonElement("postal_code")]
        public string postal_code { get; set; } = null!;
    }

}
