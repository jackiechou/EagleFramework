using Eagle.Services.Dtos.Common;

namespace Eagle.Services.Exceptions
{
    public class ExistedDataException : BaseException
    {
        public ExistedDataException()
        {
            ErrorCode = ErrorCode.ExistedData;
        }

        public ExistedDataException(string message)
            : base(message)
        {
            ErrorCode = ErrorCode.ExistedData;
        }

        public ExistedDataException(string message, System.Exception inner)
            : base(message, inner)
        {
            ErrorCode = ErrorCode.ExistedData;
        }
    }
}
