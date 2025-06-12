# Diagrama de Arquitectura - Proyecto FairPlay

Este documento presenta una visión general de la arquitectura del proyecto FairPlay, mostrando cómo se relacionan los diferentes componentes.

## Arquitectura General

```
+-------------------+      +-------------------+      +-------------------+
|                   |      |                   |      |                   |
|    Controladores  |<---->|     Servicios     |<---->|   Base de Datos   |
|    (Controllers)  |      |    (Services)     |      |    (MongoDB)      |
|                   |      |                   |      |                   |
+-------------------+      +-------------------+      +-------------------+
         ^                         ^
         |                         |
         v                         v
+-------------------+      +-------------------+
|                   |      |                   |
|      Modelos      |<---->|   Configuración   |
|     (Models)      |      |      (Data)       |
|                   |      |                   |
+-------------------+      +-------------------+
```

## Flujo de Datos

1. **Solicitud HTTP** → El cliente envía una solicitud a un endpoint de la API
2. **Controlador** → Recibe la solicitud, valida los datos y la autorización
3. **Servicio** → Implementa la lógica de negocio y realiza operaciones en la base de datos
4. **MongoDB** → Almacena o recupera los datos solicitados
5. **Respuesta** → Los datos se devuelven al cliente en formato JSON

## Componentes Principales

### Controladores (Controllers)

Gestionan las solicitudes HTTP y devuelven respuestas apropiadas:

- **UserController**: Gestión de usuarios y autenticación
- **CourtController**: Gestión de pistas deportivas
- **MatchController**: Gestión de partidos
- **ReservationController**: Gestión de reservas
- **SportController**: Gestión de deportes disponibles

### Servicios (Services)

Implementan la lógica de negocio y se comunican con la base de datos:

- **UserService**: Operaciones con usuarios y autenticación
- **CourtService**: Operaciones con pistas deportivas
- **MatchService**: Operaciones con partidos
- **ReservationService**: Operaciones con reservas
- **SportService**: Operaciones con deportes

### Modelos (Models)

Definen la estructura de los datos:

- **User**: Información de usuarios
- **Court**: Información de pistas deportivas
- **Match**: Información de partidos
- **Reservation**: Información de reservas

### Configuración (Data)

Contiene la configuración para conectarse a servicios externos:

- **MongoDBSettings**: Configuración de conexión a MongoDB

## Autenticación y Autorización

```
+-------------------+      +-------------------+      +-------------------+
|                   |      |                   |      |                   |
|  Cliente solicita |----->|  UserController   |----->|   UserService     |
|  autenticación    |      |  /api/users/login |      |  AuthenticateAsync|
|                   |      |                   |      |                   |
+-------------------+      +-------------------+      +-------------------+
                                                              |
                                                              v
+-------------------+      +-------------------+      +-------------------+
|                   |      |                   |      |                   |
|  Cliente utiliza  |<-----|  Respuesta con   |<-----|  Generación de    |
|  token JWT        |      |  token JWT       |      |  token JWT        |
|                   |      |                   |      |                   |
+-------------------+      +-------------------+      +-------------------+
```

## Reserva de Pistas

```
+-------------------+      +-------------------+      +-------------------+
|                   |      |                   |      |                   |
|  Cliente solicita |----->| ReservationCtrl   |----->| ReservationService|
|  reserva          |      | /api/reservations |      | CreateAsync       |
|                   |      |                   |      |                   |
+-------------------+      +-------------------+      +-------------------+
                                     |                         |
                                     v                         v
                           +-------------------+      +-------------------+
                           |                   |      |                   |
                           |  Verificación de  |      |  Almacenamiento  |
                           |  disponibilidad   |      |  en MongoDB      |
                           |                   |      |                   |
                           +-------------------+      +-------------------+
```

## Tecnologías Utilizadas

- **Backend**: ASP.NET Core 8.0
- **Base de Datos**: MongoDB
- **Autenticación**: JWT (JSON Web Tokens)
- **Documentación API**: Swagger/OpenAPI

Esta arquitectura sigue el patrón de diseño de Controlador-Servicio-Repositorio, proporcionando una clara separación de responsabilidades y facilitando el mantenimiento y la escalabilidad del sistema.