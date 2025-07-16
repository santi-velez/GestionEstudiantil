using System.Collections.Generic;

namespace GestionEstudiantil.Dominio.Entidades;

public class Estudiante
{
    public int Id { get; set; }
    public string Nombre { get; set; } = string.Empty;
    public string DocumentoIdentidad { get; set; } = string.Empty;
    public string CorreoElectronico { get; set; } = string.Empty;

    public ICollection<EstudianteAsignatura> AsignaturasInscritas { get; set; } = new List<EstudianteAsignatura>();
}
