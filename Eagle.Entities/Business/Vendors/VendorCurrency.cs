using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Eagle.Entities.Business.Transactions;
using Eagle.Entities.SystemManagement;

namespace Eagle.Entities.Business.Vendors
{
    [Table("Purchasing.VendorCurrency")]
    public class VendorCurrency : EntityBase
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int VendorCurrencyId { get; set; }
        public int VendorId { get; set; }
        public string CurrencyCode { get; set; }
        public string CurrencySymbol { get; set; }
        public int? Decimals { get; set; }
        public string DecimalSymbol { get; set; }
        public string ThousandSeparator { get; set; }
        public string PositiveFormat { get; set; }
        public string NegativeFormat { get; set; }

        public virtual CurrencyGroup Currency { get; set; }
    }
}
