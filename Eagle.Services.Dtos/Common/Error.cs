using System.Collections.Generic;

namespace Eagle.Services.Dtos.Common
{
    public class Error
    {
        public ErrorCode? ErrorCode { get; set; }

        public string ErrorMessage { get; set; }

        public List<RuleViolation> ExtraInfos { get; set; }

        public Error()
        {
            ExtraInfos = new List<RuleViolation>();
        }
    }
}
