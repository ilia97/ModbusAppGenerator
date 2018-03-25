namespace ModbusAppGenerator.ModbusApp.Core.Models
{
    public class MasterSettingsIp : MasterSettings
    {
        public string Host { set; get; }

        public int Port { set; get; }
    }
}
