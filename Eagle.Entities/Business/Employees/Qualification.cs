using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Eagle.Entities.Business.Employees
{
    [Table("Qualification", Schema = "Personnel")]
    public class Qualification: EntityBase
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int QualificationId { get; set; }
        public string QualificationNo { get; set; }
        public DateTime? QualificationDate { get; set; }
        public int? FileId { get; set; }
        public string Note { get; set; }

        public int EmployeeId { get; set; }
    }
}
