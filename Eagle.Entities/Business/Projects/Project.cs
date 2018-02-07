using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Eagle.Entities.Business.Projects
{
    [Table("Project")]
    public class Project: EntityBase
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int ProjectId { get; set; }
        public string ProjectCode { get; set; }
        public int? ProjectManager { get; set; }
        public int PositionId { get; set; }
        public string FromDate { get; set; }
        public string ToDate { get; set; }
        public string Description { get; set; }
        public int Status { get; set; }
        public int? DepartmentId { get; set; } //CompanyId
    }
}
