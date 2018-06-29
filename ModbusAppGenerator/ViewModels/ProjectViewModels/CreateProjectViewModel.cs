using System.ComponentModel.DataAnnotations;
using ModbusAppGenerator.DataAccess.Enums;

namespace ModbusAppGenerator.ViewModels.ProjectViewModels
{
    public class CreateProjectViewModel
    {
        [Required]
        [Display(Name = "Name")]
        public string Name { set; get; }

        [Required]
        [Display(Name = "Description")]
        public string Description { set; get; }

        [Required]
        [Display(Name = "Connection Type")]
        public ConnectionTypes? ConnectionType { set; get; }
        
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
