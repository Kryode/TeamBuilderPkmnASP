using KryodeHelpers;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TeamBuilderPkmnASP.Models
{
    public class Type
    {
        [Key] [Column("id")]
        public int Id { get; set; }

        [Column("identifier")]
        public string Name { 
            get 
            {
                return StringHelper.FirstCharToUpper(lowerName.Trim());
            } 
            set
            {
                lowerName = value.ToLower(); 
            } 
        }

        [Column("generation_id")]
        public int GenerationId { get; set; }

        [NotMapped]
        public string Image { get { return DataPaths.Types + lowerName.Trim() + ".png"; } }

        private string lowerName;
    }
}
