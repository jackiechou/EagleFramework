using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Eagle.Entities.SystemManagement
{
    [Table("ContentType")]
    public class ContentType: EntityBase
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int ContentTypeId { get; set; }
        public string ContentTypeName { get; set; }

        public Guid ApplicationId { get; set; }
    }
}
