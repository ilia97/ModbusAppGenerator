using System.IO.Ports;

namespace ModbusAppGenerator.ModbusApp.Core.Models
{
    public class MasterSettingsCom : MasterSettings
    {
        public string PortName { set; get; }

        public int BaudRate { set; get; }

        public int DataBits { set; get; }

        public Parity Parity { set; get; }

        public StopBits StopBits { set; get; }
    }
}
