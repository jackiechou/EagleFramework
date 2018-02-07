using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Eagle.Entities.SystemManagement
{
    [Table("FunctionCommand")]
    public class FunctionCommand : EntityBase, IEntity<Guid>
    {
        [Key]
        public Guid Id { get; set; }

        [StringLength(256, ErrorMessage = "Name cannot be longer than 256 characters.")]
        public string Name { get; set; }

        public virtual ICollection<AppClaim> AppClaims { get; set; } = new HashSet<AppClaim>();

        public bool IsDeleted { get; set; }
    }
}
