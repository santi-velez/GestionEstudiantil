using GestionEstudiantil.Infraestructura.Datos;
using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using GestionEstudiantil.Web.Models;

namespace GestionEstudiantil.Web.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly BaseDeDatosContexto _context;

        public HomeController(ILogger<HomeController> logger, BaseDeDatosContexto context)
        {
            _logger = logger;
            _context = context;
        }

        public IActionResult Index()
        {
            var estudiantes = _context.Estudiantes.ToList();
            return View(estudiantes);
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
