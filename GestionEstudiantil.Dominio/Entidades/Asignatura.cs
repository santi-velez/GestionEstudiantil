using System.Collections.Generic;

namespace GestionEstudiantil.Dominio.Entidades;

public class Asignatura
{
    public int Id { get; set; }
    public string Nombre { get; set; } = string.Empty;
    public string Codigo { get; set; } = string.Empty;
    public int Creditos { get; set; }

    public ICollection<EstudianteAsignatura> EstudiantesInscriptos { get; set; } = new List<EstudianteAsignatura>();
}
