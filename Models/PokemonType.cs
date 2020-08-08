using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TeamBuilderPkmnASP.Models
{
    public class PokemonType
    {
        [Key] [Column("id")]
        public int Id { get; set; }

        [ForeignKey("pokemon_id")]
        [Column("pokemon_id")]
        public int PokemonId { get; set; }
        public virtual Pokemon Pokemon { get; set; }

        [ForeignKey("type_id")]
        [Column("type_id")]
        public int TypeId { get; set; }
        public virtual Type Type { get; set; }

        [Column("slot")]
        public int Slot { get; set; }
    }
}
