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

            var animal = await _ctx.Animales.FirstOrDefaultAsync(a => a.IdSolicitud == animalId);
            if (animal == null)
                return NotFound();

            if (animal.UsuarioCreadorId == usuarioId.Value)
            {
                TempData["msg"] = "No podés solicitar la adopción de una mascota que vos mismo publicaste.";
                return RedirectToAction("Details", "Animal", new { id = animalId });
            }

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

            if (string.IsNullOrEmpty(animal.Estado) || animal.Estado == "Disponible")
                animal.Estado = "Pendiente";

            await _ctx.SaveChangesAsync();

            TempData["ok"] = "🐾 ¡Tu solicitud fue registrada!";
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

        // NUEVO: Ver solicitudes recibidas (para el dueño del animal)
        public async Task<IActionResult> SolicitudesRecibidas()
        {
            var usuarioId = HttpContext.Session.GetInt32("UsuarioId");
            if (usuarioId == null)
                return RedirectToAction("Login", "Cuenta");

            var solicitudes = await _ctx.Adopciones
                .Include(a => a.Animal)
                .Include(a => a.Usuario)
                .Where(a => a.Animal!.UsuarioCreadorId == usuarioId.Value)
                .OrderByDescending(a => a.FechaSolicitud)
                .ToListAsync();

            return View(solicitudes);
        }

        // NUEVO: Aceptar solicitud
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Aceptar(int id)
        {
            var usuarioId = HttpContext.Session.GetInt32("UsuarioId");
            if (usuarioId == null)
                return RedirectToAction("Login", "Cuenta");

            var adopcion = await _ctx.Adopciones
                .Include(a => a.Animal)
                .Include(a => a.Usuario)
                .FirstOrDefaultAsync(a => a.Id == id);

            if (adopcion == null || adopcion.Animal?.UsuarioCreadorId != usuarioId)
                return NotFound();

            // Actualizar estado de la adopción
            adopcion.Estado = "Aceptada";

            // Actualizar estado del animal
            if (adopcion.Animal != null)
            {
                adopcion.Animal.Estado = "Adoptado";
                adopcion.Animal.UsuarioSolicitanteId = adopcion.UsuarioId;
            }

            // Registrar en el historial
            var historial = new Historial
            {
                UsuarioSolicitanteId = adopcion.UsuarioId,
                NombreUsuario = $"{adopcion.Usuario?.Nombre} {adopcion.Usuario?.Apellido}",
                EstadoSolicitud = "Aceptada",
                TipoMascota = adopcion.Animal?.Especie?.ToLower() == "perro" ? "un perro" : "un gato",
                NombreMascota = adopcion.Animal?.NombreAnimal,
                FechaResolucion = DateTime.Now,
                AdopcionId = adopcion.Id
            };

            _ctx.Historiales.Add(historial);

            // Rechazar todas las otras solicitudes pendientes para este animal
            var otrasSolicitudes = await _ctx.Adopciones
                .Where(a => a.AnimalId == adopcion.AnimalId && a.Id != id && a.Estado == "Pendiente")
                .ToListAsync();

            foreach (var otra in otrasSolicitudes)
            {
                otra.Estado = "Rechazada";
            }

            await _ctx.SaveChangesAsync();

            TempData["ok"] = "✅ Solicitud aceptada correctamente.";
            return RedirectToAction(nameof(SolicitudesRecibidas));
        }

        // NUEVO: Rechazar solicitud
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Rechazar(int id)
        {
            var usuarioId = HttpContext.Session.GetInt32("UsuarioId");
            if (usuarioId == null)
                return RedirectToAction("Login", "Cuenta");

            var adopcion = await _ctx.Adopciones
                .Include(a => a.Animal)
                .Include(a => a.Usuario)
                .FirstOrDefaultAsync(a => a.Id == id);

            if (adopcion == null || adopcion.Animal?.UsuarioCreadorId != usuarioId)
                return NotFound();

            // Actualizar estado de la adopción
            adopcion.Estado = "Rechazada";

            // Registrar en el historial
            var historial = new Historial
            {
                UsuarioSolicitanteId = adopcion.UsuarioId,
                NombreUsuario = $"{adopcion.Usuario?.Nombre} {adopcion.Usuario?.Apellido}",
                EstadoSolicitud = "Rechazada",
                TipoMascota = adopcion.Animal?.Especie?.ToLower() == "perro" ? "un perro" : "un gato",
                NombreMascota = adopcion.Animal?.NombreAnimal,
                FechaResolucion = DateTime.Now,
                AdopcionId = adopcion.Id
            };

            _ctx.Historiales.Add(historial);

            // Si no hay más solicitudes pendientes, volver el animal a "Disponible"
            var hayOtrasPendientes = await _ctx.Adopciones
                .AnyAsync(a => a.AnimalId == adopcion.AnimalId && a.Id != id && a.Estado == "Pendiente");

            if (!hayOtrasPendientes && adopcion.Animal != null)
            {
                adopcion.Animal.Estado = "Disponible";
            }

            await _ctx.SaveChangesAsync();

            TempData["ok"] = "❌ Solicitud rechazada.";
            return RedirectToAction(nameof(SolicitudesRecibidas));
        }

        // NUEVO: Ver el historial completo
        public async Task<IActionResult> Historial()
        {
            var historiales = await _ctx.Historiales
                .Include(h => h.Usuario)
                .Include(h => h.Adopcion)
                    .ThenInclude(a => a!.Animal)
                .OrderByDescending(h => h.FechaResolucion)
                .ToListAsync();

            return View(historiales);
        }
    }
}