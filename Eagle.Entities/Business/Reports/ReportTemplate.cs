using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Eagle.Entities.Business.Reports
{
    [Table("ReportTemplate")]
    public class ReportTemplate : EntityBase
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int ReportTemplateId { get; set; }
        public string ReportTemplateName { get; set; }
        public string ReportTemplateContent { get; set; }
        public bool ReportTemplateDiscontinued { get; set; }

        public int ReportTypeId { get; set; }
    }
}
