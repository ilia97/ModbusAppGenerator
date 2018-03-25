using System;

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
