using System.Net;

namespace BusinessLogicLayer.Exceptions
{
    public abstract class ApiException:Exception
    {
            public HttpStatusCode StatusCode { get; set; }

            protected ApiException(string message, HttpStatusCode statusCode) : base(message)
            {
                StatusCode = statusCode;
            }
        }

        public class BadRequestException : ApiException
        {
            public BadRequestException(string message) : base(message, HttpStatusCode.BadRequest)
            {
            }

       }
    public class KeyNotFoundException : ApiException
    {
        public KeyNotFoundException(string message) : base(message, HttpStatusCode.Unauthorized) { }
    }
    public class NotFoundException : ApiException
    {
        public NotFoundException(string message) : base(message,HttpStatusCode.NotFound) { }
    }
    public class NotImplementedException : ApiException
    {
        public NotImplementedException(string message) : base(message,HttpStatusCode.NotImplemented) { }
    }
    public class UnauthorizedAccessException : ApiException
    {
        public UnauthorizedAccessException(string message) : base(message,HttpStatusCode.Unauthorized) { }
    }
    public class InternalServerException : ApiException
    {
        public InternalServerException(string message) : base(message, HttpStatusCode.InternalServerError) { }
    }
}
