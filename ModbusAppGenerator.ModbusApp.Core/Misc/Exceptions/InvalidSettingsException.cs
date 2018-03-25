using System;

namespace ModbusAppGenerator.ModbusApp.Core.Misc.Exceptions
{
    public class InvalidSettingsException : Exception
    {
        public InvalidSettingsException()
        {
        }

        public InvalidSettingsException(string message)
        : base(message)
        {
        }

        public InvalidSettingsException(string message, Exception inner)
        : base(message, inner)
        {
        }
    }
}
