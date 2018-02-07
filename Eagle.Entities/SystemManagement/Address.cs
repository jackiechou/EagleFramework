using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Eagle.Core.Settings;

namespace Eagle.Entities.SystemManagement
{
   [Table("Address")]
    public class Address : EntityBase
    {
        public Address()
        {
            CreatedDate = DateTime.UtcNow;
        }
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int AddressId { get; set; }
        public AddressType AddressTypeId { get; set; }
        public string Street { get; set; }
        public string PostalCode { get; set; }
        public string Description { get; set; }
        public DateTime? CreatedDate { get; set; }
        public DateTime? ModifiedDate { get; set; }
        
        public int? CountryId { get; set; }
        public int? ProvinceId { get; set; }
        public int? RegionId { get; set; }
    }
}
