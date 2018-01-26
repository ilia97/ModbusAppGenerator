using System;
using System.Collections.Generic;
using System.Text;

namespace ModbusAppGenerator.Core.Models
{
    public class IpConnectionSettings: ConnectionSettings
    {
        public string Host { set; get; }

        public int Port { set; get; }
    }
}
