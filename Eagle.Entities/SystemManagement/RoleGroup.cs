using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Eagle.Entities.SystemManagement
{
    [Table("RoleGroup")]
    public class RoleGroup : EntityBase
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int RoleGroupId { get; set; }

        [Key]
        public Guid RoleId { get; set; }

        [Key]
        public Guid GroupId { get; set; }
        public bool? IsDefaultGroup { get; set; }
    }
}
