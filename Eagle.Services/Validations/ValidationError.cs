using System.Collections.Generic;
using Eagle.Services.Dtos.Common;
using Eagle.Services.Exceptions;

namespace Eagle.Services.Validations
{
    public sealed class ValidationError : BaseException
    {
        public ValidationError(List<RuleViolation> violations)
        {
            ErrorCode = ErrorCode.Validation;
            Data.Add("ValidationErrors", violations);
        }
    }
}
