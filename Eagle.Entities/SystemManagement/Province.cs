using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Eagle.Entities.SystemManagement
{
    [Table("Province")]
    public class Province : EntityBase
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int ProvinceId { get; set; }
        public string ProvinceCode { get; set; }
        public string ProvinceName { get; set; }
        public int ListOrder { get; set; }
        public bool IsActive { get; set; }
        public int CountryId { get; set; }

        [NotMapped]
        public virtual ICollection<Region> Regions { get; set; }
    }
}
