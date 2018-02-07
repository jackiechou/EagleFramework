using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Eagle.Core.Settings;

namespace Eagle.Entities.Business.Transactions
{
   [Table("CreditCard", Schema = "Purchasing")]
    public class CreditCard : EntityBase
    {
        public CreditCard()
        {
            IsActive = CreditCardStatus.Active;
            ModifiedDate = DateTime.UtcNow;
        }

        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int CreditCardId { get; set; }
        public string CreditCardName { get; set; }
        public string CreditCardCode { get; set; }
        public string CardType { get; set; }
        public string CardNumber { get; set; }
        public int ExpiredMonth { get; set; }
        public int ExpiredYear { get; set; }
        public CreditCardStatus IsActive { get; set; }
        public DateTime? ModifiedDate { get; set; }
    }
}
