using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Eagle.Core.Logging;

namespace Eagle.Entities.SystemManagement
{
    [Table("Log")]
    public class Log : EntityBase
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int LogId { get; set; }
        public LogLevel LogLevel { get; set; }
        public LogType LogType { get; set; }
        public ActionType ActionType { get; set; }
        public string OldData { get; set; }
        public string NewData { get; set; }
        public string Thread { get; set; }
        public string Message { get; set; }
        public string Exception { get; set; }
        public Guid Logger { get; set; }
        public DateTime LogDate { get; set; }

        public Guid ApplicationId{ get; set; }
    }
}
