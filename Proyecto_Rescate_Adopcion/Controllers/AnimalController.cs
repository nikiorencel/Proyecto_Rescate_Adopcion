using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Proyecto_Rescate_Adopcion.Context;
using Proyecto_Rescate_Adopcion.Models;
using Proyecto_Rescate_Adopcion.ViewModels;
using Microsoft.AspNetCore.Hosting;

namespace Proyecto_Rescate_Adopcion.Controllers
{
    public class AnimalController : Controller
    {
        private readonly RescateDBContext _ctx;
        private readonly IWebHostEnvironment _env;

        public AnimalController(RescateDBContext ctx, IWebHostEnvironment env)
        {
            _ctx = ctx;
            _env = env;
        }

        public async Task<IActionResult> Index(string? localidad, string? estado)
        {
            var q = _ctx.Animales.AsQueryable();

            if (!string.IsNullOrWhiteSpace(localidad))
                q = q.Where(a => a.Localidad!.Contains(localidad));

            if (!string.IsNullOrWhiteSpace(estado))
                q = q.Where(a => a.Estado == estado);

            q = q.OrderBy(a => a.NombreAnimal);

            return View(await q.ToListAsync());
        }

        public async Task<IActionResult> MisPublicaciones()
        {
            var uid = HttpContext.Session.GetInt32("UsuarioId");
            if (uid == null)
            {
                return RedirectToAction("Login", "Cuenta");
            }

            var publicaciones = await _ctx.Animales
                .Where(a => a.UsuarioCreadorId == uid.Value)
                .OrderByDescending(a => a.FechaPublicacion)
                .ToListAsync();

            return View(publicaciones);
        }

        [HttpGet]
        public async Task<IActionResult> Details(int id, string? returnUrl = null)
        {
            var animal = await _ctx.Animales.AsNoTracking()
                               .FirstOrDefaultAsync(a => a.IdSolicitud == id);
            if (animal == null) return NotFound();

            var usuarioId = HttpContext.Session.GetInt32("UsuarioId");
            var yaPend = false;
            if (usuarioId != null)
            {
                yaPend = await _ctx.Adopciones
                   .AnyAsync(x => x.UsuarioId == usuarioId.Value
                               && x.AnimalId == id
                               && (x.Estado ?? "Pendiente") == "Pendiente");
            }

            ViewBag.ReturnUrl = returnUrl;

            return View(new AnimalDetalle { Animal = animal, YaPendiente = yaPend });
        }

        [HttpGet]
        public IActionResult Create()
        {
            var uid = HttpContext.Session.GetInt32("UsuarioId");
            if (uid == null) return RedirectToAction("Login", "Cuenta");

            return View(new CrearAnimal());
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(CrearAnimal model)
        {
            if (!ModelState.IsValid)
                return View(model);

            string? fotoUrl = null;
            if (model.Foto != null && model.Foto.Length > 0)
            {
                var uploads = Path.Combine(_env.WebRootPath, "uploads", "animals");
                Directory.CreateDirectory(uploads);

                var fileName = $"{Guid.NewGuid()}{Path.GetExtension(model.Foto.FileName)}";
                var fullPath = Path.Combine(uploads, fileName);

                using (var fs = new FileStream(fullPath, FileMode.Create))
                    await model.Foto.CopyToAsync(fs);

                fotoUrl = $"/uploads/animals/{fileName}";
            }

            var entity = new Animal
            {
                NombreAnimal = model.NombreAnimal,
                Especie = model.Especie,
                Sexo = model.Sexo,
                Edad = model.Edad,
                Localidad = model.Localidad,
                Descripcion = model.Descripcion,
                FotoUrl = fotoUrl,
                Estado = "Disponible",
                FechaPublicacion = DateTime.Now,
                UsuarioCreadorId = HttpContext.Session.GetInt32("UsuarioId")
            };

            _ctx.Animales.Add(entity);
            await _ctx.SaveChangesAsync();

            TempData["ok"] = "Mascota publicada correctamente.";
            return RedirectToAction("Index", "Home");
        }

        [HttpGet]
        public async Task<IActionResult> Edit(int id)
        {
            var uid = HttpContext.Session.GetInt32("UsuarioId");
            if (uid == null)
            {
                return RedirectToAction("Login", "Cuenta");
            }

            var animal = await _ctx.Animales.FindAsync(id);
            if (animal == null)
            {
                return NotFound();
            }

            if (animal.UsuarioCreadorId != uid.Value)
            {
                return Forbid();
            }

            return View(animal);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("IdSolicitud,NombreAnimal,Edad,Especie,Sexo,Localidad,Descripcion,Estado")] Animal model)
        {
            var uid = HttpContext.Session.GetInt32("UsuarioId");
            if (uid == null)
            {
                return RedirectToAction("Login", "Cuenta");
            }

            if (id != model.IdSolicitud)
            {
                return NotFound();
            }

            if (!ModelState.IsValid)
            {
                return View(model);
            }

            var animal = await _ctx.Animales.FirstOrDefaultAsync(a => a.IdSolicitud == id);
            if (animal == null)
            {
                return NotFound();
            }

            if (animal.UsuarioCreadorId != uid.Value)
            {
                return Forbid();
            }

            animal.NombreAnimal = model.NombreAnimal;
            animal.Edad = model.Edad;
            animal.Especie = model.Especie;
            animal.Sexo = model.Sexo;
            animal.Localidad = model.Localidad;
            animal.Descripcion = model.Descripcion;
            animal.Estado = model.Estado;

            await _ctx.SaveChangesAsync();

            TempData["ok"] = "Publicación actualizada correctamente.";
            return RedirectToAction("MisPublicaciones");
        }

        private bool AnimalExists(int id) =>
            _ctx.Animales.Any(e => e.IdSolicitud == id);
    }
}