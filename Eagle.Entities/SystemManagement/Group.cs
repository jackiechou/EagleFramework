using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Eagle.Entities.SystemManagement
{
    [Table("Group")]
    public class Group : EntityBase
    {
        public Group()
        {
            GroupId = Guid.NewGuid();
            IsActive = true;
        }

        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public Guid GroupId { get; set; } 
        public string GroupName { get; set; }
        public string Description { get; set; }
        public int? ListOrder { get; set; }

        public bool? IsActive { get; set; }

        public Guid ApplicationId { get; set; }
        //public virtual ICollection<User> Users { get; set; } = new HashSet<User>();
        //public virtual ICollection<AppClaim> AppClaims { get; set; } = new HashSet<AppClaim>();
    }
}
