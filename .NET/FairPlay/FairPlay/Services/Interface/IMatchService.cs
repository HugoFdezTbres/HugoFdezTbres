using FairPlay.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace FairPlay.Services.Interface
{
    /// <summary>
    /// Interfaz que define las operaciones disponibles para la gestión de partidos en el sistema FairPlay.
    /// Esta interfaz es implementada por MatchService que interactúa con la colección "Matches" en MongoDB.
    /// Permite la creación, búsqueda, actualización y eliminación de partidos, así como la gestión de participantes.
    /// </summary>
    public interface IMatchService
    {
        /// <summary>
        /// Obtiene todos los partidos registrados en el sistema.
        /// </summary>
        /// <returns>Una lista de todos los partidos.</returns>
        Task<List<Match>> GetAllAsync();

        /// <summary>
        /// Busca un partido por su identificador único.
        /// </summary>
        /// <param name="id">El identificador único del partido.</param>
        /// <returns>El partido encontrado o null si no existe.</returns>
        Task<Match> GetByIdAsync(string id);

        /// <summary>
        /// Obtiene los partidos programados en una pista específica.
        /// </summary>
        /// <param name="courtId">El identificador único de la pista.</param>
        /// <returns>Lista de partidos programados en la pista especificada.</returns>
        Task<List<Match>> GetByCourtIdAsync(string courtId);

        /// <summary>
        /// Obtiene los partidos en los que participa un usuario específico.
        /// </summary>
        /// <param name="userId">El identificador único del usuario.</param>
        /// <returns>Lista de partidos en los que participa el usuario.</returns>
        Task<List<Match>> GetByUserIdAsync(string userId);

        /// <summary>
        /// Obtiene los partidos disponibles para unirse (que no han alcanzado su capacidad máxima).
        /// </summary>
        /// <returns>Lista de partidos disponibles para unirse.</returns>
        Task<List<Match>> GetAvailableMatchesAsync();

        /// <summary>
        /// Crea un nuevo partido en el sistema.
        /// </summary>
        /// <param name="match">Objeto con la información del nuevo partido.</param>
        Task CreateAsync(Match match);

        /// <summary>
        /// Actualiza la información de un partido existente.
        /// </summary>
        /// <param name="id">El identificador único del partido a actualizar.</param>
        /// <param name="match">Objeto con la nueva información del partido.</param>
        Task UpdateAsync(string id, Match match);

        /// <summary>
        /// Elimina un partido del sistema.
        /// </summary>
        /// <param name="id">El identificador único del partido a eliminar.</param>
        Task DeleteAsync(string id);

        /// <summary>
        /// Permite a un usuario unirse a un partido existente.
        /// </summary>
        /// <param name="matchId">El identificador único del partido.</param>
        /// <param name="userId">El identificador único del usuario que se une.</param>
        /// <returns>True si el usuario se unió correctamente, False en caso contrario.</returns>
        Task<bool> JoinMatchAsync(string matchId, string userId);

        /// <summary>
        /// Permite a un usuario abandonar un partido en el que participa.
        /// </summary>
        /// <param name="matchId">El identificador único del partido.</param>
        /// <param name="userId">El identificador único del usuario que abandona.</param>
        /// <returns>True si el usuario abandonó correctamente, False en caso contrario.</returns>
        Task<bool> LeaveMatchAsync(string matchId, string userId);
    }
}