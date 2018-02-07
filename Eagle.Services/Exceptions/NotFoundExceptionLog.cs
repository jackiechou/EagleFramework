using Eagle.Services.Dtos.Common;

namespace Eagle.Services.Exceptions
{
    public class NotFoundExceptionLog : BaseException
    {
        public NotFoundExceptionLog()
        {
            ErrorCode = Dtos.Common.ErrorCode.NotFoundLog;
        }

        public NotFoundExceptionLog(string message)
            : base(message)
        {
            ErrorCode = Dtos.Common.ErrorCode.NotFoundLog;
        }

        public NotFoundExceptionLog(string message, System.Exception inner)
            : base(message, inner)
        {
            ErrorCode = Dtos.Common.ErrorCode.NotFoundLog;
        }
    }
}
