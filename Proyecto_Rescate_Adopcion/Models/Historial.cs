using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Proyecto_Rescate_Adopcion.Models
{
    public class Historial
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        [Required]
        public int UsuarioSolicitanteId { get; set; }

        [Required, StringLength(100)]
        public string NombreUsuario { get; set; } = string.Empty;

        [Required, StringLength(50)]
        public string EstadoSolicitud { get; set; } = string.Empty; // "Aceptada" o "Rechazada"

        [Required, StringLength(50)]
        public string TipoMascota { get; set; } = string.Empty; // "un perro", "un gato"

        [StringLength(100)]
        public string? NombreMascota { get; set; }

        public DateTime FechaResolucion { get; set; } = DateTime.Now;

        public int AdopcionId { get; set; }

        // Relaciones
        [ForeignKey(nameof(UsuarioSolicitanteId))]
        public Usuario? Usuario { get; set; }

        [ForeignKey(nameof(AdopcionId))]
        public Adopcion? Adopcion { get; set; }
    }
}