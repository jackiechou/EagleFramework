using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Eagle.Resources;

namespace Eagle.Entities.SystemManagement
{
    [Table("AppClaim")]
    public class AppClaim : EntityBase
    {
        [Key]
        public Guid ClaimId { get; set; }

        [StringLength(500, ErrorMessage = "Key cannot be longer than 500 characters.")]
        public string Key { get; set; }

        [StringLength(500, ErrorMessage = "Name cannot be longer than 500 characters.")]
        public string Value { get; set; }
      
        public Guid GroupId { get; set; }
        public bool IsDeleted { get; set; }
        //public virtual ICollection<FunctionCommand> FunctionCommands { get; set; } = new HashSet<FunctionCommand>();

        //public virtual ICollection<RoleGroup> RoleGroups { get; set; } = new HashSet<RoleGroup>();


    }
}
