using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Http;

namespace Proyecto_Rescate_Adopcion.ViewModels
{
    public class CrearAnimal
    {
        [Required, StringLength(50)]
        public string? NombreAnimal { get; set; }

        [Range(0, 40)]
        public int? Edad { get; set; }

        [StringLength(30)]
        public string? Especie { get; set; }

        [StringLength(100)]
        public string? Localidad { get; set; }

        [StringLength(500)]
        public string? Descripcion { get; set; }

        public IFormFile? Foto { get; set; }
    }
}
