using System;
using System.Collections.Generic;
using System.IO.Ports;
using System.Text;

namespace ModbusAppGenerator.Core.Models
{
    public class ComConnectionSettings: ConnectionSettings
    {
        public string PortName { set; get; }

        public int BaudRate { set; get; }

        public int DataBits { set; get; }

        //public Parity Parity { set; get; }

        //public StopBits StopBits { set; get; }
    }
}
