using System.ComponentModel.DataAnnotations;

namespace Eagle.Core.Logging
{
    public enum LogLevel
    {
        [Display(Name = "Debug")]
        Debug,
        [Display(Name = "Info")]
        Info,
        [Display(Name = "Warn")]
        Warn,
        [Display(Name = "Error")]
        Error,
        [Display(Name = "Fatal")]
        Fatal
    }
}