using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Eagle.Entities.SystemManagement
{
    [Table("UserRoleGroup")]
    public class UserRoleGroup : EntityBase
    {
        [Key, Column(Order = 0)]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int UserRoleGroupId { get; set; }
        public Guid UserId { get; set; }
        public int RoleGroupId { get; set; }

        public DateTime? EffectiveDate { get; set; }
        public DateTime? ExpiryDate { get; set; }
        public bool? IsDefault { get; set; }
    }
}