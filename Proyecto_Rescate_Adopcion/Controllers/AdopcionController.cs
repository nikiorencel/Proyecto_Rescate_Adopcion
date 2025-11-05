using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;               
using Proyecto_Rescate_Adopcion.Context;
using Proyecto_Rescate_Adopcion.Models;

namespace Proyecto_Rescate_Adopcion.Controllers
{
    public class AdopcionController : Controller
    {
        private readonly RescateDBContext _ctx;
        public AdopcionController(RescateDBContext ctx) => _ctx = ctx;

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Solicitar(int animalId)
        {
            var usuarioId = HttpContext.Session.GetInt32("UsuarioId");
            if (usuarioId == null) return RedirectToAction("Login", "Cuenta");

            bool yaExistePendiente = _ctx.Adopciones.Any(a =>
                a.UsuarioId == usuarioId.Value &&
                a.AnimalId == animalId &&
               (a.Estado ?? "Pendiente") == "Pendiente");

            if (yaExistePendiente)
            {
                TempData["msg"] = "Ya tenés una solicitud pendiente para esta mascota.";
                return RedirectToAction("MisSolicitudes");
            }

            var s = new Adopcion
            {
                UsuarioId = usuarioId.Value,
                AnimalId = animalId,
                Estado = "Pendiente"
            };
            _ctx.Adopciones.Add(s);
            _ctx.SaveChanges();

            TempData["msg"] = "Solicitud registrada.";
            return RedirectToAction(nameof(Confirmacion), new { id = s.Id });
        }

        public IActionResult Confirmacion(int id) => View();

        public IActionResult MisSolicitudes()
        {
            var usuarioId = HttpContext.Session.GetInt32("UsuarioId");
            if (usuarioId == null) return RedirectToAction("Login", "Cuenta");

            var items = _ctx.Adopciones
                .Include(a => a.Animal)                 
                .Where(a => a.UsuarioId == usuarioId.Value)
                .OrderByDescending(a => a.FechaSolicitud)
                .ToList();

            return View(items);
        }
    }
}