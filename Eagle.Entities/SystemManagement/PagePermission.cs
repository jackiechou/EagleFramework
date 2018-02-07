using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Eagle.Entities.SystemManagement
{
    [Table("PagePermission")]
    public class PagePermission : EntityBase
    {
        [Key, Column(Order = 1)]
        public Guid RoleId { get; set; }

        [Key, Column(Order = 2)]
        public int PageId { get; set; }
        public bool AllowAccess { get; set; }

        public string UserIds { get; set; }

        public virtual Role Role { get; set; }
        public virtual Page Page { get; set; }
    }
}
