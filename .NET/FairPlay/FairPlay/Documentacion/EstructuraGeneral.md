# Documentación de FairPlay

## Estructura General del Proyecto

FairPlay es una aplicación web desarrollada con ASP.NET Core que permite gestionar reservas de pistas deportivas, partidos, deportes y usuarios. La aplicación utiliza una arquitectura de API RESTful con MongoDB como base de datos.

### Tecnologías Utilizadas

- **Backend**: ASP.NET Core 8.0
- **Base de Datos**: MongoDB
- **Autenticación**: JWT (JSON Web Tokens)
- **Documentación API**: Swagger/OpenAPI

### Estructura de Carpetas

- **Controllers/**: Controladores API que manejan las solicitudes HTTP
- **Models/**: Clases que representan las entidades de datos
- **Services/**: Servicios que implementan la lógica de negocio
- **Data/**: Configuración de la base de datos

### Componentes Principales

1. **Gestión de Usuarios**: Registro, autenticación y gestión de perfiles de usuario con imágenes y deportes favoritos
2. **Gestión de Pistas**: Administración de pistas deportivas con imágenes
3. **Gestión de Partidos**: Creación y gestión de partidos entre usuarios con tipos de deportes
4. **Sistema de Reservas**: Reserva de pistas en horarios específicos con opciones de modificación
5. **Gestión de Deportes**: Administración de los deportes disponibles en el sistema

### Conexiones Externas

- **MongoDB**: La aplicación se conecta a una base de datos MongoDB local (mongodb://localhost:27017) para almacenar todos los datos.
- **JWT**: Se utiliza para la autenticación y autorización de usuarios.

### Configuración

La configuración principal se encuentra en el archivo `appsettings.json`, que incluye:

- Configuración de MongoDB
- Configuración de JWT para autenticación
- Configuración de logging