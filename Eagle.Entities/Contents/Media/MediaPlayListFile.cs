using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Eagle.Core.Settings;

namespace Eagle.Entities.Contents.Media
{
    [Table("MediaPlayListFile",Schema= "Media")]
    public class MediaPlayListFile : EntityBase
    {
        public MediaPlayListFile()
        {
            Status = MediaPlayListFileStatus.Active;
        }

        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int PlayListFileId { get; set; }
        public int PlayListId { get; set; }
        public int FileId { get; set; }
        public int? ListOrder { get; set; }
        public MediaPlayListFileStatus Status { get; set; }
    }
}
