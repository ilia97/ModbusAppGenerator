using System.ComponentModel.DataAnnotations;
using System.IO.Ports;
using ModbusAppGenerator.DataAccess.Enums;

namespace ModbusAppGenerator.ViewModels.ProjectViewModels
{
    public class CreateComProjectViewModel
    {
        public int Id { set; get; }

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
    }
}
