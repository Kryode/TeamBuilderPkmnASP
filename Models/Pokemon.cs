using CsvHelper;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using CsvHelper.Configuration;
using CsvHelper.Configuration.Attributes;

namespace TeamBuilderPkmnASP.Models
{
    public class Pokemon
    {
        [Name("id")]
        public int Id { get; set; }
        [Name("identifier")]
        public string Name { get; set; }
        
        //Csv construct
        public Pokemon(int id, string identifier, int species_id, int height, int weight, int base_experience, int order, int is_default)
        {
            this.Id = id;
            this.Name = identifier;
        }

        public static Pokemon[] GetPokemons()
        {
            Pokemon[] pokemons;
            using (var reader = new StreamReader("wwwroot/Data/pokemon.csv"))
            {
                CsvConfiguration configuration = new CsvConfiguration(CultureInfo.InvariantCulture);
                using (var csv = new CsvReader(reader, configuration))
                {
                    pokemons = csv.GetRecords<Pokemon>().ToArray();
                }
            }
            return pokemons;
        }
    }
}
