using FairPlay.Models;
using Microsoft.Extensions.Options;
using MongoDB.Driver;
using FairPlay.Data;
using System.Collections.Generic;
using System.Threading.Tasks;
using FairPlay.Services.Interface;

namespace FairPlay.Services.Impl
{
    /// <summary>
    /// Implementación de la interfaz ISportService que proporciona funcionalidad para la gestión de deportes.
    /// Esta clase interactúa con la colección "Sports" en MongoDB para realizar operaciones CRUD.
    /// </summary>
    public class SportService : ISportService
    {
        private readonly IMongoCollection<Sport> _sportsCollection;

        /// <summary>
        /// Constructor que inicializa la conexión a la base de datos MongoDB y configura la colección de deportes.
        /// </summary>
        /// <param name="mongoDBSettings">Configuración de conexión a MongoDB.</param>
        public SportService(IOptions<MongoDBSettings> mongoDBSettings)
        {
            var client = new MongoClient(mongoDBSettings.Value.ConnectionString);
            var database = client.GetDatabase(mongoDBSettings.Value.DatabaseName);
            _sportsCollection = database.GetCollection<Sport>("Sports");
        }

        /// <summary>
        /// Obtiene todos los deportes registrados en el sistema.
        /// </summary>
        /// <returns>Lista de todos los deportes.</returns>
        public async Task<List<Sport>> GetAllAsync() =>
            await _sportsCollection.Find(_ => true).ToListAsync();

        /// <summary>
        /// Busca un deporte por su identificador único.
        /// </summary>
        /// <param name="id">Identificador único del deporte.</param>
        /// <returns>Deporte encontrado o null si no existe.</returns>
        public async Task<Sport> GetByIdAsync(string id) =>
            await _sportsCollection.Find(s => s.Id == id).FirstOrDefaultAsync();

        /// <summary>
        /// Busca un deporte por su nombre.
        /// </summary>
        /// <param name="name">Nombre del deporte.</param>
        /// <returns>Deporte encontrado o null si no existe.</returns>
        public async Task<Sport> GetByNameAsync(string name) =>
            await _sportsCollection.Find(s => s.Name == name).FirstOrDefaultAsync();

        /// <summary>
        /// Crea un nuevo deporte en el sistema.
        /// </summary>
        /// <param name="sport">Objeto con la información del nuevo deporte.</param>
        public async Task CreateAsync(Sport sport) =>
            await _sportsCollection.InsertOneAsync(sport);

        /// <summary>
        /// Actualiza la información de un deporte existente.
        /// </summary>
        /// <param name="id">Identificador único del deporte a actualizar.</param>
        /// <param name="sport">Objeto con la nueva información del deporte.</param>
        public async Task UpdateAsync(string id, Sport sport) =>
            await _sportsCollection.ReplaceOneAsync(s => s.Id == id, sport);

        /// <summary>
        /// Elimina un deporte del sistema.
        /// </summary>
        /// <param name="id">Identificador único del deporte a eliminar.</param>
        public async Task DeleteAsync(string id) =>
            await _sportsCollection.DeleteOneAsync(s => s.Id == id);
    }
}