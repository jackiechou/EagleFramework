using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Eagle.Entities.SystemManagement
{
    [Table("ContentItem")]
    public class ContentItem : EntityBase
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int ContentItemId { get; set; }
        public string ItemKey { get; set; }
        public string ItemText { get; set; }
        public bool IsActive { get; set; }

        public int ContentTypeId { get; set; }
        public virtual ContentType ContentType { get; set; }
    }
}
