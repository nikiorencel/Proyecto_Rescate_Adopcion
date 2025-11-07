using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Proyecto_Rescate_Adopcion.Context;
using Proyecto_Rescate_Adopcion.Models;
using Proyecto_Rescate_Adopcion.ViewModels; 
using System.Diagnostics;

namespace Proyecto_Rescate_Adopcion.Controllers
{
    public class HomeController : Controller
    {
        private readonly RescateDBContext _context;
        private readonly ILogger<HomeController> _logger;

        public HomeController(RescateDBContext context, ILogger<HomeController> logger)
        {
            _context = context ?? throw new ArgumentNullException(nameof(context));
            _logger = logger;
        }

        public async Task<IActionResult> Index()
        {
            var animales = await _context.Animales
                .AsNoTracking()

                .Where(a => a.Estado != "Adoptado" && a.Estado != "Inactivo" && a.Estado != "Eliminado")

                .OrderByDescending(a => a.FechaPublicacion)
                .ToListAsync();

            var vm = new HomeView { Animales = animales };

            var usuarioId = HttpContext.Session.GetInt32("UsuarioId");

            if (usuarioId != null)
            {

                ViewBag.Pendientes = _context.Adopciones
                    .Where(x => x.UsuarioId == usuarioId && (x.Estado ?? "Pendiente") == "Pendiente")
                    .Select(x => x.AnimalId)
                    .ToHashSet();
            }
            else
            {
                ViewBag.Pendientes = new HashSet<int>();
            }

            return View(vm);
        }

        public IActionResult Privacy() => View();

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}