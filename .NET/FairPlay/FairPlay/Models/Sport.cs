using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace FairPlay.Models
{
    /// <summary>
    /// Representa un deporte en el sistema FairPlay.
    /// Contiene información básica sobre los deportes disponibles para los partidos.
    /// </summary>
    public class Sport
    {
        /// <summary>
        /// Identificador único del deporte en la base de datos.
        /// </summary>
        [BsonId]
        [BsonElement("_id"), BsonRepresentation(BsonType.ObjectId)]
        public string? Id { get; set; }

        /// <summary>
        /// Nombre del deporte.
        /// </summary>
        [BsonElement("name")]
        public string Name { get; set; } = null!;

        /// <summary>
        /// URL o ruta de la imagen representativa del deporte.
        /// </summary>
        [BsonElement("image")]
        public string? Image { get; set; }

        /// <summary>
        /// Descripción detallada del deporte.
        /// </summary>
        [BsonElement("description")]
        public string? Description { get; set; }
    }
}