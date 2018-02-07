using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Eagle.Core.Settings;

namespace Eagle.Entities.Contents.Media
{
    [Table("MediaComposer", Schema = "Media")]
    public class MediaComposer : BaseEntity
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int ComposerId { get; set; }
        public string ComposerName { get; set; }
        public string ComposerAlias { get; set; }
        public int Photo { get; set; }
        public string Description { get; set; }
        public int? ListOrder { get; set; }
        public MediaComposerStatus Status { get; set; }
    }
}
