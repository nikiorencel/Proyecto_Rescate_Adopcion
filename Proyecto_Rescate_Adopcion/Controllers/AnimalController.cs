using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Proyecto_Rescate_Adopcion.Context;
using Proyecto_Rescate_Adopcion.Models;
using Proyecto_Rescate_Adopcion.ViewModels;
using Microsoft.AspNetCore.Hosting;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

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

        // GET: /Animal
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

        // GET: /Animal/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null) return NotFound();

            var animal = await _ctx.Animales
                .FirstOrDefaultAsync(m => m.IdSolicitud == id);
            if (animal == null) return NotFound();

            return View(animal);
        }

        // GET: Animal/Create
        [HttpGet]
        public IActionResult Create()
        {
            // si usás login por sesión:
            var uid = HttpContext.Session.GetInt32("UsuarioId");
            if (uid == null) return RedirectToAction("Login", "Cuenta");

            return View(new CrearAnimal());
        }

        // POST: Animal/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(CrearAnimal model)
        {
            if (!ModelState.IsValid)
                return View(model);

            // 1) Subida de foto (opcional)
            string? fotoUrl = null;
            if (model.Foto != null && model.Foto.Length > 0)
            {
                var uploads = Path.Combine(_env.WebRootPath, "uploads", "animals");
                Directory.CreateDirectory(uploads);

                var fileName = $"{Guid.NewGuid()}{Path.GetExtension(model.Foto.FileName)}";
                var fullPath = Path.Combine(uploads, fileName);

                using (var fs = new FileStream(fullPath, FileMode.Create))
                {
                    await model.Foto.CopyToAsync(fs);
                }

                fotoUrl = $"/uploads/animals/{fileName}";
            }

            // 2) Crear entidad con defaults en servidor
            var entity = new Animal
            {
                NombreAnimal = model.NombreAnimal,
                Especie = model.Especie,
                Edad = model.Edad,
                Localidad = model.Localidad,
                Descripcion = model.Descripcion,
                FotoUrl = fotoUrl,

                // defaults importantes:
                Estado = "Disponible",
                FechaPublicacion = DateTime.Now,
                UsuarioCreadorId = HttpContext.Session.GetInt32("UsuarioId")
            };

            _ctx.Animales.Add(entity);
            await _ctx.SaveChangesAsync();

            TempData["ok"] = "Mascota publicada correctamente.";
            return RedirectToAction("Index", "Home");
        }

        public async Task<IActionResult> Index()
        {
            var data = await _ctx.Animales
                .OrderByDescending(a => a.FechaPublicacion)
                .ToListAsync();
            return View(data);
        }
    

        // GET: /Animal/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null) return NotFound();

            var animal = await _ctx.Animales.FindAsync(id);
            if (animal == null) return NotFound();

            return View(animal);
        }

        // POST: /Animal/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id,
            [Bind("IdSolicitud,NombreAnimal,Especie,Raza,Localidad,Estado,UsuarioSolicitante")]
            Animal animal)
        {
            if (id != animal.IdSolicitud) return NotFound();
            if (!ModelState.IsValid) return View(animal);

            try
            {
                _ctx.Update(animal);
                await _ctx.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!AnimalExists(animal.IdSolicitud)) return NotFound();
                throw;
            }

            return RedirectToAction(nameof(Index));
        }

        // GET: /Animal/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null) return NotFound();

            var animal = await _ctx.Animales
                .FirstOrDefaultAsync(m => m.IdSolicitud == id);
            if (animal == null) return NotFound();

            return View(animal);
        }

        // POST: /Animal/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var animal = await _ctx.Animales.FindAsync(id);
            if (animal != null)
            {
                _ctx.Animales.Remove(animal);
                await _ctx.SaveChangesAsync();
            }
            return RedirectToAction(nameof(Index));
        }

        private bool AnimalExists(int id) =>
            _ctx.Animales.Any(e => e.IdSolicitud == id);
    }
}