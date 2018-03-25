using System.ComponentModel.DataAnnotations;
using System.IO.Ports;

namespace ModbusAppGenerator.ViewModels.ProjectViewModels
{
    public class EditComProjectViewModel
    {
        public int Id { set; get; }

        [Required]
        [Display(Name = "Name")]
        public string Name { set; get; }

        [Required]
        [Display(Name = "Description")]
        public string Description { set; get; }

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
