using System;
using System.ComponentModel.DataAnnotations;

namespace Proyecto_Rescate_Adopcion.Models
{
    public class Adopcion
    {
        public int Id { get; set; }

        public int UsuarioId { get; set; }
        public int AnimalId { get; set; }

        public DateTime FechaSolicitud { get; set; } = DateTime.Now;
        public string Estado { get; set; } = "Pendiente";
        public string? Notas { get; set; }

        public Usuario? Usuario { get; set; }    
        public Animal? Animal { get; set; }   
    }
}