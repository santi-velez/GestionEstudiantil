# Sistema de Gestión Estudiantil

Aplicación web desarrollada en ASP.NET Core MVC para gestionar estudiantes, asignaturas e inscripciones, aplicando reglas de negocio y buenas prácticas de desarrollo.

## Descripción

Este proyecto permite registrar, editar, eliminar y listar estudiantes y asignaturas, así como asociar materias a los estudiantes. Incluye una regla de validación para que un estudiante no pueda inscribir más de **3 asignaturas con más de 4 créditos**.

## Arquitectura

El proyecto está organizado en capas siguiendo principios SOLID:

- **Dominio**: Entidades del negocio (Estudiante, Asignatura, EstudianteAsignatura).
- **Infraestructura**: Contexto de base de datos (Entity Framework Core).
- **Aplicación**: Servicios para lógica de negocio.
- **Web**: Proyecto ASP.NET Core MVC con Razor Pages.
- **Tests**: Proyecto con pruebas unitarias usando xUnit y EF Core InMemory.

## Requisitos

- .NET SDK 9.0 o superior
- Visual Studio 2022 (opcional)
- SQLite (opcional, viene preconfigurado)

## Cómo ejecutar el proyecto

1. Clona el repositorio o descarga el código:

git clone https://github.com/<USUARIO>/<REPO>.git
cd <REPO>

2. Restaura los paquetes y ejecuta la aplicación:

dotnet restore
dotnet run --project GestionEstudiantil.Web


Desarrollado por Santiago Vélez Zapata