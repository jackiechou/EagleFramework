using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Eagle.Core.Settings;

namespace Eagle.Entities.Business.Transactions
{
    [Table("Sales.PaymentMethod")]
    public class PaymentMethod : EntityBase
    {
        public PaymentMethod()
        {
            IsCreditCard = false;
            IsSelected = false;
        }

        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int PaymentMethodId { get; set; }
        public string PaymentMethodName { get; set; }
        public bool IsCreditCard { get; set; }
        public bool IsSelected { get; set; }
        public PaymentMethodStatus? IsActive { get; set; }
    }
}
