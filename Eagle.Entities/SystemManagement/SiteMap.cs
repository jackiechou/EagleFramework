using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Eagle.Core.Settings;

namespace Eagle.Entities.SystemManagement
{
    [Table("SiteMap")]
    public class SiteMap : EntityBase
    {
        public SiteMap()
        {
            LastModified = DateTime.UtcNow;
            Frequency = SiteMapFrequency.Always;
            Priority = 1;
        }

        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int SiteMapId { get; set; }
        public int? ParentId { get; set; }
        public string Title { get; set; }
        public int? Depth { get; set; }
        public string Lineage { get; set; }
        public bool? HasChild { get; set; }
        public string Action { get; set; }
        public string Controller { get; set; }
        public string Url { get; set; }
        public SiteMapFrequency? Frequency { get; set; }
        public decimal? Priority { get; set; }
        public int ListOrder { get; set; }
        public bool Status { get; set; }
        public DateTime? LastModified { get; set; }

        [NotMapped]
        public virtual ICollection<SiteMap> Children { get; set; }
    }
}
