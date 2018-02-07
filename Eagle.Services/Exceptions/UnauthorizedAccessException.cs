using Eagle.Services.Dtos.Common;

namespace Eagle.Services.Exceptions
{
    public class UnauthorizedAccessException : BaseException
    {
        public UnauthorizedAccessException()
        {
            ErrorCode = ErrorCode.UnauthorizedAsccessError;
        }

        public UnauthorizedAccessException(string message)
            : base(message)
        {
            ErrorCode = ErrorCode.UnauthorizedAsccessError;
        }

        public UnauthorizedAccessException(string message, System.Exception inner)
            : base(message, inner)
        {
            ErrorCode = ErrorCode.UnauthorizedAsccessError;
        }
    }
}
