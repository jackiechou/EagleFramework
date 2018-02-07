using System.Runtime.Serialization;
using Eagle.Services.Dtos.Common;

namespace Eagle.Services.Exceptions
{
    /// <summary>
    /// Base Exception for Eagle
    /// </summary>
    public class BaseException : System.Exception
    {
        public ErrorCode ErrorCode { get; set; }
        public bool LogException { get; set; }

        public BaseException()
        {
            // Add implementation (if required)
            ErrorCode = ErrorCode.Error;
        }

        public BaseException(string message)
            : base(message)
        {
            // Add implementation (if required)
            ErrorCode = ErrorCode.Error;
        }

        public BaseException(string message, System.Exception inner)
            : base(message, inner)
        {
            // Add implementation (if required)
            ErrorCode = ErrorCode.Error;
        }

        protected BaseException(SerializationInfo info, StreamingContext context)
            : base(info, context)
        {
            // Add implementation (if required)
            ErrorCode = ErrorCode.Error;
        }

    }
}
