using Eagle.Services.Dtos.Common;

namespace Eagle.Services.Exceptions
{
    public class NoAccessException : BaseException
    {
         public NoAccessException()
         {
            ErrorCode = ErrorCode.NoAccessData;
        }

        public NoAccessException(string message)
            : base(message)
        {
            ErrorCode = ErrorCode.NoAccessData;
        }

        public NoAccessException(string message, System.Exception inner)
            : base(message, inner)
        {
            ErrorCode = ErrorCode.NoAccessData;
        }
    }
}