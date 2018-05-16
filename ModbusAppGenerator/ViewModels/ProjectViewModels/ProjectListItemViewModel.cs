using System.IO.Ports;
using System.Web.Mvc;
using ModbusAppGenerator.DataAccess.Enums;

namespace ModbusAppGenerator.ViewModels.ProjectViewModels
{
    public class ProjectListItemViewModel
    {
        [HiddenInput]
        public int Id { set; get; }

        public string Name { set; get; }

        public string Description { set; get; }

        public ConnectionTypes ConnectionType { set; get; }

        public string Host { set; get; }

        public int Port { set; get; }

        public string PortName { set; get; }

        public int BaudRate { set; get; }

        public int DataBits { set; get; }

        public Parity Parity { set; get; }

        public StopBits StopBits { set; get; }

        public string ShortDescription
        {
            get
            {
                return Description.Length > 100 ? Description.Substring(0, 100) + "..." : Description;
            }
        }

        public string StopBitsString
        {
            get
            {
                switch (StopBits)
                {
                    case StopBits.None:
                        return "0";
                    case StopBits.One:
                        return "1";
                    case StopBits.OnePointFive:
                        return "1.5";
                    case StopBits.Two:
                        return "2";
                    default:
                        return "";
                }
            }
        }
    }
}
