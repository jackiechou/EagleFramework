using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Eagle.Core.Settings;

namespace Eagle.Entities.Business.Customers
{
    [Table("Customer",Schema = "Sales")]
    public class Customer : BaseEntity
    {
        public Customer()
        {
            CustomerTypeId = 1;
            Gender = Sex.NoneSpecified;
            IsActive = CustomerStatus.Published;
        }
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int CustomerId { get; set; }
        public int CustomerTypeId { get; set; }
        public string CustomerNo { get; set; }
        public string CardNo { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string ContactName { get; set; }
        public string IdCardNo { get; set; }
        public string PassPortNo { get; set; }
        public string TaxCode { get; set; }
        public int? Photo { get; set; }
        public Sex Gender { get; set; }
        public DateTime? BirthDay { get; set; }
        public string HomePhone { get; set; }
        public string WorkPhone { get; set; }
        public string Mobile { get; set; }
        public string Fax { get; set; }
        public string Email { get; set; }
        public string PasswordHash { get; set; }
        public string PasswordSalt { get; set; }
        public bool Verified { get; set; }
        public CustomerStatus IsActive { get; set; }


        public int? VendorId { get; set; }
        public int? AddressId { get; set; }
    }
}
