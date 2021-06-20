using System;
using System.Collections;
using System.Collections.Generic;

namespace Todo.Common
{
    public class HttpException : Exception
    {
        public HttpException(string message) : base(message)
        {
        }
    }
}
