using System.Collections.Generic;

namespace Proyecto_Rescate_Adopcion.Models.ViewModels
{
    public class HomeView
    {
        public List<Animal> Animales { get; set; } = new();
        public HashSet<int> PendientesDelUsuario { get; set; } = new();
    }
}