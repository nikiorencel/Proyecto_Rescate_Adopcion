using System;
using System.ComponentModel.DataAnnotations;

namespace Proyecto_Rescate_Adopcion.Models
{
    public class Adopcion
    {
        [Key] public int Id { get; set; }

        [Required] public int UsuarioId { get; set; }   
        [Required] public int AnimalId { get; set; }    

        public DateTime FechaSolicitud { get; set; } = DateTime.UtcNow;
    }
}