using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Eagle.Entities.Business.Shipping;

namespace Eagle.Entities.Business.Vendors
{
    [Table("Purchasing.VendorShippingCarrier")]
    public class VendorShippingCarrier : EntityBase
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int VendorShippingCarrierId { get; set; }
        public int VendorId { get; set; }
        public int ShippingCarrierId { get; set; }
        public bool IsSelected { get; set; }

        public virtual ShippingCarrier ShippingCarrier { get; set; }
    }
}