using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EBYS.Domain.Exceptions
{

    public abstract  class BusinessException : Exception
    {
        public int StatusCode => 400;
        protected BusinessException(string message) : base(message) { }
    }



    public abstract class NotFoundException : Exception
    {
        public int StatusCode => 404;
        protected NotFoundException(string message) : base(message) { }
    }
}

