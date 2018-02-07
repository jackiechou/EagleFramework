using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Eagle.Core.Settings;

namespace Eagle.Entities.Business.Transactions
{
    [Table("Sales.TransactionMethod")]
    public class TransactionMethod : EntityBase
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int TransactionMethodId { get; set; }
        public string TransactionMethodName { get; set; }
        public decimal? TransactionMethodFee { get; set; }
        public TransactionMethodStatus? IsActive { get; set; }
    }
}
