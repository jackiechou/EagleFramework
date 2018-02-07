using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace Eagle.Entities.SystemManagement
{
    [NotMapped]
    public class UserRoleGroupInfo: UserRoleGroup
    {
        [NotMapped]
        public virtual RoleGroup RoleGroup { get; set; }

        [NotMapped]
        public virtual Role Role { get; set; }

        [NotMapped]
        public virtual Group Group { get; set; }

        [NotMapped]
        public virtual User User { get; set; }
    }
}
