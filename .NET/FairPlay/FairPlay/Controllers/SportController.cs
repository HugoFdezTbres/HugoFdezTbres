using Microsoft.AspNetCore.Mvc;
using FairPlay.Models;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using FairPlay.Services.Interface;

namespace FairPlay.Controllers
{
    /// <summary>
    /// Controlador que gestiona las operaciones relacionadas con los deportes en el sistema.
    /// Proporciona endpoints para crear, leer, actualizar y eliminar deportes.
    /// </summary>
    [Route("api/sports")]
    [ApiController]
    public class SportController : ControllerBase
    {
        /// <summary>
        /// Servicio que proporciona funcionalidad para la gestión de deportes.
        /// </summary>
        private readonly ISportService _sportService;

        /// <summary>
        /// Constructor que inicializa el controlador con las dependencias necesarias.
        /// </summary>
        /// <param name="sportService">Servicio para la gestión de deportes.</param>
        public SportController(ISportService sportService)
        {
            _sportService = sportService;
        }

        /// <summary>
        /// Obtiene todos los deportes registrados en el sistema.
        /// </summary>
        /// <returns>Lista de todos los deportes disponibles.</returns>
        // GET: api/sports
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Sport>>> GetAll()
        {
            var sports = await _sportService.GetAllAsync();
            return Ok(sports);
        }

        /// <summary>
        /// Obtiene un deporte específico por su identificador único.
        /// </summary>
        /// <param name="id">Identificador único del deporte.</param>
        /// <returns>El deporte solicitado o un error 404 si no se encuentra.</returns>
        // GET: api/sports/{id}
        [HttpGet("{id}")]
        public async Task<ActionResult<Sport>> GetById(string id)
        {
            var sport = await _sportService.GetByIdAsync(id);
            
            if (sport == null)
                return NotFound(new { message = "Deporte no encontrado" });

            return Ok(sport);
        }

        /// <summary>
        /// Crea un nuevo deporte en el sistema.
        /// </summary>
        /// <param name="sport">Datos del deporte a crear.</param>
        /// <returns>El deporte creado y la ruta para acceder a él.</returns>
        // POST: api/sports
        [HttpPost]
        [Authorize]
        public async Task<ActionResult<Sport>> Create(Sport sport)
        {
            await _sportService.CreateAsync(sport);
            return CreatedAtAction(nameof(GetById), new { id = sport.Id }, sport);
        }

        /// <summary>
        /// Actualiza la información de un deporte existente.
        /// </summary>
        /// <param name="id">Identificador único del deporte a actualizar.</param>
        /// <param name="sport">Nuevos datos del deporte.</param>
        /// <returns>Respuesta sin contenido (204) si la actualización es exitosa, o error 404 si no se encuentra el deporte.</returns>
        // PUT: api/sports/{id}
        [HttpPut("{id}")]
        [Authorize]
        public async Task<IActionResult> Update(string id, Sport sport)
        {
            var existingSport = await _sportService.GetByIdAsync(id);
            
            if (existingSport == null)
                return NotFound(new { message = "Deporte no encontrado" });

            sport.Id = id;
            await _sportService.UpdateAsync(id, sport);

            return NoContent();
        }

        /// <summary>
        /// Elimina un deporte del sistema.
        /// </summary>
        /// <param name="id">Identificador único del deporte a eliminar.</param>
        /// <returns>Respuesta sin contenido (204) si la eliminación es exitosa, o error 404 si no se encuentra el deporte.</returns>
        // DELETE: api/sports/{id}
        [HttpDelete("{id}")]
        [Authorize]
        public async Task<IActionResult> Delete(string id)
        {
            var sport = await _sportService.GetByIdAsync(id);
            
            if (sport == null)
                return NotFound(new { message = "Deporte no encontrado" });

            await _sportService.DeleteAsync(id);

            return NoContent();
        }
    }
}