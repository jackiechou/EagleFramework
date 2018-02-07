
namespace Eagle.Services.Dtos.Common
{
    public class RuleViolation
    {
        public ErrorCode ErrorCode { get; set; }
        public string PropertyName { get; set; }
        public object PropertyValue { get; set; }
        public string ErrorMessage { get; set; }

        public RuleViolation(string errorMessage, string propertyName)
        {
            PropertyName = propertyName;
            ErrorMessage = errorMessage;
        }

        public RuleViolation(ErrorCode errorCode, string propertyName=null, object propertyValue = null, string errorMessage = null)
        {
            ErrorCode = errorCode;
            PropertyName = propertyName;
            PropertyValue = propertyValue;
            ErrorMessage = errorMessage??Common.ErrorMessage.Messages[errorCode];
        }
    }
}
