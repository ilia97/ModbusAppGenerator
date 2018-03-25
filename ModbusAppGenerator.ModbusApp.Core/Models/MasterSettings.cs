using System.Collections.Generic;

namespace ModbusAppGenerator.ModbusApp.Core.Models
{
    public class MasterSettings
    {
        public MasterSettings()
        {
            SlaveSettings = new List<GroupSettings>();
        }
        
        public bool IsLoggerEnabled { set; get; }
        
        public int StatFlushPeriod { set; get; }
        
        public int Timeout { set; get; }
        
        public int Period { set; get; }
        
        public List<GroupSettings> SlaveSettings { set; get; }
    }
}
