using FairPlay.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace FairPlay.Services.Interface
{
    /// <summary>
    /// Interfaz que define las operaciones disponibles para la gestión de deportes en el sistema FairPlay.
    /// </summary>
    public interface ISportService
    {
        /// <summary>
        /// Obtiene todos los deportes registrados en el sistema.
        /// </summary>
        /// <returns>Una lista de todos los deportes.</returns>
        Task<List<Sport>> GetAllAsync();

        /// <summary>
        /// Busca un deporte por su identificador único.
        /// </summary>
        /// <param name="id">El identificador único del deporte.</param>
        /// <returns>El deporte encontrado o null si no existe.</returns>
        Task<Sport> GetByIdAsync(string id);

        /// <summary>
        /// Busca un deporte por su nombre.
        /// </summary>
        /// <param name="name">El nombre del deporte.</param>
        /// <returns>El deporte encontrado o null si no existe.</returns>
        Task<Sport> GetByNameAsync(string name);

        /// <summary>
        /// Crea un nuevo deporte en el sistema.
        /// </summary>
        /// <param name="sport">Objeto con la información del nuevo deporte.</param>
        Task CreateAsync(Sport sport);

        /// <summary>
        /// Actualiza la información de un deporte existente.
        /// </summary>
        /// <param name="id">El identificador único del deporte a actualizar.</param>
        /// <param name="sport">Objeto con la nueva información del deporte.</param>
        Task UpdateAsync(string id, Sport sport);

        /// <summary>
        /// Elimina un deporte del sistema.
        /// </summary>
        /// <param name="id">El identificador único del deporte a eliminar.</param>
        Task DeleteAsync(string id);
    }
}