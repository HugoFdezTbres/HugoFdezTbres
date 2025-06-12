using Microsoft.AspNetCore.Mvc;
using FairPlay.Models;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using FairPlay.Services.Interface;

namespace FairPlay.Controllers
{
    [Route("api/matches")]
    [ApiController]
    public class MatchController : ControllerBase
    {
        private readonly IMatchService _matchService;

        public MatchController(IMatchService matchService)
        {
            _matchService = matchService;
        }

        // GET: api/matches
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Match>>> GetAll()
        {
            var matches = await _matchService.GetAllAsync();
            return Ok(matches);
        }

        // GET: api/matches/{id}
        [HttpGet("{id:length(24)}")]
        public async Task<ActionResult<Match>> GetById(string id)
        {
            var match = await _matchService.GetByIdAsync(id);

            if (match == null)
                return NotFound();

            return Ok(match);
        }

        // GET: api/matches/court/{courtId}
        [HttpGet("court/{courtId:length(24)}")]
        public async Task<ActionResult<IEnumerable<Match>>> GetByCourtId(string courtId)
        {
            var matches = await _matchService.GetByCourtIdAsync(courtId);
            return Ok(matches);
        }

        // GET: api/matches/user/{userId}
        [HttpGet("user/{userId:length(24)}")]
        [Authorize]
        public async Task<ActionResult<IEnumerable<Match>>> GetByUserId(string userId)
        {
            // Verificar que el usuario solo pueda ver sus propios partidos
            var currentUserId = User.FindFirst("id")?.Value;
            if (currentUserId != userId)
                return Forbid();

            var matches = await _matchService.GetByUserIdAsync(userId);
            return Ok(matches);
        }

        // GET: api/matches/available
        [HttpGet("available")]
        public async Task<ActionResult<IEnumerable<Match>>> GetAvailableMatches()
        {
            var matches = await _matchService.GetAvailableMatchesAsync();
            return Ok(matches);
        }

        // POST: api/matches
        [HttpPost]
        [Authorize]
        public async Task<ActionResult<Match>> Create(Match match)
        {
            // Establecer el usuario actual como creador del partido
            match.CreatedBy = User.FindFirst("id")?.Value;
            if (string.IsNullOrEmpty(match.CreatedBy))
                return Unauthorized();

            // Añadir al creador como primer jugador
            if (!match.Players.Contains(match.CreatedBy))
                match.Players.Add(match.CreatedBy);

            await _matchService.CreateAsync(match);
            return CreatedAtAction(nameof(GetById), new { id = match.Id }, match);
        }

        // PUT: api/matches/{id}
        [HttpPut("{id:length(24)}")]
        [Authorize]
        public async Task<IActionResult> Update(string id, Match updatedMatch)
        {
            var match = await _matchService.GetByIdAsync(id);

            if (match == null)
                return NotFound();

            // Verificar que solo el creador pueda actualizar el partido
            var userId = User.FindFirst("id")?.Value;
            if (userId != match.CreatedBy)
                return Forbid();

            updatedMatch.Id = match.Id;
            updatedMatch.CreatedBy = match.CreatedBy;
            updatedMatch.CreatedAt = match.CreatedAt;

            await _matchService.UpdateAsync(id, updatedMatch);
            return NoContent();
        }

        // DELETE: api/matches/{id}
        [HttpDelete("{id:length(24)}")]
        [Authorize]
        public async Task<IActionResult> Delete(string id)
        {
            var match = await _matchService.GetByIdAsync(id);

            if (match == null)
                return NotFound();

            // Verificar que solo el creador pueda eliminar el partido
            var userId = User.FindFirst("id")?.Value;
            if (userId != match.CreatedBy)
                return Forbid();

            await _matchService.DeleteAsync(id);
            return NoContent();
        }

        // POST: api/matches/{id}/join
        [HttpPost("{id:length(24)}/join")]
        [Authorize]
        public async Task<IActionResult> JoinMatch(string id)
        {
            var userId = User.FindFirst("id")?.Value;
            if (string.IsNullOrEmpty(userId))
                return Unauthorized();

            var success = await _matchService.JoinMatchAsync(id, userId);

            if (!success)
                return BadRequest(new { message = "No se pudo unir al partido" });

            return NoContent();
        }

        // POST: api/matches/{id}/leave
        [HttpPost("{id:length(24)}/leave")]
        [Authorize]
        public async Task<IActionResult> LeaveMatch(string id)
        {
            var userId = User.FindFirst("id")?.Value;
            if (string.IsNullOrEmpty(userId))
                return Unauthorized();

            var success = await _matchService.LeaveMatchAsync(id, userId);

            if (!success)
                return BadRequest(new { message = "No se pudo abandonar el partido" });

            return NoContent();
        }
    }
}