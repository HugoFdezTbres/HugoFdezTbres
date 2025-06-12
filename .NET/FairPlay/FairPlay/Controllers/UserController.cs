using Microsoft.AspNetCore.Mvc;
using FairPlay.Models;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using FairPlay.Services.Interface;
using System.Text;

namespace FairPlay.Controllers
{
    [Route("api/users")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly IUserService _userService;

        public UserController(IUserService userService)
        {
            _userService = userService;
        }

        // POST: api/users/register
        [HttpPost("register")]
        public async Task<ActionResult<User>> Register(RegisterRequest registerRequest)
        {
            try
            {
                var user = await _userService.RegisterAsync(registerRequest);
                return CreatedAtAction(nameof(GetById), new { id = user.Id }, new {
                    id = user.Id,
                    name = user.Name,
                    lastName = user.LastName,
                    gender = user.Gender,
                    email = user.Email,
                    phoneNumber = user.PhoneNumber,
                    address = user.Address
                });
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }

        // POST: api/users/login
        [HttpPost("login")]
        public async Task<ActionResult<LoginResponse>> Login(LoginRequest loginRequest)
        {
            var response = await _userService.AuthenticateAsync(loginRequest);
            
            if (response == null)
                return Unauthorized(new { message = "Email o contraseña incorrectos" });

            return Ok(response);
        }

        // GET: api/users
        [HttpGet]
        [Authorize]
        public async Task<ActionResult<IEnumerable<User>>> GetAll()
        {
            var users = await _userService.GetAllAsync();
            return Ok(users);
        }

        // GET: api/users/{id}
        [HttpGet("{id:length(24)}")]
        [Authorize]
        public async Task<ActionResult<User>> GetById(string id)
        {
            var user = await _userService.GetByIdAsync(id);

            if (user == null)
                return NotFound();

            return Ok(user);
        }

        // GET: api/users/profile
        [HttpGet("profile")]
        [Authorize]
        public async Task<ActionResult<User>> GetProfile()
        {
            // Obtener el ID del usuario del token JWT
            var userId = User.FindFirst("id")?.Value;
            
            if (string.IsNullOrEmpty(userId))
                return Unauthorized(new { message = "Usuario no autenticado" });

            var user = await _userService.GetByIdAsync(userId);

            if (user == null)
                return NotFound();

            // No devolver la contraseña en la respuesta
            user.Password = null;

            return Ok(user);
        }

        // PUT: api/users/{id}
        [HttpPut("{id:length(24)}")]
        [Authorize]
        public async Task<IActionResult> Update(string id, User updatedUser)
        {
            var user = await _userService.GetByIdAsync(id);

            if (user == null)
                return NotFound();

            // Verificar que el usuario solo pueda actualizar su propio perfil
            var userId = User.FindFirst("id")?.Value;
            if (userId != id)
                return Forbid();

            // Mantener la contraseña actual si no se proporciona una nueva
            if (string.IsNullOrEmpty(updatedUser.Password))
            {
                updatedUser.Password = user.Password;
            }
            else
            {
                // Hashear la nueva contraseña
                updatedUser.Password = Convert.ToBase64String(Encoding.UTF8.GetBytes(updatedUser.Password));
            }

            updatedUser.Id = user.Id;
            await _userService.UpdateAsync(id, updatedUser);

            return NoContent();
        }

        // DELETE: api/users/{id}
        [HttpDelete("{id:length(24)}")]
        [Authorize]
        public async Task<IActionResult> Delete(string id)
        {
            var user = await _userService.GetByIdAsync(id);

            if (user == null)
                return NotFound();

            // Verificar que el usuario solo pueda eliminar su propio perfil
            var userId = User.FindFirst("id")?.Value;
            if (userId != id)
                return Forbid();

            await _userService.DeleteAsync(id);
            return NoContent();
        }
    }
}