using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Eagle.Entities.SystemManagement
{
    [Table("UserAddress")]
    public class UserAddress : EntityBase
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int UserAddressId { get; set; }

        [Key]
        public int AddressId { get; set; }

        [Key]
        public Guid UserId { get; set; }
        public bool? IsDefault { get; set; }

        [NotMapped]
        public virtual Address Address { get; set; }
    }
}
