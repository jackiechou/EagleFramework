using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Eagle.Entities.Business.Transactions;

namespace Eagle.Entities.Business.Vendors
{
    [Table("Purchasing.VendorTransactionMethod")]
    public class VendorTransactionMethod : EntityBase
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int VendorTransactionMethodId { get; set; }
        public int VendorId { get; set; }
        public int TransactionMethodId { get; set; }
        public bool IsSelected { get; set; }

        public virtual TransactionMethod TransactionMethod { get; set; }
    }
}
