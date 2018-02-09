using System.ComponentModel.DataAnnotations;
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
        public int? DataBits { set; get; }

        [Required]
        [Display(Name = "Parity")]
        public Parity? Parity { set; get; }

        [Required]
        [Display(Name = "Stop Bits")]
        public StopBits? StopBits { set; get; }
    }
}
