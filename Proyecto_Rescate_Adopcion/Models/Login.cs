using System.ComponentModel.DataAnnotations;

namespace Proyecto_Rescate_Adopcion.Models
{
    public class Login
    {
        [Required, EmailAddress]
        public string Email { get; set; }

        [Required, DataType(DataType.Password)]
        public string Contrasenia { get; set; }
    }
}

