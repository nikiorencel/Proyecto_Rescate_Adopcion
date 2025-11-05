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

            var u = _context.Usuarios
                .FirstOrDefault(x => x.Email == model.Email && x.Contrasenia == model.Contrasenia);

            if (u == null)
            {
                ViewBag.Error = "Correo o contraseña incorrectos.";
                return View(model);
            }

            HttpContext.Session.SetInt32("UsuarioId", u.IdUsuario);
            HttpContext.Session.SetString("UsuarioNombre", u.Nombre);

            return RedirectToAction("Index", "Home");
        }

        public IActionResult Logout()
        {
            HttpContext.Session.Clear();
            return RedirectToAction("Login");
        }
    }
}

