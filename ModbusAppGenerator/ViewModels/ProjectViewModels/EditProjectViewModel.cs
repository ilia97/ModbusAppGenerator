using System.ComponentModel.DataAnnotations;
using System.IO.Ports;

namespace ModbusAppGenerator.ViewModels.ProjectViewModels
{
    public abstract class EditProjectViewModel
    {
        public int Id { set; get; }

        [Required]
        [Display(Name = "Name")]
        public string Name { set; get; }

        [Required]
        [Display(Name = "Description")]
        public string Description { set; get; }

        [Display(Name = "Enable Logging")]
        public bool IsLoggerEnabled { set; get; }

        [Required]
        [Display(Name = "Period (seconds)")]
        public int? Period { set; get; }

        [Required]
        [Display(Name = "Timeout (milliseconds)")]
        public int? Timeout { set; get; }
    }
}
