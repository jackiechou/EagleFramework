using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Eagle.Core.Settings;

namespace Eagle.Entities.Business.Companies
{
    [Table("Company")]
    public class Company: BaseEntity
    {
        public Company()
        {
            Depth = 1;
            ParentId = 0;
            HasChild = false;
        }

        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int CompanyId { get; set; }
        public int? ParentId { get; set; }
        public int? Depth { get; set; }
        public string Lineage { get; set; }
        public bool? HasChild { get; set; }
        public string CompanyName { get; set; }
        public int? Logo { get; set; }
        public string Slogan { get; set; }
        public string Fax { get; set; }
        public string Hotline { get; set; }
        public string Mobile { get; set; }
        public string Telephone { get; set; }
        public string Email { get; set; }
        public string Website { get; set; }
        public string SupportOnline { get; set; }
        public string CopyRight { get; set; }
        public string TaxCode { get; set; }
        public string Description { get; set; }
        public int? ListOrder { get; set; }
        public CompanyStatus Status { get; set; }

        public int? AddressId { get; set; }
    }
}
