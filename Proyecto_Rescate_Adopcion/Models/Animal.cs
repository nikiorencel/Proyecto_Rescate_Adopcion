using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Proyecto_Rescate_Adopcion.Models
{
    public class Animal
    {
        [Key]
        [Column("IdSolicitud")]
        public int IdSolicitud { get; set; }

        [Required, StringLength(50)]
        [Column("NombreAnimal")]
        public string? NombreAnimal { get; set; }

        [Range(0, 40)]
        [Column("Edad")]
        public int? Edad { get; set; }

        [StringLength(30)]
        [Column("Especie")]
        public string? Especie { get; set; }

        [NotMapped]
        public IFormFile? NuevaFoto { get; set; }

        [StringLength(10)]
        [Column("Sexo")]  // ← AGREGADO
        public string? Sexo { get; set; }

        [StringLength(100)]
        [Column("Localidad")]
        public string? Localidad { get; set; }

        [StringLength(500)]
        [Column("Descripcion")]
        public string? Descripcion { get; set; }

        [StringLength(30)]
        [Column("Estado")]
        public string? Estado { get; set; } = "Disponible";

        [Column("FotoUrl")]
        public string? FotoUrl { get; set; }

        [Column("FechaPublicacion")]
        public DateTime FechaPublicacion { get; set; } = DateTime.Now;

        [Column("UsuarioCreadorId")]
        public int? UsuarioCreadorId { get; set; }

        [Column("UsuarioSolicitanteId")]
        public int? UsuarioSolicitanteId { get; set; }

        [ForeignKey(nameof(UsuarioCreadorId))]
        public Usuario? UsuarioCreador { get; set; }

        [ForeignKey(nameof(UsuarioSolicitanteId))]
        public Usuario? UsuarioSolicitante { get; set; }
    }
}