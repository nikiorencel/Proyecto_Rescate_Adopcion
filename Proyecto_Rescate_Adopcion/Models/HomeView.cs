using System;
using System.Collections.Generic;
using Proyecto_Rescate_Adopcion.Models;

namespace Proyecto_Rescate_Adopcion.ViewModels
{
    public class HomeView
    {
        public IEnumerable<Animal> Animales { get; set; } = Enumerable.Empty<Animal>();
        public int Page { get; set; }
        public int PageSize { get; set; }
        public int Total { get; set; }
        public int TotalPages => (int)Math.Ceiling((double)Total / PageSize);
        public bool HasPrev => Page > 1;
        public bool HasNext => Page < TotalPages;
    }
}