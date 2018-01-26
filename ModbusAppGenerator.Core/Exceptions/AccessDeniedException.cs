using System;
using System.Collections.Generic;
using System.Text;

namespace ModbusAppGenerator.Core.Exceptions
{
    [Serializable()]
    public class AccessDeniedException : Exception
    {
        public AccessDeniedException()
           : base()
        { }

        public AccessDeniedException(string message) :
           base(message)
        {
        }
    }
}
