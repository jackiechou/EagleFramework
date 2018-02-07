using System;
using System.Linq;
using Eagle.Services.Dtos.Common;
using Eagle.Services.Validations;

namespace Eagle.Services.Exceptions
{
    internal static class ErrorExtentions
    {
        public static System.Exception ToException(this FailResult failResult)
        {
            var exception = failResult.Errors.Count == 1 ? failResult.Errors[0].ToException() :
                new AggregateException(failResult.Errors.Select(error => error.ToException()).ToList());
            return exception;
        }

        public static System.Exception ToException(this Error error)
        {
            System.Exception exception;
            switch (error.ErrorCode)
            {
                case ErrorCode.Validation:
                    var extraInfos = error.ExtraInfos.Select(extraInfo => new RuleViolation(extraInfo.ErrorCode, extraInfo.PropertyName, extraInfo.PropertyValue, extraInfo.ErrorMessage)).ToList();
                    exception = new ValidationError(extraInfos);
                    break;
                default:
                    exception = new System.Exception(error.ErrorMessage);
                    break;
            }
            return exception;
        }
    }
}
