using Microsoft.EntityFrameworkCore;
using TeamBuilderPkmnASP.Models;
namespace TeamBuilderPkmnASP.Data
{
    public partial class PokemonContext : DbContext
    {
        public PokemonContext(DbContextOptions<PokemonContext> options)
            : base(options)
        {
        }

        public virtual DbSet<PokemonType> PokemonType { get; set; }
        public virtual DbSet<Type> Type { get; set; }
        public virtual DbSet<Pokemon> Pokemon { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                optionsBuilder.UseSqlServer(DatabaseConnection.Connection);
            }
        }
        

    }
}
