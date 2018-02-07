using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Eagle.Entities.Business.Shipping;

namespace Eagle.Entities.Business.Vendors
{
    [Table("Purchasing.VendorShippingMethod")]
    public class VendorShippingMethod : EntityBase
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int VendorShippingMethodId { get; set; }
        public int VendorId { get; set; }
        public int ShippingMethodId { get; set; }
        public bool IsSelected { get; set; }

        public virtual ShippingMethod ShippingMethod { get; set; }
    }
}
