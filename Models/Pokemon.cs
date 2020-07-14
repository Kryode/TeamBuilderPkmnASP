using KryodeHelpers;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TeamBuilderPkmnASP.Models
{
    public partial class Pokemon
    {
        [Key]
        [Column("id")]
        public int Id { get; set; }

        [Required]
        [Column("identifier")]
        [StringLength(100)]
        public string Identifier
        {
            get
            {
                return StringHelper.FirstCharToUpper(lowerName);
            }
            set
            {
                lowerName = value.ToLower();
            }
        }

        [Column("species_id")]
        public int SpeciesId { get; set; }

        [Column("order")]
        public int Order { get; set; }

        [Column("is_default")]
        public bool IsDefault { get; set; }

        public string Image
        {
            get
            {
                if (IsDefault)
                {
                    return DataPaths.Sprite + SpeciesId.ToString() + ".png";
                }
                else
                {
                    string specialFormId = lowerName.Split('-', 2)[1];
                    return DataPaths.Sprite + SpeciesId.ToString() + "-" + specialFormId + ".png";
                }
            }
        }

        [NotMapped]
        public List<Type> Types { get; set; }

        private string lowerName;
    }
}
