using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Eagle.Core.Settings;

namespace Eagle.Entities.Contents.Media
{
    [Table("MediaType", Schema = "Media")]
    public class MediaType : BaseEntity
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int TypeId { get; set; }
        public string TypeName { get; set; }
        public string Description { get; set; }
        public string TypeExtension { get; set; }
        public string TypePath { get; set; }
        public int? ListOrder { get; set; }
        public MediaTypeStatus Status { get; set; }
    }
}
