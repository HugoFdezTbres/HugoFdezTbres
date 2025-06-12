using Microsoft.AspNetCore.Mvc;
using FairPlay.Models;
using System.Collections.Generic;
using System.Threading.Tasks;
using FairPlay.Services.Interface;

namespace FairPlay.Controllers
{
    [Route("api/courts")]
    [ApiController]
    public class CourtController : ControllerBase
    {
        private readonly ICourtService _courtService;

        public CourtController(ICourtService courtService)
        {
            _courtService = courtService;
        }

        // GET: api/courts
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Court>>> Get()
        {
            var courts = await _courtService.GetAllAsync();
            return Ok(courts);
        }

        // GET api/courts/5
        [HttpGet("{id:length(24)}")]
        public async Task<ActionResult<Court>> Get(string id)
        {
            var court = await _courtService.GetByIdAsync(id);

            if (court == null)
            {
                return NotFound();
            }

            return Ok(court);
        }

        // POST api/courts
        [HttpPost]
        public async Task<ActionResult<Court>> Post(Court court)
        {
            var createdCourt = await _courtService.CreateAsync(court);
            return CreatedAtAction(nameof(Get), new { id = createdCourt.id_court }, createdCourt);
        }

        // PUT api/courts/5
        [HttpPut("{id:length(24)}")]
        public async Task<IActionResult> Put(string id, Court updatedCourt)
        {
            var court = await _courtService.GetByIdAsync(id);

            if (court == null)
            {
                return NotFound();
            }

            updatedCourt.id_court = court.id_court;
            await _courtService.UpdateAsync(id, updatedCourt);

            return NoContent();
        }

        // DELETE api/courts/5
        [HttpDelete("{id:length(24)}")]
        public async Task<IActionResult> Delete(string id)
        {
            var court = await _courtService.GetByIdAsync(id);

            if (court == null)
            {
                return NotFound();
            }

            await _courtService.DeleteAsync(id);
            return NoContent();
        }
    }
}
