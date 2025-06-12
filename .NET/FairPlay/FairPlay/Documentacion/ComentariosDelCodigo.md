# Comentarios del Código - Proyecto FairPlay

## Estructura General

FairPlay es una aplicación web ASP.NET Core que gestiona reservas de pistas deportivas, partidos, deportes y usuarios. Utiliza MongoDB como base de datos y JWT para autenticación.

## Archivos Principales y su Funcionalidad

### Program.cs

```csharp
// Configuración principal de la aplicación
```

Este archivo es el punto de entrada de la aplicación y contiene:

- Configuración de servicios de MongoDB
- Registro de servicios de la aplicación (ICourtService, IUserService, ISportService, etc.)
- Configuración de autenticación JWT con una clave secreta
- Configuración de Swagger para documentación de API
- Configuración del pipeline HTTP (middleware)

Conexiones:
- Se conecta a MongoDB según la configuración en appsettings.json
- Configura JWT para autenticación de usuarios

### Modelos (Models/)

#### User.cs

```csharp
// Modelo de usuario con atributos para MongoDB
```

Representa a los usuarios de la aplicación con:
- Información personal (nombre, apellido, teléfono)
- Credenciales (email, contraseña) - Ya no incluye username
- Dirección completa del usuario
- Imagen de perfil (opcional)
- Deporte favorito (referencia a Sport)
- Fecha de registro

También contiene clases auxiliares:
- `LoginRequest`: Para solicitudes de inicio de sesión (ahora con email en lugar de username)
- `RegisterRequest`: Para solicitudes de registro
- `LoginResponse`: Para respuestas de autenticación con token JWT

#### Court.cs

```csharp
// Modelo de pista deportiva
```

Representa las pistas deportivas con:
- Nombre y deportes disponibles
- Dirección completa (calle, número, ciudad, etc.)
- Horarios de apertura y cierre
- Lista de pistas disponibles
- Imágenes de la instalación deportiva

Incluye una clase anidada `Address` para gestionar la información de ubicación.

#### Match.cs

```csharp
// Modelo de partido
```

Representa los partidos organizados con:
- Referencia a la pista (CourtId)
- Fecha y horario (startTime, endTime)
- Tipo de deporte (ahora con referencia a Sport)
- Lista de jugadores y número máximo
- Estado del partido (abierto, cerrado, cancelado)
- Usuario creador y fecha de creación

#### Reservation.cs

```csharp
// Modelo de reserva
```

Representa las reservas de pistas con:
- Referencias a la pista (CourtId) y usuario (UserId)
- Fecha y horario (startTime, endTime)
- Tipo de deporte (ahora con referencia a Sport)
- Estado de la reserva (confirmada, cancelada, completada)
- Precio y estado del pago
- Opciones de modificación y cancelación

### Modelos (Models/)

#### Sport.cs

```csharp
// Modelo de deporte
```

Representa los deportes disponibles en el sistema con:
- Nombre del deporte
- Imagen representativa del deporte
- Descripción y reglas básicas
- Número de jugadores recomendado

Este modelo se utiliza como referencia en los modelos User, Court, Match y Reservation.

### Controladores (Controllers/)

#### UserController.cs

```csharp
// Controlador para gestión de usuarios
```

Gestiona todas las operaciones relacionadas con usuarios:
- Registro e inicio de sesión (sin autenticación)
- Obtención, actualización y eliminación de perfiles (con autenticación)
- Implementa autorización para que los usuarios solo puedan modificar sus propios datos

Endpoints principales:
- POST /api/users/register: Registro de nuevos usuarios (ahora con dirección y deporte favorito)
- POST /api/users/login: Autenticación con email y generación de token JWT
- GET /api/users/profile: Obtención del perfil del usuario autenticado
- PUT /api/users/profile: Actualización del perfil incluyendo imagen y deporte favorito

#### CourtController.cs

```csharp
// Controlador para gestión de pistas
```

Gestiona las operaciones CRUD para pistas deportivas:
- Listado y búsqueda de pistas
- Creación, actualización y eliminación de pistas

Endpoints principales:
- GET /api/courts: Obtener todas las pistas
- GET /api/courts/{id}: Obtener una pista específica
- POST /api/courts: Crear una nueva pista

#### MatchController.cs

```csharp
// Controlador para gestión de partidos
```

Gestiona los partidos entre usuarios:
- Creación y gestión de partidos
- Unirse o abandonar partidos
- Filtrado de partidos por pista, usuario o disponibilidad

Endpoints principales:
- GET /api/matches/available: Obtener partidos disponibles
- POST /api/matches/{id}/join: Unirse a un partido
- POST /api/matches/{id}/leave: Abandonar un partido

Implementa autorización para que solo el creador pueda modificar o eliminar un partido.

#### ReservationController.cs

```csharp
// Controlador para gestión de reservas
```

Gestiona las reservas de pistas:
- Creación y gestión de reservas
- Verificación de disponibilidad de horarios
- Cancelación de reservas
- Modificación de reservas existentes

Endpoints principales:
- GET /api/reservations/user: Obtener reservas del usuario actual
- GET /api/reservations/available: Verificar disponibilidad de horarios
- POST /api/reservations/{id}/cancel: Cancelar una reserva
- PUT /api/reservations/{id}: Modificar una reserva existente

Implementa verificaciones de disponibilidad para evitar conflictos de horarios y autorización para que los usuarios solo puedan gestionar sus propias reservas.

#### SportController.cs

```csharp
// Controlador para gestión de deportes
```

Gestiona las operaciones relacionadas con deportes:
- Listado y búsqueda de deportes disponibles
- Creación, actualización y eliminación de deportes

Endpoints principales:
- GET /api/sports: Obtener todos los deportes
- GET /api/sports/{id}: Obtener un deporte específico
- POST /api/sports: Crear un nuevo deporte
- PUT /api/sports/{id}: Actualizar un deporte existente
- DELETE /api/sports/{id}: Eliminar un deporte

Implementa autorización para que solo los administradores puedan crear, modificar o eliminar deportes.

### Servicios (Services/)

#### UserService.cs

```csharp
// Servicio para operaciones con usuarios
```

Implementa la lógica de negocio para usuarios:
- Autenticación y generación de tokens JWT
- Registro con validación de email único (ya no username)
- Operaciones CRUD en la colección "Users" de MongoDB
- Gestión de imágenes de perfil

Funcionalidades importantes:
- Hashing de contraseñas (implementación básica)
- Generación de tokens JWT con claims de usuario
- Verificación de credenciales con email
- Actualización de perfil incluyendo deporte favorito

#### CourtService.cs

```csharp
// Servicio para operaciones con pistas
```

Implementa operaciones CRUD para pistas deportivas en la colección "Courts" de MongoDB.

#### MatchService.cs

```csharp
// Servicio para operaciones con partidos
```

Implementa la lógica de negocio para partidos:
- Operaciones CRUD en la colección "Matches" de MongoDB
- Funcionalidad para unirse o abandonar partidos
- Filtrado de partidos por diferentes criterios

#### ReservationService.cs

```csharp
// Servicio para operaciones con reservas
```

Implementa la lógica de negocio para reservas:
- Operaciones CRUD en la colección "Reservations" de MongoDB
- Verificación de disponibilidad de horarios
- Cancelación de reservas
- Modificación de reservas existentes

Funcionalidad importante:
- `IsTimeSlotAvailableAsync`: Verifica si un horario está disponible para reserva
- `ModifyReservationAsync`: Permite modificar una reserva existente

#### SportService.cs

```csharp
// Servicio para operaciones con deportes
```

Implementa la lógica de negocio para deportes:
- Operaciones CRUD en la colección "Sports" de MongoDB
- Búsqueda de deportes por nombre o ID
- Gestión de imágenes de deportes

Funcionalidades importantes:
- `GetByNameAsync`: Busca un deporte por su nombre
- `GetAllAsync`: Obtiene todos los deportes disponibles
- `CreateAsync`: Crea un nuevo deporte con su imagen

### Configuración

#### MongoDBSettings.cs

```csharp
// Configuración de conexión a MongoDB
```

Clase para configurar la conexión a MongoDB con:
- ConnectionString: URL de conexión a MongoDB
- DatabaseName: Nombre de la base de datos
- CollectionName: Nombre de la colección principal

#### appsettings.json

```json
// Configuración de la aplicación
```

Contiene la configuración principal:
- Conexión a MongoDB (localhost:27017)
- Configuración de JWT con clave secreta
- Configuración de logging

## Conexiones y Dependencias

1. **Base de Datos**: La aplicación se conecta a MongoDB local (mongodb://localhost:27017) para almacenar todos los datos en la base de datos "FairPlay".

2. **Autenticación**: Utiliza JWT para autenticar usuarios y proteger endpoints. La clave secreta está definida en appsettings.json.

3. **Swagger**: Integra Swagger/OpenAPI para documentación de la API, accesible en modo desarrollo.

## Flujos Principales

1. **Registro y Autenticación**:
   - Usuario se registra → UserController → UserService → MongoDB
   - Usuario inicia sesión con email → UserController → UserService (genera JWT) → Cliente recibe token

2. **Reserva de Pista**:
   - Usuario busca pistas disponibles → CourtController → CourtService → MongoDB
   - Usuario selecciona deporte → SportController → SportService → MongoDB
   - Usuario verifica disponibilidad → ReservationController → ReservationService → MongoDB
   - Usuario crea reserva → ReservationController → ReservationService → MongoDB

3. **Gestión de Partidos**:
   - Usuario crea partido → MatchController → MatchService → MongoDB
   - Otros usuarios se unen → MatchController → MatchService → MongoDB

4. **Gestión de Deportes**:
   - Administrador crea deportes → SportController → SportService → MongoDB
   - Usuario selecciona deporte favorito → UserController → UserService → MongoDB

## Seguridad

- Autenticación mediante JWT
- Autorización basada en claims para proteger recursos
- Validación para que los usuarios solo accedan a sus propios datos
- Hashing básico de contraseñas (podría mejorarse con BCrypt en producción)