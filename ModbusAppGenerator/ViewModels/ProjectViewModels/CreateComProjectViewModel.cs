using ModbusAppGenerator.DataAccess.Enums;

namespace ModbusAppGenerator.ViewModels.ProjectViewModels
{
    public class CreateComProjectViewModel : CreateProjectViewModel
    {
        public string PortName { set; get; }

        public int BaudRate { set; get; }

        public int DataBits { set; get; }

        public Parity Parity { set; get; }

        public StopBits StopBits { set; get; }
    }
}
