using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Eagle.Entities.Business.Transactions;

namespace Eagle.Entities.Business.Vendors
{
    [Table("Purchasing.VendorPaymentMethod")]
    public class VendorPaymentMethod : EntityBase
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int VendorPaymentMethodId { get; set; }
        public int VendorId { get; set; }
        public int PaymentMethodId { get; set; }
        public bool IsSelected { get; set; }

        public virtual PaymentMethod PaymentMethod { get; set; }
    }
}
