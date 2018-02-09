using System.Collections.Generic;
using ModbusAppGenerator.DataAccess.Enums;

namespace ModbusAppGenerator.ViewModels.ProjectViewModels
{
    public class DetailsViewModel
    {
        public int Id { set; get; }

        public string Name { set; get; }
        
        public string Description { set; get; }
        
        public ConnectionTypes? ConnectionType { set; get; }

        public string Host { set; get; }
        
        public int? Port { set; get; }

        public string PortName { set; get; }
        
        public int? BaudRate { set; get; }
        
        public int? DataBits { set; get; }
        
        public Parity? Parity { set; get; }
        
        public StopBits? StopBits { set; get; }

        public List<ActionViewModel> Actions { set; get; }
    }
}
