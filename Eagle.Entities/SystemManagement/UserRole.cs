using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Eagle.Entities.SystemManagement
{
    [Table("UserRole")]
    public class UserRole : EntityBase
    {
       
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int UserRoleId { get; set; }

        [Key]
        public Guid UserId { get; set; }

        [Key]
        public Guid RoleId { get; set; }
        public DateTime? EffectiveDate { get; set; }
        public DateTime? ExpiryDate { get; set; }
        public bool? IsTrialUsed { get; set; }
        public bool? IsDefaultRole { get; set; }

        [NotMapped]
        public virtual User User { get; set; }

        [NotMapped]
        public virtual Role Role { get; set; }
    }
}
