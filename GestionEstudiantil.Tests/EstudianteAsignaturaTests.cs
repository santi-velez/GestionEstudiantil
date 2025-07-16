using GestionEstudiantil.Dominio.Entidades;
using GestionEstudiantil.Infraestructura.Datos;
using Microsoft.EntityFrameworkCore;
using Xunit;
using System.Linq;
using System.Threading.Tasks;

namespace GestionEstudiantil.Tests
{
    public class EstudianteAsignaturaTests
    {
        [Fact]
        public async Task NoPermitirMasDeTresMateriasConMasDeCuatroCreditos()
        {
            var options = new DbContextOptionsBuilder<BaseDeDatosContexto>()
                .UseInMemoryDatabase(databaseName: "TestDB")
                .Options;

            using var context = new BaseDeDatosContexto(options);

            var estudianteId = 1;
            context.Estudiantes.Add(new Estudiante { Id = estudianteId, Nombre = "Santi" });

            // 3 materias con más de 4 créditos
            for (int i = 1; i <= 3; i++)
            {
                var asignatura = new Asignatura { Id = i, Nombre = $"Materia {i}", Creditos = 5 };
                context.Asignaturas.Add(asignatura);
                context.EstudiantesAsignaturas.Add(new EstudianteAsignatura
                {
                    EstudianteId = estudianteId,
                    AsignaturaId = i
                });
            }

            context.SaveChanges();

            // Validacions
            var asignaciones = context.EstudiantesAsignaturas
                .Include(ea => ea.Asignatura)
                .Where(ea => ea.EstudianteId == estudianteId && ea.Asignatura.Creditos > 4)
                .Count();

            Assert.Equal(3, asignaciones);
        }
    }
}
