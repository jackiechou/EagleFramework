using System.ComponentModel.DataAnnotations;
using Eagle.Core.Common;
using Eagle.Core.Logging;
using Eagle.Resources;

namespace Eagle.Services.Dtos.SystemManagement
{
    public class LogEntry : DtoBase
    {
        public LogLevel LogLevel { get; set; }
        public LogType LogType { get; set; }
        public ActionType ActionType { get; set; }

        [Display(ResourceType = typeof(LanguageResource), Name = "OldData")]
        //[Required(ErrorMessageResourceType = typeof(LanguageResource), ErrorMessageResourceName = "Required")]
        //[StringLength(250, ErrorMessageResourceType = typeof(LanguageResource), ErrorMessageResourceName = "CustomStringLength", MinimumLength = 3)]
        public string OldData { get; set; }
        //[Required(ErrorMessageResourceType = typeof(LanguageResource), ErrorMessageResourceName = "Required")]
        //[StringLength(4000, ErrorMessageResourceType = typeof(LanguageResource), ErrorMessageResourceName = "MaxStringLength")]
        [Display(ResourceType = typeof(LanguageResource), Name = "NewData")]
        public string NewData { get; set; }

        public string Thread { get; set; }
        public string Message { get; set; }
        public string Exception { get; set; }
    }
    public class LogDetail : DtoBase
    {
        [Display(ResourceType = typeof(LanguageResource), Name = "Id")]
        public int Id { get; set; }

        [Display(ResourceType = typeof(LanguageResource), Name = "LogTypeId")]
        public int LogTypeId { get; set; }

        [Display(ResourceType = typeof(LanguageResource), Name = "Severity")]
        public LogLevel Severity { get; set; }

        [Display(ResourceType = typeof(LanguageResource), Name = "Message")]
        public string Message { get; set; }

        [Display(ResourceType = typeof(LanguageResource), Name = "Exception")]
        public string Exception { get; set; }

        [Display(ResourceType = typeof(LanguageResource), Name = "Ip")]
        public string Ip { get; set; }

        [Display(ResourceType = typeof(LanguageResource), Name = "PageUrl")]
        public string PageUrl { get; set; }

        [Display(ResourceType = typeof(LanguageResource), Name = "IsActive")]
        public StatusMode IsActive { get; set; }
    }
}
