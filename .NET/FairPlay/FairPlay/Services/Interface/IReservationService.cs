using FairPlay.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace FairPlay.Services.Interface
{
    /// <summary>
    /// Interfaz que define las operaciones disponibles para la gestión de reservas de pistas en el sistema FairPlay.
    /// Esta interfaz es implementada por ReservationService que interactúa con la colección "Reservations" en MongoDB.
    /// Permite la creación, búsqueda, actualización, cancelación y eliminación de reservas de pistas deportivas.
    /// </summary>
    public interface IReservationService
    {
        /// <summary>
        /// Obtiene todas las reservas registradas en el sistema.
        /// </summary>
        /// <returns>Una lista de todas las reservas.</returns>
        Task<List<Reservation>> GetAllAsync();

        /// <summary>
        /// Busca una reserva por su identificador único.
        /// </summary>
        /// <param name="id">El identificador único de la reserva.</param>
        /// <returns>La reserva encontrada o null si no existe.</returns>
        Task<Reservation> GetByIdAsync(string id);

        /// <summary>
        /// Obtiene las reservas realizadas por un usuario específico.
        /// </summary>
        /// <param name="userId">El identificador único del usuario.</param>
        /// <returns>Lista de reservas realizadas por el usuario.</returns>
        Task<List<Reservation>> GetByUserIdAsync(string userId);

        /// <summary>
        /// Obtiene las reservas para una pista específica.
        /// </summary>
        /// <param name="courtId">El identificador único de la pista.</param>
        /// <returns>Lista de reservas para la pista especificada.</returns>
        Task<List<Reservation>> GetByCourtIdAsync(string courtId);

        /// <summary>
        /// Obtiene las reservas para una fecha específica.
        /// </summary>
        /// <param name="date">La fecha para la cual se buscan reservas.</param>
        /// <returns>Lista de reservas para la fecha especificada.</returns>
        Task<List<Reservation>> GetByDateAsync(DateTime date);

        /// <summary>
        /// Verifica si un horario está disponible para reserva en una pista específica.
        /// </summary>
        /// <param name="courtId">El identificador único de la pista.</param>
        /// <param name="date">La fecha para la cual se verifica disponibilidad.</param>
        /// <param name="startTime">La hora de inicio del horario a verificar.</param>
        /// <param name="endTime">La hora de fin del horario a verificar.</param>
        /// <returns>True si el horario está disponible, False en caso contrario.</returns>
        Task<bool> IsTimeSlotAvailableAsync(string courtId, DateTime date, DateTime startTime, DateTime endTime);

        /// <summary>
        /// Crea una nueva reserva en el sistema.
        /// </summary>
        /// <param name="reservation">Objeto con la información de la nueva reserva.</param>
        Task CreateAsync(Reservation reservation);

        /// <summary>
        /// Actualiza la información de una reserva existente.
        /// </summary>
        /// <param name="id">El identificador único de la reserva a actualizar.</param>
        /// <param name="reservation">Objeto con la nueva información de la reserva.</param>
        Task UpdateAsync(string id, Reservation reservation);

        /// <summary>
        /// Cancela una reserva existente sin eliminarla del sistema.
        /// </summary>
        /// <param name="id">El identificador único de la reserva a cancelar.</param>
        Task CancelReservationAsync(string id);

        /// <summary>
        /// Elimina una reserva del sistema.
        /// </summary>
        /// <param name="id">El identificador único de la reserva a eliminar.</param>
        Task DeleteAsync(string id);
    }
}