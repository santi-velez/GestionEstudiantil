using System.Threading.Tasks;
using GestionEstudiantil.Dominio.Entidades;
using GestionEstudiantil.Infraestructura.Datos;
using GestionEstudiantil.Aplicacion.Servicios;
using Microsoft.EntityFrameworkCore;
using Xunit;

namespace GestionEstudiantil.Tests
{
    public class InscripcionServiceTests
    {
        private BaseDeDatosContexto GetContext(string dbName)
        {
            var options = new DbContextOptionsBuilder<BaseDeDatosContexto>()
                .UseInMemoryDatabase(databaseName: dbName)
                .Options;

            return new BaseDeDatosContexto(options);
        }

        [Fact]
        public async Task PermitirInscripcion_MenosDeTresMateriasPesadas()
        {
            // Arrange
            var context = GetContext("PermitirInscripcion");
            var service = new InscripcionService(context);

            var estudiante = new Estudiante { Id = 1, Nombre = "Santi" };
            context.Estudiantes.Add(estudiante);

            // 2 materias pesadas
            for (int i = 1; i <= 2; i++)
            {
                var materia = new Asignatura { Id = i, Nombre = $"Mat {i}", Creditos = 5 };
                context.Asignaturas.Add(materia);
                context.EstudiantesAsignaturas.Add(new EstudianteAsignatura { EstudianteId = estudiante.Id, AsignaturaId = i });
            }

            // Nueva materia
            var nuevaMateria = new Asignatura { Id = 99, Nombre = "Mat Pesada", Creditos = 5 };
            context.Asignaturas.Add(nuevaMateria);

            await context.SaveChangesAsync();

            // Act
            var puede = await service.PuedeInscribirse(estudiante.Id, nuevaMateria.Id);

            // Assert
            Assert.True(puede);
        }

        [Fact]
        public async Task RechazarInscripcion_MasDeTresMateriasPesadas()
        {
            // Arrange
            var context = GetContext("RechazarInscripcion");
            var service = new InscripcionService(context);

            var estudiante = new Estudiante { Id = 1, Nombre = "Santi" };
            context.Estudiantes.Add(estudiante);

            // 3 materias pesadas
            for (int i = 1; i <= 3; i++)
            {
                var materia = new Asignatura { Id = i, Nombre = $"Mat {i}", Creditos = 5 };
                context.Asignaturas.Add(materia);
                context.EstudiantesAsignaturas.Add(new EstudianteAsignatura { EstudianteId = estudiante.Id, AsignaturaId = i });
            }

            // Nueva materia
            var nuevaMateria = new Asignatura { Id = 99, Nombre = "Mat Pesada", Creditos = 5 };
            context.Asignaturas.Add(nuevaMateria);

            await context.SaveChangesAsync();

            // Act
            var puede = await service.PuedeInscribirse(estudiante.Id, nuevaMateria.Id);

            // Assert
            Assert.False(puede);
        }

        [Fact]
        public async Task PermitirInscripcion_MateriaLigera()
        {
            var context = GetContext("MateriaLigera");
            var service = new InscripcionService(context);

            var estudiante = new Estudiante { Id = 1, Nombre = "Santi" };
            var materia = new Asignatura { Id = 10, Nombre = "Ligera", Creditos = 3 };

            context.Estudiantes.Add(estudiante);
            context.Asignaturas.Add(materia);

            await context.SaveChangesAsync();

            var puede = await service.PuedeInscribirse(estudiante.Id, materia.Id);

            Assert.True(puede);
        }
    }
}
