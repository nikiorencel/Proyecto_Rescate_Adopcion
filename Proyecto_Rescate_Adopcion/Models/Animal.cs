using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Xml.Linq;

namespace Proyecto_Rescate_Adopcion.Models

{
    public class Animal
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int IdSolicitud { get; set; }

        [Required]
        [StringLength(50)]
        public string NombreAnimal { get; set; }

        [Required]
        [StringLength(100)]
        public string UsuarioSolicitante { get; set; }

        [Required]
        [StringLength(30)]
        public string Estado { get; set; }
    }
}
