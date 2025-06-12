using Microsoft.AspNetCore.Mvc;
using FairPlay.Models;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using FairPlay.Services.Interface;

namespace FairPlay.Controllers
{
    [Route("api/reservations")]
    [ApiController]
    public class ReservationController : ControllerBase
    {
        private readonly IReservationService _reservationService;
        private readonly ICourtService _courtService;

        public ReservationController(IReservationService reservationService, ICourtService courtService)
        {
            _reservationService = reservationService;
            _courtService = courtService;
        }

        // GET: api/reservations
        [HttpGet]
        [Authorize]
        public async Task<ActionResult<IEnumerable<Reservation>>> GetAll()
        {
            // Solo administradores deberían poder ver todas las reservas
            var reservations = await _reservationService.GetAllAsync();
            return Ok(reservations);
        }

        // GET: api/reservations/{id}
        [HttpGet("{id:length(24)}")]
        [Authorize]
        public async Task<ActionResult<Reservation>> GetById(string id)
        {
            var reservation = await _reservationService.GetByIdAsync(id);

            if (reservation == null)
                return NotFound();

            // Verificar que el usuario solo pueda ver sus propias reservas
            var userId = User.FindFirst("id")?.Value;
            if (userId != reservation.UserId)
                return Forbid();

            return Ok(reservation);
        }

        // GET: api/reservations/user
        [HttpGet("user")]
        [Authorize]
        public async Task<ActionResult<IEnumerable<Reservation>>> GetUserReservations()
        {
            var userId = User.FindFirst("id")?.Value;
            if (string.IsNullOrEmpty(userId))
                return Unauthorized();

            var reservations = await _reservationService.GetByUserIdAsync(userId);
            return Ok(reservations);
        }

        // GET: api/reservations/court/{courtId}
        [HttpGet("court/{courtId:length(24)}")]
        public async Task<ActionResult<IEnumerable<Reservation>>> GetByCourtId(string courtId)
        {
            var court = await _courtService.GetByIdAsync(courtId);
            if (court == null)
                return NotFound(new { message = "Pista no encontrada" });

            var reservations = await _reservationService.GetByCourtIdAsync(courtId);
            return Ok(reservations);
        }

        // GET: api/reservations/date/{date}
        [HttpGet("date/{date}")]
        public async Task<ActionResult<IEnumerable<Reservation>>> GetByDate(DateTime date)
        {
            var reservations = await _reservationService.GetByDateAsync(date);
            return Ok(reservations);
        }

        // GET: api/reservations/available
        [HttpGet("available")]
        public async Task<ActionResult<bool>> CheckAvailability([FromQuery] string courtId, [FromQuery] DateTime date, [FromQuery] DateTime startTime, [FromQuery] DateTime endTime)
        {
            if (string.IsNullOrEmpty(courtId))
                return BadRequest(new { message = "El ID de la pista es requerido" });

            var isAvailable = await _reservationService.IsTimeSlotAvailableAsync(courtId, date, startTime, endTime);
            return Ok(new { isAvailable });
        }

        // POST: api/reservations
        [HttpPost]
        [Authorize]
        public async Task<ActionResult<Reservation>> Create(Reservation reservation)
        {
            // Establecer el usuario actual como el que hace la reserva
            var userId = User.FindFirst("id")?.Value;
            if (string.IsNullOrEmpty(userId))
                return Unauthorized();

            reservation.UserId = userId;

            // Verificar que la pista exista
            var court = await _courtService.GetByIdAsync(reservation.CourtId);
            if (court == null)
                return NotFound(new { message = "Pista no encontrada" });

            // Verificar disponibilidad
            var isAvailable = await _reservationService.IsTimeSlotAvailableAsync(
                reservation.CourtId, reservation.Date, reservation.StartTime, reservation.EndTime);

            if (!isAvailable)
                return BadRequest(new { message = "El horario seleccionado no está disponible" });

            await _reservationService.CreateAsync(reservation);
            return CreatedAtAction(nameof(GetById), new { id = reservation.Id }, reservation);
        }

        // PUT: api/reservations/{id}
        [HttpPut("{id:length(24)}")]
        [Authorize]
        public async Task<IActionResult> Update(string id, Reservation updatedReservation)
        {
            var reservation = await _reservationService.GetByIdAsync(id);

            if (reservation == null)
                return NotFound();

            // Verificar que el usuario solo pueda actualizar sus propias reservas
            var userId = User.FindFirst("id")?.Value;
            if (userId != reservation.UserId)
                return Forbid();

            // Si se cambia la fecha/hora, verificar disponibilidad
            if (updatedReservation.Date != reservation.Date ||
                updatedReservation.StartTime != reservation.StartTime ||
                updatedReservation.EndTime != reservation.EndTime ||
                updatedReservation.CourtId != reservation.CourtId)
            {
                var isAvailable = await _reservationService.IsTimeSlotAvailableAsync(
                    updatedReservation.CourtId, updatedReservation.Date, 
                    updatedReservation.StartTime, updatedReservation.EndTime);

                if (!isAvailable)
                    return BadRequest(new { message = "El horario seleccionado no está disponible" });
            }

            updatedReservation.Id = reservation.Id;
            updatedReservation.UserId = reservation.UserId;
            updatedReservation.CreatedAt = reservation.CreatedAt;

            await _reservationService.UpdateAsync(id, updatedReservation);
            return NoContent();
        }

        // POST: api/reservations/{id}/cancel
        [HttpPost("{id:length(24)}/cancel")]
        [Authorize]
        public async Task<IActionResult> CancelReservation(string id)
        {
            var reservation = await _reservationService.GetByIdAsync(id);

            if (reservation == null)
                return NotFound();

            // Verificar que el usuario solo pueda cancelar sus propias reservas
            var userId = User.FindFirst("id")?.Value;
            if (userId != reservation.UserId)
                return Forbid();

            await _reservationService.CancelReservationAsync(id);
            return NoContent();
        }

        // DELETE: api/reservations/{id}
        [HttpDelete("{id:length(24)}")]
        [Authorize]
        public async Task<IActionResult> Delete(string id)
        {
            var reservation = await _reservationService.GetByIdAsync(id);

            if (reservation == null)
                return NotFound();

            // Verificar que el usuario solo pueda eliminar sus propias reservas
            var userId = User.FindFirst("id")?.Value;
            if (userId != reservation.UserId)
                return Forbid();

            await _reservationService.DeleteAsync(id);
            return NoContent();
        }
    }
}