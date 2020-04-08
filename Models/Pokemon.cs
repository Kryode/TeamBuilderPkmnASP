using KryodeHelpers;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TeamBuilderPkmnASP.Models
{
    public partial class Pokemon
    {
        [Required]
        [Column("identifier")]
        [StringLength(100)]
        private string lowerName;
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
                return DataPaths.Sprite + SpeciesId.ToString() + ".png";
            }
        }
    }
}
