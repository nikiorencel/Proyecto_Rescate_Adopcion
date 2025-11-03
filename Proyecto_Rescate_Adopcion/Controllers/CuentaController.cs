using Microsoft.AspNetCore.Mvc;
using Proyecto_Rescate_Adopcion.Models;
using Proyecto_Rescate_Adopcion.Context;
using System.Linq;

namespace Proyecto_Rescate_Adopcion.Controllers
{
    public class CuentaController : Controller
    {
        private readonly RescateDBContext _context;
        public CuentaController(RescateDBContext context) => _context = context;

        [HttpGet]
        public IActionResult Login() => View(new Login());

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Login(Login model)
        {
            if (!ModelState.IsValid) return View(model);

            // Login básico (sin identidad todavía):
            var ok = _context.Usuarios
                .Any(u => u.Email == model.Email && u.Contrasenia == model.Contrasenia);

            if (!ok)
            {
                ViewBag.Error = "Correo o contraseña incorrectos.";
                return View(model);
            }

            // TODO: setear sesión/cookie si lo necesitas
            return RedirectToAction("Index", "Home");
        }
    }
}

