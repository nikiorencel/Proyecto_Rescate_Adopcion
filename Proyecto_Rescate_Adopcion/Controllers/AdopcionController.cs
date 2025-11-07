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
        public async Task<IActionResult> Solicitar(int animalId)
        {
            var usuarioId = HttpContext.Session.GetInt32("UsuarioId");
            if (usuarioId == null)
                return RedirectToAction("Login", "Cuenta");


            bool yaExistePendiente = await _ctx.Adopciones.AnyAsync(a =>
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
                Estado = "Pendiente",
                FechaSolicitud = DateTime.Now 
            };

            _ctx.Adopciones.Add(s);


            var animal = await _ctx.Animales.FirstOrDefaultAsync(a => a.IdSolicitud == animalId);
            if (animal != null)
            {

                if (string.IsNullOrEmpty(animal.Estado) || animal.Estado == "Disponible")
                    animal.Estado = "Pendiente";
            }

            await _ctx.SaveChangesAsync();

            TempData["ok"] = "¡Tu solicitud fue registrada!";
            return RedirectToAction(nameof(MisSolicitudes));
        }

        public IActionResult Confirmacion(int id) => View();

        public async Task<IActionResult> MisSolicitudes()
        {
            var usuarioId = HttpContext.Session.GetInt32("UsuarioId");
            if (usuarioId == null)
                return RedirectToAction("Login", "Cuenta");

            var items = await _ctx.Adopciones
                .Include(a => a.Animal)
                .Where(a => a.UsuarioId == usuarioId.Value)
                .OrderByDescending(a => a.FechaSolicitud)
                .ToListAsync();

            return View(items);
        }
    }
}