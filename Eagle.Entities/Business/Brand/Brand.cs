using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Eagle.Entities.Business.Brand
{
    [Table("Production.Brand")]
    public class Brand : EntityBase
    {
        [NotMapped]
        public int Id => BrandId;

        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int BrandId { get; set; }
        public string BrandName { get; set; }
        public string BrandAlias { get; set; }
        public bool? IsOnline { get; set; }
        public int? FileId { get; set; }
    }
}
