using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Xml.Linq;

namespace Proyecto_Rescate_Adopcion.Models

{
    public class Animal
    {
        [Key]
        public int IdSolicitud { get; set; }     // PK
        public string? NombreAnimal { get; set; }
        public string? Localidad { get; set; }
        public string? Estado { get; set; }
        public string? UsuarioSolicitante { get; set; }
        public ICollection<Adopcion>? Adopciones { get; set; }
    }
}
