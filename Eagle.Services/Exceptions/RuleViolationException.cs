using System;
using System.Collections.Generic;
using Eagle.Services.Dtos.Common;

namespace Eagle.Services.Exceptions
{
    public class RuleViolationException : BaseException
    {
        public RuleViolationException(string message, List<RuleViolation> validationIssues)
            : base(message)
        {
            this.ValidationIssues = validationIssues;
        }

        public List<RuleViolation> ValidationIssues { get; private set; }
    }
}
