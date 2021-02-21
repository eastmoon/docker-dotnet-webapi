using System;
using WebService.Core.Common.Enums;

namespace WebService.Core.Common.Exceptions
{
    public class BaseException : ApplicationException
    {
        public HttpStatusCode StatusCode { get; } = HttpStatusCode.ServiceInternalError;

        public BaseException(string message)
            : base(message)
        {
        }

        public BaseException(string message, HttpStatusCode statusCode)
            : base(message)
        {
            StatusCode = statusCode;
        }

        public BaseException(string message, Exception innerException)
            : base(message, innerException)
        {
        }

        public BaseException(string message, Exception innerException, HttpStatusCode statusCode)
            : base(message, innerException)
        {
            StatusCode = statusCode;
        }
    }
}
