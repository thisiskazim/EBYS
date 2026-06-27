using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EBYS.Domain.Exceptions
{
    public abstract class BaseException : Exception
    {
        public int StatusCode { get; }
        protected BaseException(string message, int statusCode) : base(message)
        {
            StatusCode = statusCode;
        }
    }

    public class NotFoundException : BaseException
    {
        public NotFoundException(string message) : base(message, 404)
        {
        }
    }
    public class BadRequestException : BaseException
    {
        public BadRequestException(string message) : base(message, 400)
        {
        }
    }
    public class ForbiddenException : BaseException
    {
        public ForbiddenException(string message) : base(message, 403)
        {
        }
    }
    public class UnauthorizedException : BaseException
    {
        public UnauthorizedException(string message) : base(message, 401)
        {
        }
    }
}
