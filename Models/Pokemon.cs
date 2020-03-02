using CsvHelper;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using CsvHelper.Configuration;
using CsvHelper.Configuration.Attributes;
using System.Text;
using KryodeHelpers;
using static System.Net.Mime.MediaTypeNames;

namespace TeamBuilderPkmnASP.Models
{
    public class Pokemon
    {
        private string lowerName;
        [Name("id")]
        public int Id { get; set; }
        [Name("identifier")]
        public string Name { 
            get 
            {
                return StringHelper.FirstCharToUpper(lowerName);
            } 
            set 
            {
                lowerName = value.ToLower();
            }
        }

        public string Image { get; set; }
        
        //Csv construct
        public Pokemon(int id, string identifier)
        {
            this.Id = id;
            this.lowerName = identifier;
            this.Image = DataPaths.Sprite + Id.ToString() + ".png";
        }

        public static Pokemon[] GetPokemons()
        {
            Pokemon[] pokemons;
            using (var reader = new StreamReader(DataPaths.PokemonCsv))
            {
                CsvConfiguration configuration = new CsvConfiguration(CultureInfo.InvariantCulture);
                configuration.HeaderValidated = null;
                using (var csv = new CsvReader(reader, configuration))
                {
                    pokemons = csv.GetRecords<Pokemon>().ToArray();
                }
            }
            return pokemons;
        }
    }
}
