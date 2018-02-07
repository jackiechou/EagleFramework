using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Eagle.Entities.SystemManagement;

namespace Eagle.Entities.Business.Vendors
{
    [Table("Purchasing.VendorAddress")]
    public class VendorAddress : EntityBase
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int VendorAddressId { get; set; }
        public int VendorId { get; set; }
        public int AddressId { get; set; }
        public DateTime ModifiedDate { get; set; }
    }
}
