using Eagle.Services.Dtos.Common;

namespace Eagle.Services.Exceptions
{
   public class NullReferenceObjectException : BaseException
    {
        public NullReferenceObjectException()
        {
            ErrorCode = ErrorCode.NullReference;
        }

        public NullReferenceObjectException(string message)
            : base(message)
        {
            ErrorCode = ErrorCode.NullReference;
        }

        public NullReferenceObjectException(string message, System.Exception inner)
            : base(message, inner)
        {
            ErrorCode = ErrorCode.NullReference;
        }
    }
}
