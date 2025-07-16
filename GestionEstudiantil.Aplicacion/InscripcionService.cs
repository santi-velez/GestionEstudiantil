// Ruta sugerida: GestionEstudiantil.Aplicacion/Servicios/InscripcionService.cs

using GestionEstudiantil.Dominio.Entidades;
using GestionEstudiantil.Infraestructura.Datos;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;

namespace GestionEstudiantil.Aplicacion.Servicios;

public class InscripcionService
{
    private readonly BaseDeDatosContexto _context;

    public InscripcionService(BaseDeDatosContexto context)
    {
        _context = context;
    }

    public async Task<bool> PuedeInscribirse(int estudianteId, int asignaturaId)
    {
        var asignatura = await _context.Asignaturas.FirstOrDefaultAsync(a => a.Id == asignaturaId);
        if (asignatura == null) return false;

        if (asignatura.Creditos <= 4) return true;

        var materiasPesadas = await _context.EstudiantesAsignaturas
            .Include(ea => ea.Asignatura)
            .Where(ea => ea.EstudianteId == estudianteId && ea.Asignatura.Creditos > 4)
            .CountAsync();

        return materiasPesadas < 3;
    }
}
