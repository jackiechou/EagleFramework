using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Eagle.Core.Settings;

namespace Eagle.Entities.Contents.Tags
{
    [Table("Tag")]
    public class Tag : EntityBase
    {
        [Key]
        public int TagId { get; set; }
        public TagType TagType { get; set; }
        public string TagName { get; set; }
    }
}
