using System.Web.Http.ModelBinding;
using Eagle.Services.Exceptions;

namespace Eagle.Services.Validations
{
    public class Validation
    {
        public static void UpdateModelStateWithViolations(RuleViolationException ruleViolationException, ModelStateDictionary modelState)
        {
            foreach (var issue in ruleViolationException.ValidationIssues)
            {
                var value = issue.PropertyValue ?? string.Empty;
                //modelState.AddModelError(issue.PropertyName, value.ToString(), issue.ErrorMessage);
                modelState.AddModelError(issue.PropertyName, issue.ErrorMessage);
            }
        }
    }
}
