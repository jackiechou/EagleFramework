using System;
using Eagle.Services.Dtos.Common;

namespace Eagle.Services.Exceptions
{

    [Serializable]
    public class NotFoundDataException : BaseException
    {
        public NotFoundDataException()
        {
            ErrorCode = ErrorCode.NotFoundData;
        }

        public NotFoundDataException(string message)
            : base(message)
        {
            ErrorCode = ErrorCode.NotFoundData;
        }

        public NotFoundDataException(string message, System.Exception inner)
            : base(message, inner)
        {
            ErrorCode = ErrorCode.NotFoundData;
        }
    }
}
