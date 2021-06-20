using System;
using System.Collections;
using System.Collections.Generic;

namespace Todo.Common
{
    public class ProtocolBufferParseException : Exception
    {
        public ProtocolBufferParseException(string message) : base(message)
        {
        }
    }
}
