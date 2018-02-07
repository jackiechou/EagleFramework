using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Eagle.Core.Settings;

namespace Eagle.Entities.Contents.Tags
{
    [Table("TagIntegration")]

    public class TagIntegration: EntityBase
    {
        [Key]
        public int TagIntegrationId { get; set; }
        public TagType TagType { get; set; }
        public int TagKey { get; set; }
        public int TagId { get; set; }
        public TagStatus TagStatus { get; set; }
        public DateTime CreatedDate { get; set; }
        public DateTime? LastModifiedDate { get; set; }
    }
}
