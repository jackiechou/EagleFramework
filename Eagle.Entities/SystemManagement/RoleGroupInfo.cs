using System.ComponentModel.DataAnnotations.Schema;

namespace Eagle.Entities.SystemManagement
{
    [NotMapped]
    public class RoleGroupInfo : RoleGroup
    {
        [NotMapped]
        public Role Role { get; set; }


        [NotMapped]
        public Group Group { get; set; }
    }
}
