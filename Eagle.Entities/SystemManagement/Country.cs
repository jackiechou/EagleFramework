using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Eagle.Entities.SystemManagement
{
    /// <summary>
    /// Represents a country
    /// </summary>
    [Table("Country")]
    public class Country : EntityBase
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int CountryId { get; set; }
        public string CountryName { get; set; }
        public string NiceName { get; set; }
        public string Iso { get; set; }
        public string Iso3 { get; set; }
        public int? NumCode { get; set; }
        public int PhoneCode { get; set; }
        public bool IsActive { get; set; }

        [NotMapped]
        public virtual ICollection<Province> Provinces { get; set; }
    }
}
