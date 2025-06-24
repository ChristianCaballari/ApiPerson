# Personas API

API RESTful desarrollada con .NET 7 para gestionar una entidad **Persona**, permitiendo realizar operaciones CRUD y búsquedas avanzadas. La API utiliza **PostgreSQL** como base de datos y sigue principios de arquitectura limpia con separación de responsabilidades.

## 🚀 Tecnologías Utilizadas

- **.NET 8**
- **ASP.NET Core Web API**
- **Entity Framework Core**
- **PostgreSQL**
- **Swagger / OpenAPI**
- **AutoMapper**
- **CORS**
- **Manejo de excepciones personalizado (Middleware)**

---

## 🎯 Objetivo

Desarrollar una API robusta y documentada que permita administrar registros de personas, con validaciones y manejo de errores adecuado.

---

| Capa        | Descripción                                                                 |
|-------------|------------------------------------------------------------------------------|
| **Controllers** | Encargados de exponer los endpoints de la API REST.                      |
| **AppDbContext**        | Incluye el `DbContext` y configuración de la base de datos PostgreSQL.   |
| **DTOs**        | Objetos para la transferencia de datos entre cliente y servidor.          |
| **Entidades**    | Representan las entidades del dominio y el modelo relacional.             |
| **AutoMapperProfiles**    | Configuración de AutoMapper entre entidades y DTOs.                      |
| **Middlewares** | Lógica para capturar y gestionar errores globales de la API.              |

## ▶️ Ejecución del Proyecto

### 1. Clonar el repositorio

```
git clone https://github.com/tu-usuario/tu-repo.git
cd tu-repo

- Después de clonar el proyecto, ejecuta `dotnet restore` para descargar las dependencias necesarias.


##  Configurar tu appSettings
{
origenesPermitidos: "*",
"connectionStrings": {
  "defaultConnection": "Host=localhost;Username=postgres;Password=tu_password;Database=person_db"
  }
}

## 🛠️ Ejecutar Migraciones con Entity Framework Core

Una vez configurada tu cadena de conexión en `appsettings.json`, puedes crear e implementar la base de datos con los siguientes comandos desde la **Package Manager Console**:

```powershell
Add-Migration Inicial
Update-Database

## 🔐 Consideraciones

- El campo `FechaRegistro` se establece automáticamente en el servidor.
- Todos los campos requeridos son validados antes de persistirse.
- Se implementa un middleware para registrar errores inesperados.

## 🔧 Mejoras Futuras

- Implementar autenticación con JWT.
- Agregar paginación y ordenamiento en el endpoint GET.




