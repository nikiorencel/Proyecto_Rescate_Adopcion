using Microsoft.AspNetCore.Mvc;
using Proyecto_Rescate_Adopcion.Context;
using Proyecto_Rescate_Adopcion.Models;

namespace Proyecto_Rescate_Adopcion.Controllers
{
    public class AdopcionController : Controller
    {
        private readonly RescateDBContext _ctx;
        public AdopcionController(RescateDBContext ctx) => _ctx = ctx;

        // POST /Adopcion/Solicitar
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Solicitar(int animalId)
        {
            var usuarioId = HttpContext.Session.GetInt32("UsuarioId");
            if (usuarioId == null) return RedirectToAction("Login", "Cuenta");

            var s = new Adopcion { UsuarioId = usuarioId.Value, AnimalId = animalId};
            _ctx.Adopciones.Add(s);
            _ctx.SaveChanges();

            return RedirectToAction(nameof(Confirmacion), new { id = s.Id });
        }
        public IActionResult Confirmacion(int id)
        {
            return View(); 
        }
    }
}