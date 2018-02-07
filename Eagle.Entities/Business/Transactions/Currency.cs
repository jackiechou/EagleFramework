using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Eagle.Core.Settings;

namespace Eagle.Entities.Business.Transactions
{
    [Table("Currency", Schema = "Purchasing")]
    public class CurrencyGroup : EntityBase
    {
        public CurrencyGroup()
        {
            IsActive =  CurrencyStatus.Active;
            ModifiedDate = DateTime.UtcNow;
        }

        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int CurrencyId { get; set; }
        public string CurrencyCode { get; set; }
        public string CurrencyName { get; set; }
        public bool IsSelected { get; set; }
        public CurrencyStatus IsActive { get; set; }
        public DateTime? ModifiedDate { get; set; }
    }
}
