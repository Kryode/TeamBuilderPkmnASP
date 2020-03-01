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
        [Name("species_id")]
        public int Species_id { get; set; }
        [Name("height")]
        public int Height { get; set; }
        [Name("weight")]
        public int Weight { get; set; }
        [Name("base_experience")]
        public int Base_experience { get; set; }
        [Name("order")]
        public int Order { get; set; }
        [Name("is_default")]
        public int Is_default { get; set; }

        //Csv construct
        /*public Pokemon(int id, string identifier, int species_id, int height, int weight, int base_experience, int order, int is_default)
        {
            this.Id = id;
            this.Name = identifier;
        }*/

        public static Pokemon[] GetPokemons()
        {
            Pokemon[] pokemons;
            using (var reader = new StreamReader("./Data/pokemon.csv"))
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
