using System.Collections.Generic;
using Eagle.Services.Dtos.Common;

namespace Eagle.Services.Validations
{
    public interface IRuleEntity
    {
        List<RuleViolation> GetRuleViolation();
        void Validate();
    }
}
