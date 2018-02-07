using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Eagle.Entities.Business.Reports
{
    [Table("ReportType")]
    public class ReportType : EntityBase
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        public Guid ApplicationId { get; set; }
        public int ParentId { get; set; }
        public string CultureCode { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public int SortKey { get; set; }
        public int Depth { get; set; }
        public DateTime? PostedDate { get; set; }
        public DateTime? LastUpdatedDate { get; set; }
        public bool Status { get; set; }
    }
}
