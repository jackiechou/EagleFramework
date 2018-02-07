using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Eagle.Entities.Business.Shipping
{
    [Table("ShippingCarrier",Schema = "Sales")]
    public class ShippingCarrier : EntityBase
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int ShippingCarrierId { get; set; }
        public string ShippingCarrierName { get; set; }
        public string Phone { get; set; }
        public string Address { get; set; }
        public int? ListOrder { get; set; }
        public bool IsSelected { get; set; }
        public bool IsActive { get; set; }
        public DateTime? CreatedDate { get; set; }
        public DateTime? LastModifiedDate { get; set; }

        public int VendorId { get; set; }
    }
}
