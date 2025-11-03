using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Proyecto_Rescate_Adopcion.Models;
using System.Collections.Generic;

namespace Proyecto_Rescate_Adopcion.Context
{
    public class RescateDBContext : DbContext
    {
        public RescateDBContext(DbContextOptions<RescateDBContext> options) : base(options)
        {
        }

        public DbSet<Usuario> Usuarios { get; set; }
    }
}
