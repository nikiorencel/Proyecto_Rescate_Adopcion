using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Proyecto_Rescate_Adopcion.Context;
using Proyecto_Rescate_Adopcion.Models;
using System.Diagnostics;
using Proyecto_Rescate_Adopcion.Models.ViewModels;

namespace Proyecto_Rescate_Adopcion.Controllers
{
    public class HomeController : Controller
    {
        private readonly RescateDBContext _context;
        private readonly ILogger<HomeController> _logger;
        private readonly IHttpContextAccessor _http;

        public HomeController(
            RescateDBContext context,
            ILogger<HomeController> logger,          
            IHttpContextAccessor http                 
        )
        {
            _context = context ?? throw new ArgumentNullException(nameof(context));
            _logger = logger;
            _http = http;
        }


        public IActionResult Index()
        {
            var usuarioId = HttpContext.Session.GetInt32("UsuarioId");
            var pendientes = new HashSet<int>();

            if (usuarioId != null)
            {
                pendientes = _context.Adopciones
                    .Where(a => a.UsuarioId == usuarioId && (a.Estado ?? "Pendiente") == "Pendiente")
                    .Select(a => a.AnimalId)
                    .ToHashSet();
            }


            ViewBag.Pendientes = pendientes;

            return View();
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
