using FairPlay.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace FairPlay.Services.Interface
{
    /// <summary>
    /// Interfaz que define las operaciones disponibles para la gestión de usuarios en el sistema FairPlay.
    /// Esta interfaz es implementada por UserService que interactúa con la colección "Users" en MongoDB.
    /// </summary>
    public interface IUserService
    {
        /// <summary>
        /// Obtiene todos los usuarios registrados en el sistema.
        /// </summary>
        /// <returns>Una lista de todos los usuarios.</returns>
        Task<List<User>> GetAllAsync();

        /// <summary>
        /// Busca un usuario por su identificador único.
        /// </summary>
        /// <param name="id">El identificador único del usuario.</param>
        /// <returns>El usuario encontrado o null si no existe.</returns>
        Task<User> GetByIdAsync(string id);

        /// <summary>
        /// Busca un usuario por su dirección de correo electrónico.
        /// </summary>
        /// <param name="email">La dirección de correo electrónico del usuario.</param>
        /// <returns>El usuario encontrado o null si no existe.</returns>
        Task<User> GetByEmailAsync(string email);



        /// <summary>
        /// Autentica a un usuario y genera un token JWT para su sesión.
        /// </summary>
        /// <param name="loginRequest">Objeto con las credenciales de inicio de sesión.</param>
        /// <returns>Respuesta con el token JWT y la información del usuario autenticado.</returns>
        Task<LoginResponse> AuthenticateAsync(LoginRequest loginRequest);

        /// <summary>
        /// Registra un nuevo usuario en el sistema.
        /// </summary>
        /// <param name="registerRequest">Objeto con la información del nuevo usuario.</param>
        /// <returns>El usuario creado.</returns>
        Task<User> RegisterAsync(RegisterRequest registerRequest);

        /// <summary>
        /// Actualiza la información de un usuario existente.
        /// </summary>
        /// <param name="id">El identificador único del usuario a actualizar.</param>
        /// <param name="user">Objeto con la nueva información del usuario.</param>
        Task UpdateAsync(string id, User user);

        /// <summary>
        /// Elimina un usuario del sistema.
        /// </summary>
        /// <param name="id">El identificador único del usuario a eliminar.</param>
        Task DeleteAsync(string id);
    }
}