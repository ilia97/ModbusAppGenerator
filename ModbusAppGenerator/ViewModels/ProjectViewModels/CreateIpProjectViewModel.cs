using System.ComponentModel.DataAnnotations;

namespace ModbusAppGenerator.ViewModels.ProjectViewModels
{
    public class CreateIpProjectViewModel
    {
        public int Id { set; get; }

        [Required]
        [RegularExpression(@"\b\d{1,3}\.\d{1,3}\.\d{1,3}\.\d{1,3}\b", ErrorMessage = "Wrong host format.")]
        public string Host { set; get; }

        [Required]
        public int? Port { set; get; }
    }
}
