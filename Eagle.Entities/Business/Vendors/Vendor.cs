using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Eagle.Core.Settings;

namespace Eagle.Entities.Business.Vendors
{
    [Table("Purchasing.Vendor")]
    public class Vendor : BaseEntity
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int VendorId { get; set; }
        public string VendorName { get; set; }
        public string StoreName { get; set; }
        public string AccountNumber { get; set; }
        public string CopyRight { get; set; }
        public string TaxCode { get; set; }
        public int? Logo { get; set; }
        public string Slogan { get; set; }
        public string Telephone { get; set; }
        public string Mobile { get; set; }
        public string Fax { get; set; }
        public string Email { get; set; }
        public string Hotline { get; set; }
        public string SupportOnline { get; set; }
        public string Website { get; set; }
        public decimal? ClickThroughs { get; set; }
        public int? CreditRating { get; set; }
        public string TermsOfService { get; set; }
        public string Keywords { get; set; }
        public string Summary { get; set; }
        public string Description { get; set; }
        public string RefundPolicy { get; set; }
        public string ShoppingHelp { get; set; }
        public string OrganizationalStructure { get; set; }
        public string FunctionalAreas { get; set; }
        public VendorStatus IsAuthorized { get; set; }

        public virtual ICollection<VendorAddress> Addresses { get; set; }
        public virtual ICollection<VendorCreditCard> CreditCards { get; set; }
        public virtual ICollection<VendorCurrency> Currencies { get; set; }
    }
}
