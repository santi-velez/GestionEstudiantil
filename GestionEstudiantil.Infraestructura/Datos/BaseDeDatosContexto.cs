using GestionEstudiantil.Dominio.Entidades;
using Microsoft.EntityFrameworkCore;

namespace GestionEstudiantil.Infraestructura.Datos
{
   
    public class BaseDeDatosContexto : DbContext
    {
        public BaseDeDatosContexto(DbContextOptions<BaseDeDatosContexto> opciones)
            : base(opciones)
        {
        }

        public DbSet<Estudiante> Estudiantes => Set<Estudiante>();
        public DbSet<Asignatura> Asignaturas => Set<Asignatura>();
        public DbSet<EstudianteAsignatura> EstudiantesAsignaturas => Set<EstudianteAsignatura>();

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {

            modelBuilder.Entity<EstudianteAsignatura>()
                .HasKey(ea => new { ea.EstudianteId, ea.AsignaturaId });

            modelBuilder.Entity<EstudianteAsignatura>()
                .HasOne(ea => ea.Estudiante)
                .WithMany(e => e.AsignaturasInscritas)
                .HasForeignKey(ea => ea.EstudianteId);

           modelBuilder.Entity<EstudianteAsignatura>()
                .HasOne(ea => ea.Asignatura)
                .WithMany(a => a.EstudiantesInscriptos)
                .HasForeignKey(ea => ea.AsignaturaId);
        }
    }
}
