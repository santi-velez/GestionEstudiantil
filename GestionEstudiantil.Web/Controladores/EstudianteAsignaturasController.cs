using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using GestionEstudiantil.Dominio.Entidades;
using GestionEstudiantil.Infraestructura.Datos;
using GestionEstudiantil.Aplicacion.Servicios;

namespace GestionEstudiantil.Web.Controladores
{
    public class EstudianteAsignaturasController : Controller
    {
        private readonly BaseDeDatosContexto _context;
        private readonly InscripcionService _inscripcionService;

        public EstudianteAsignaturasController(BaseDeDatosContexto context, InscripcionService inscripcionService)
        {
            _context = context;
            _inscripcionService = inscripcionService;
        }

        // GET: EstudianteAsignaturas
        public async Task<IActionResult> Index()
        {
            var relaciones = await _context.EstudiantesAsignaturas
                .Include(ea => ea.Estudiante)
                .Include(ea => ea.Asignatura)
                .ToListAsync();

            Console.WriteLine("Total de relaciones cargadas: " + relaciones.Count);

            return View(relaciones);
        }


        // GET: EstudianteAsignaturas/Details
        public async Task<IActionResult> Details(int? estudianteId, int? asignaturaId)
        {
            if (estudianteId == null || asignaturaId == null)
                return NotFound();

            var estudianteAsignatura = await _context.EstudiantesAsignaturas
                .Include(e => e.Estudiante)
                .Include(e => e.Asignatura)
                .FirstOrDefaultAsync(m => m.EstudianteId == estudianteId && m.AsignaturaId == asignaturaId);

            if (estudianteAsignatura == null)
                return NotFound();

            return View(estudianteAsignatura);
        }

        // GET: EstudianteAsignaturas/Create
        public IActionResult Create()
        {
            ViewData["AsignaturaId"] = new SelectList(_context.Asignaturas, "Id", "Nombre");
            ViewData["EstudianteId"] = new SelectList(_context.Estudiantes, "Id", "Nombre");
            return View();
        }

        // POST: EstudianteAsignaturas/Create
        // POST: EstudianteAsignaturas/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("EstudianteId,AsignaturaId")] EstudianteAsignatura estudianteAsignatura)
        {
            // Verificar si ya existe la relación
            var yaExiste = await _context.EstudiantesAsignaturas
                .AnyAsync(ea => ea.EstudianteId == estudianteAsignatura.EstudianteId && ea.AsignaturaId == estudianteAsignatura.AsignaturaId);

            if (yaExiste)
            {
                ModelState.AddModelError("", "Esta relación ya existe.");
            }

            // Validar la lógica de inscripción (créditos)
            var puedeInscribirse = await _inscripcionService.PuedeInscribirse(estudianteAsignatura.EstudianteId, estudianteAsignatura.AsignaturaId);

            if (!puedeInscribirse)
            {
                ModelState.AddModelError("", "No se pueden inscribir más de 3 materias con más de 4 créditos.");
            }

            // Si todo es válido, guardar la relación
            if (ModelState.IsValid)
            {
                _context.Add(estudianteAsignatura);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }

            // Recargar datos para el formulario en caso de error
            ViewData["AsignaturaId"] = new SelectList(_context.Asignaturas, "Id", "Nombre", estudianteAsignatura.AsignaturaId);
            ViewData["EstudianteId"] = new SelectList(_context.Estudiantes, "Id", "Nombre", estudianteAsignatura.EstudianteId);
            return View(estudianteAsignatura);
        }


        // GET: EstudianteAsignaturas/Edit
        public async Task<IActionResult> Edit(int? estudianteId, int? asignaturaId)
        {
            if (estudianteId == null || asignaturaId == null)
                return NotFound();

            var estudianteAsignatura = await _context.EstudiantesAsignaturas
                .FirstOrDefaultAsync(ea => ea.EstudianteId == estudianteId && ea.AsignaturaId == asignaturaId);

            if (estudianteAsignatura == null)
                return NotFound();

            ViewData["AsignaturaId"] = new SelectList(_context.Asignaturas, "Id", "Nombre", estudianteAsignatura.AsignaturaId);
            ViewData["EstudianteId"] = new SelectList(_context.Estudiantes, "Id", "Nombre", estudianteAsignatura.EstudianteId);
            return View(estudianteAsignatura);
        }

        // POST: EstudianteAsignaturas/Edit
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int estudianteId, int asignaturaId, [Bind("EstudianteId,AsignaturaId")] EstudianteAsignatura estudianteAsignatura)
        {
            if (estudianteId != estudianteAsignatura.EstudianteId || asignaturaId != estudianteAsignatura.AsignaturaId)
                return NotFound();

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(estudianteAsignatura);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!EstudianteAsignaturaExists(estudianteId, asignaturaId))
                        return NotFound();
                    else
                        throw;
                }

                return RedirectToAction(nameof(Index));
            }

            ViewData["AsignaturaId"] = new SelectList(_context.Asignaturas, "Id", "Nombre", estudianteAsignatura.AsignaturaId);
            ViewData["EstudianteId"] = new SelectList(_context.Estudiantes, "Id", "Nombre", estudianteAsignatura.EstudianteId);
            return View(estudianteAsignatura);
        }

        // GET: EstudianteAsignaturas/Delete
        public async Task<IActionResult> Delete(int? estudianteId, int? asignaturaId)
        {
            if (estudianteId == null || asignaturaId == null)
                return NotFound();

            var estudianteAsignatura = await _context.EstudiantesAsignaturas
                .Include(e => e.Estudiante)
                .Include(e => e.Asignatura)
                .FirstOrDefaultAsync(m => m.EstudianteId == estudianteId && m.AsignaturaId == asignaturaId);

            if (estudianteAsignatura == null)
                return NotFound();

            return View(estudianteAsignatura);
        }

        // POST: EstudianteAsignaturas/DeleteConfirmed
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int estudianteId, int asignaturaId)
        {
            var estudianteAsignatura = await _context.EstudiantesAsignaturas
                .FirstOrDefaultAsync(e => e.EstudianteId == estudianteId && e.AsignaturaId == asignaturaId);

            if (estudianteAsignatura != null)
            {
                _context.EstudiantesAsignaturas.Remove(estudianteAsignatura);
                await _context.SaveChangesAsync();
            }

            return RedirectToAction(nameof(Index));
        }

        private bool EstudianteAsignaturaExists(int estudianteId, int asignaturaId)
        {
            return _context.EstudiantesAsignaturas.Any(e =>
                e.EstudianteId == estudianteId && e.AsignaturaId == asignaturaId);
        }
    }
}
