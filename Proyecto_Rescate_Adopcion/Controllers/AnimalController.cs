using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Proyecto_Rescate_Adopcion.Context;
using Proyecto_Rescate_Adopcion.Models;
using System.Linq;
using System.Threading.Tasks;

namespace Proyecto_Rescate_Adopcion.Controllers
{
    public class AnimalController : Controller
    {
        private readonly RescateDBContext _ctx;
        public AnimalController(RescateDBContext ctx) => _ctx = ctx;

        // GET: /Animal
        // Soporta filtros opcionales: ?localidad=...&estado=Disponible
        public async Task<IActionResult> Index(string? localidad, string? estado)
        {
            var q = _ctx.Animales.AsQueryable();

            if (!string.IsNullOrWhiteSpace(localidad))
                q = q.Where(a => a.Localidad!.Contains(localidad));

            if (!string.IsNullOrWhiteSpace(estado))
                q = q.Where(a => a.Estado == estado);

            // Sugerido: listar disponibles primero
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

        // GET: /Animal/Create
        public IActionResult Create() => View();

        // POST: /Animal/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(
            [Bind("IdSolicitud,NombreAnimal,Especie,Raza,Localidad,Estado,UsuarioSolicitante")]
            Animal animal)
        {
            if (!ModelState.IsValid) return View(animal);

            // Valor por defecto si tu modelo lo necesita
            animal.Estado ??= "Disponible";

            _ctx.Add(animal);
            await _ctx.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
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