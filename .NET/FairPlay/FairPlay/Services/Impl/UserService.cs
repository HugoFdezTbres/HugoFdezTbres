using FairPlay.Models;
using Microsoft.Extensions.Options;
using MongoDB.Driver;
using FairPlay.Data;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Security.Claims;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Text;
using Microsoft.Extensions.Configuration;
using FairPlay.Services.Interface;

namespace FairPlay.Services.Impl
{
    /// <summary>
    /// Implementación de la interfaz IUserService que proporciona funcionalidad para la gestión de usuarios.
    /// Esta clase interactúa con la colección "Users" en MongoDB para realizar operaciones CRUD y autenticación.
    /// </summary>
    public class UserService : IUserService
    {
        private readonly IMongoCollection<User> _usersCollection;
        private readonly IConfiguration _configuration;

        /// <summary>
        /// Constructor que inicializa la conexión a la base de datos MongoDB y configura la colección de usuarios.
        /// </summary>
        /// <param name="mongoDBSettings">Configuración de conexión a MongoDB.</param>
        /// <param name="configuration">Configuración de la aplicación para acceder a valores como la clave secreta JWT.</param>
        public UserService(IOptions<MongoDBSettings> mongoDBSettings, IConfiguration configuration)
        {
            var client = new MongoClient(mongoDBSettings.Value.ConnectionString);
            var database = client.GetDatabase(mongoDBSettings.Value.DatabaseName);
            _usersCollection = database.GetCollection<User>("Users");
            _configuration = configuration;
        }

        /// <summary>
        /// Obtiene todos los usuarios registrados en el sistema.
        /// </summary>
        /// <returns>Lista de todos los usuarios.</returns>
        public async Task<List<User>> GetAllAsync() =>
            await _usersCollection.Find(_ => true).ToListAsync();

        /// <summary>
        /// Busca un usuario por su identificador único.
        /// </summary>
        /// <param name="id">Identificador único del usuario.</param>
        /// <returns>Usuario encontrado o null si no existe.</returns>
        public async Task<User> GetByIdAsync(string id) =>
            await _usersCollection.Find(u => u.Id == id).FirstOrDefaultAsync();

        /// <summary>
        /// Busca un usuario por su dirección de correo electrónico.
        /// </summary>
        /// <param name="email">Dirección de correo electrónico del usuario.</param>
        /// <returns>Usuario encontrado o null si no existe.</returns>
        public async Task<User> GetByEmailAsync(string email) =>
            await _usersCollection.Find(u => u.Email == email).FirstOrDefaultAsync();



        /// <summary>
        /// Autentica a un usuario verificando sus credenciales y genera un token JWT si son válidas.
        /// </summary>
        /// <param name="loginRequest">Objeto con las credenciales de inicio de sesión (email y contraseña).</param>
        /// <returns>Respuesta con el token JWT y la información del usuario, o null si la autenticación falla.</returns>
        public async Task<LoginResponse> AuthenticateAsync(LoginRequest loginRequest)
        {
            var user = await GetByEmailAsync(loginRequest.Email);

            // Verificar si el usuario existe y la contraseña es correcta
            if (user == null || !VerifyPassword(loginRequest.Password, user.Password))
                return null;

            // Generar token JWT
            var token = GenerateJwtToken(user);

            return new LoginResponse
            {
                Id = user.Id,
                Name = user.Name,
                LastName = user.LastName,
                Gender = user.Gender,
                Email = user.Email,
                Token = token
            };
        }

        public async Task<User> RegisterAsync(RegisterRequest registerRequest)
        {
            // Verificar si el email ya existe
            if (await GetByEmailAsync(registerRequest.Email) != null)
                throw new Exception("El correo electrónico ya está registrado");

            var user = new User
            {
                Email = registerRequest.Email,
                Password = HashPassword(registerRequest.Password),
                Name = registerRequest.Name,
                LastName = registerRequest.LastName,
                Gender = registerRequest.Gender,
                PhoneNumber = registerRequest.PhoneNumber,
                Address = registerRequest.Address,
                RegistrationDate = DateTime.Now
            };

            await _usersCollection.InsertOneAsync(user);
            return user;
        }

        public async Task UpdateAsync(string id, User user) =>
            await _usersCollection.ReplaceOneAsync(u => u.Id == id, user);

        public async Task DeleteAsync(string id) =>
            await _usersCollection.DeleteOneAsync(u => u.Id == id);

        // Métodos auxiliares para hash de contraseña y generación de token
        private string HashPassword(string password)
        {
            // En un entorno de producción, usar un algoritmo de hash seguro como BCrypt
            // Para simplificar, usamos una implementación básica
            return Convert.ToBase64String(Encoding.UTF8.GetBytes(password));
        }

        private bool VerifyPassword(string inputPassword, string storedPassword)
        {
            // Verificar el hash de la contraseña
            var hashedInput = Convert.ToBase64String(Encoding.UTF8.GetBytes(inputPassword));
            return hashedInput == storedPassword;
        }

        private string GenerateJwtToken(User user)
        {
            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.ASCII.GetBytes(_configuration["Jwt:Key"] ?? "FairPlayDefaultSecretKey12345678901234567890");

            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new[]
                {
                    new Claim("id", user.Id),
                    new Claim(ClaimTypes.Name, user.Name + " " + user.LastName),
                    new Claim(ClaimTypes.Email, user.Email)
                }),
                Expires = DateTime.UtcNow.AddDays(7),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
            };

            var token = tokenHandler.CreateToken(tokenDescriptor);
            return tokenHandler.WriteToken(token);
        }
    }
}