using Microsoft.EntityFrameworkCore;
using Mundial.Modelo;

namespace Mundial.Persistencia
{
    public class MundialDbContext : DbContext
    {
        public MundialDbContext(DbContextOptions<MundialDbContext> opciones) : base(opciones)
        {

        }

        public DbSet<Pais> Paises { get; set; }
    }
}