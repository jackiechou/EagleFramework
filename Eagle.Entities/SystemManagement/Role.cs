using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Eagle.Entities.SystemManagement
{
    /// <summary>
    /// User role - curently maps to db table PermissionDefaultUserRole
    /// </summary>
    [Table("Role")]
    public class Role : EntityBase //, IRole
    {
        public Role()
        {
            RoleId = Guid.NewGuid();
        }

        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public Guid RoleId { get; set; }
        public string RoleCode { get; set; }
        public string RoleName { get; set; }
        public string LoweredRoleName { get; set; }
        public string Description { get; set; }
        public int? ListOrder { get; set; }
        public bool? IsActive { get; set; }

        public Guid ApplicationId { get; set; }

        ///// <summary>
        ///// eager load user role
        ///// </summary>
        //public ICollection<UserRole> UserRoles { get; set; }

        //public virtual RoleGroup Group { get; set; }
        //public virtual ICollection<User> Users { get; set; }
        //public virtual ICollection<UserRole> UserRoles { get; set; }
        //public virtual ICollection<UserProfile> UserProfiles { get; set; }
    }
}
