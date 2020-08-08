using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TeamBuilderPkmnASP.Models
{
    public class Team
    {
        public int UserId { get; set; }
        public int NumOfPokemon { get; set; }
        public  List<Pokemon> Pokemons{ get; set; }
    }
}
