# Interfaces de Servicios - Proyecto FairPlay

Este documento detalla las interfaces de servicios utilizadas en el proyecto FairPlay, explicando su propósito y los métodos que definen.

## IUserService

```csharp
// Interfaz para operaciones con usuarios
```

Esta interfaz define las operaciones disponibles para gestionar usuarios:

- `GetAllAsync()`: Obtiene todos los usuarios registrados en el sistema.
- `GetByIdAsync(string id)`: Busca un usuario por su identificador único.
- `GetByEmailAsync(string email)`: Busca un usuario por su dirección de correo electrónico.
- `AuthenticateAsync(LoginRequest loginRequest)`: Autentica a un usuario con email y genera un token JWT.
- `RegisterAsync(RegisterRequest registerRequest)`: Registra un nuevo usuario en el sistema con dirección y deporte favorito.
- `UpdateAsync(string id, User user)`: Actualiza la información de un usuario existente, incluyendo imagen de perfil.
- `UpdateFavoriteSportAsync(string userId, string sportId)`: Actualiza el deporte favorito de un usuario.
- `DeleteAsync(string id)`: Elimina un usuario del sistema.

Esta interfaz es implementada por `UserService` que interactúa con la colección "Users" en MongoDB.

## ICourtService

```csharp
// Interfaz para operaciones con pistas deportivas
```

Esta interfaz define las operaciones disponibles para gestionar pistas deportivas:

- `GetAllAsync()`: Obtiene todas las pistas deportivas registradas.
- `GetByIdAsync(string id)`: Busca una pista por su identificador único.
- `CreateAsync(Court court)`: Crea una nueva pista deportiva con imágenes.
- `UpdateAsync(string id, Court court)`: Actualiza la información de una pista existente.
- `AddImageAsync(string courtId, string imageUrl)`: Añade una imagen a una pista deportiva.
- `DeleteAsync(string id)`: Elimina una pista del sistema.

Esta interfaz es implementada por `CourtService` que interactúa con la colección "Courts" en MongoDB.

## ISportService

```csharp
// Interfaz para operaciones con deportes
```

Esta interfaz define las operaciones disponibles para gestionar deportes:

- `GetAllAsync()`: Obtiene todos los deportes registrados en el sistema.
- `GetByIdAsync(string id)`: Busca un deporte por su identificador único.
- `GetByNameAsync(string name)`: Busca un deporte por su nombre.
- `CreateAsync(Sport sport)`: Crea un nuevo deporte con su imagen.
- `UpdateAsync(string id, Sport sport)`: Actualiza la información de un deporte existente.
- `DeleteAsync(string id)`: Elimina un deporte del sistema.

Esta interfaz es implementada por `SportService` que interactúa con la colección "Sports" en MongoDB.

## IMatchService

```csharp
// Interfaz para operaciones con partidos
```

Esta interfaz define las operaciones disponibles para gestionar partidos:

- `GetAllAsync()`: Obtiene todos los partidos registrados.
- `GetByIdAsync(string id)`: Busca un partido por su identificador único.
- `GetByCourtIdAsync(string courtId)`: Obtiene los partidos programados en una pista específica.
- `GetBySportIdAsync(string sportId)`: Obtiene los partidos de un deporte específico.
- `GetByUserIdAsync(string userId)`: Obtiene los partidos en los que participa un usuario específico.
- `GetAvailableMatchesAsync()`: Obtiene los partidos disponibles para unirse.
- `CreateAsync(Match match)`: Crea un nuevo partido.
- `UpdateAsync(string id, Match match)`: Actualiza la información de un partido existente.
- `DeleteAsync(string id)`: Elimina un partido del sistema.
- `JoinMatchAsync(string matchId, string userId)`: Permite a un usuario unirse a un partido.
- `LeaveMatchAsync(string matchId, string userId)`: Permite a un usuario abandonar un partido.

Esta interfaz es implementada por `MatchService` que interactúa con la colección "Matches" en MongoDB.

## IReservationService

```csharp
// Interfaz para operaciones con reservas
```

Esta interfaz define las operaciones disponibles para gestionar reservas de pistas:

- `GetAllAsync()`: Obtiene todas las reservas registradas.
- `GetByIdAsync(string id)`: Busca una reserva por su identificador único.
- `GetByUserIdAsync(string userId)`: Obtiene las reservas realizadas por un usuario específico.
- `GetByCourtIdAsync(string courtId)`: Obtiene las reservas para una pista específica.
- `GetByDateAsync(DateTime date)`: Obtiene las reservas para una fecha específica.
- `IsTimeSlotAvailableAsync(string courtId, DateTime date, string startTime, string endTime)`: Verifica si un horario está disponible para reserva.
- `CreateAsync(Reservation reservation)`: Crea una nueva reserva.
- `UpdateAsync(string id, Reservation reservation)`: Actualiza la información de una reserva existente.
- `CancelReservationAsync(string id)`: Cancela una reserva existente.
- `DeleteAsync(string id)`: Elimina una reserva del sistema.

Esta interfaz es implementada por `ReservationService` que interactúa con la colección "Reservations" en MongoDB.

## Relaciones entre Servicios

Los servicios están diseñados para trabajar juntos en varios flujos de la aplicación:

1. **Reserva de Pista**:
   - `CourtService` proporciona información sobre las pistas disponibles.
   - `ReservationService` verifica la disponibilidad y gestiona las reservas.

2. **Creación de Partidos**:
   - `CourtService` proporciona información sobre las pistas disponibles.
   - `UserService` verifica la autenticación del usuario.
   - `MatchService` gestiona la creación y participación en partidos.

3. **Gestión de Usuarios**:
   - `UserService` maneja todo el ciclo de vida del usuario, desde el registro hasta la autenticación.
   - Proporciona tokens JWT que son utilizados por otros servicios para verificar la autorización.

Todas estas interfaces siguen el patrón de repositorio, proporcionando una capa de abstracción sobre la base de datos MongoDB y permitiendo una fácil sustitución de la implementación si fuera necesario en el futuro.