using System.ComponentModel.DataAnnotations;
using System.IO.Ports;

namespace ModbusAppGenerator.ViewModels.ProjectViewModels
{
    public class EditComProjectViewModel: EditProjectViewModel
    {
        [Required]
        [Display(Name = "Port Name")]
        public string PortName { set; get; }

        [Required]
        [Display(Name = "Baud Rate")]
        public int? BaudRate { set; get; }

        [Required]
        [Display(Name = "Data Bits")]
        [Range(5, 8)]
        public int? DataBits { set; get; }

        [Required]
        [Display(Name = "Parity")]
        public Parity? Parity { set; get; }

        [Required]
        [Display(Name = "Stop Bits")]
        public StopBits? StopBits { set; get; }

        public string StringStopBits
        {
            get
            {
                switch (StopBits)
                {
                    case System.IO.Ports.StopBits.None:
                        return "0";
                    case System.IO.Ports.StopBits.One:
                        return "1";
                    case System.IO.Ports.StopBits.OnePointFive:
                        return "1.5";
                    case System.IO.Ports.StopBits.Two:
                        return "2";
                    default:
                        return "";
                }
            }
        }
    }
}
