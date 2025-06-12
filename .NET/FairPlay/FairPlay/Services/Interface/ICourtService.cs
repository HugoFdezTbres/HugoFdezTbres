using FairPlay.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace FairPlay.Services.Interface
{
    /// <summary>
    /// Interfaz que define las operaciones disponibles para la gestión de pistas deportivas en el sistema FairPlay.
    /// Esta interfaz es implementada por CourtService que interactúa con la colección "Courts" en MongoDB.
    /// </summary>
    public interface ICourtService
    {
        /// <summary>
        /// Obtiene todas las pistas deportivas registradas en el sistema.
        /// </summary>
        /// <returns>Una lista de todas las pistas deportivas.</returns>
        Task<List<Court>> GetAllAsync();

        /// <summary>
        /// Busca una pista deportiva por su identificador único.
        /// </summary>
        /// <param name="id">El identificador único de la pista.</param>
        /// <returns>La pista encontrada o null si no existe.</returns>
        Task<Court> GetByIdAsync(string id);

        /// <summary>
        /// Crea una nueva pista deportiva en el sistema con ID autogenerado.
        /// </summary>
        /// <param name="court">Objeto con la información de la nueva pista.</param>
        /// <returns>La pista creada con su ID autogenerado.</returns>
        Task<Court> CreateAsync(Court court);

        /// <summary>
        /// Actualiza la información de una pista deportiva existente.
        /// </summary>
        /// <param name="id">El identificador único de la pista a actualizar.</param>
        /// <param name="court">Objeto con la nueva información de la pista.</param>
        Task UpdateAsync(string id, Court court);

        /// <summary>
        /// Elimina una pista deportiva del sistema.
        /// </summary>
        /// <param name="id">El identificador único de la pista a eliminar.</param>
        Task DeleteAsync(string id);
    }
}
