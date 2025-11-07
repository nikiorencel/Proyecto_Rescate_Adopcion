using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Xml.Linq;

namespace Proyecto_Rescate_Adopcion.Models
{
    public class Usuario
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int IdUsuario { get; set; }

        [Required, StringLength(50)]
        public string Nombre { get; set; } = string.Empty;

        [Required, StringLength(50)]
        public string Apellido { get; set; } = string.Empty;

        [Required, EmailAddress, StringLength(100)]
        public string Email { get; set; } = string.Empty;

        [Required, DataType(DataType.Password), StringLength(200)]
        public string Contrasenia { get; set; } = string.Empty;

        [StringLength(80)]
        public string? Localidad { get; set; }

        [Display(Name = "Historial de Adopciones")]
        public string? HistorialAdopciones { get; set; }
    }
}
