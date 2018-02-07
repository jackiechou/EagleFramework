using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Eagle.Entities.Business.Employees
{
    [Table("Termination", Schema = "Personnel")]
    public class Termination: EntityBase
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int TerminationId { get; set; }
        public int EmployeeId { get; set; }
        public string Reason { get; set; }
        public DateTime InformedDate { get; set; }
        public DateTime LastWorkingDate { get; set; }
        public bool? IsTerminationPaid { get; set; }
        public DateTime? SignDate { get; set; }
        public int? Signer { get; set; }
    }
}
