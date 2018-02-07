using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Eagle.Entities.Business.Transactions;

namespace Eagle.Entities.Business.Vendors
{
    [Table("Purchasing.VendorCreditCard")]
    public class VendorCreditCard : EntityBase
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int VendorCreditCardId { get; set; }
        public int VendorId { get; set; }
        public int CreditCardId { get; set; }

        public virtual CreditCard CreditCard { get; set; }
    }
}
