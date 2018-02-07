using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Eagle.Core.Settings;

namespace Eagle.Entities.SystemManagement
{
    [Table("Contact")]
    public class Contact : EntityBase
    {
        public Contact()
        {
            CreatedOn = DateTime.UtcNow;
        }

        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int ContactId { get; set; }
        public string Title { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string DisplayName { get; set; }
        public SexType Sex { get; set; }
        public string JobTitle { get; set; }
        public DateTime? Dob { get; set; }
        public int? Photo { get; set; }
        public string LinePhone1 { get; set; }
        public string LinePhone2 { get; set; }
        public string Mobile { get; set; }
        public string Fax { get; set; }
        public string Email { get; set; }
        public string Website { get; set; }
        public string IdNo { get; set; }
        public DateTime? IdIssuedDate { get; set; }
        public string TaxNo { get; set; }
        public bool? IsActive { get; set; }
        public DateTime CreatedOn { get; set; }
        public DateTime? ModifiedOn { get; set; }
    }
}
