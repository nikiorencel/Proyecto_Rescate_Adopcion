using System.ComponentModel.DataAnnotations;

namespace Proyecto_Rescate_Adopcion.Models
{
    public class Model
    {
        [Required(ErrorMessage = "El correo electrónico es obligatorio")]
        [EmailAddress(ErrorMessage = "Ingrese un correo válido")]
        public string Email { get; set; }

        [Required(ErrorMessage = "La contraseña es obligatoria")]
        [DataType(DataType.Password)]
        public string Contrasenia { get; set; }
    }
}
