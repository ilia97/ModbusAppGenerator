using System.ComponentModel.DataAnnotations;

namespace ModbusAppGenerator.ViewModels.ProjectViewModels
{
    public class ActionViewModel
    {
        public int Id { set; get; }

        [Required]
        [Display(Name = "Slave Address")]
        public int? SlaveAddress { set; get; }

        [Required]
        [Display(Name = "Start Address")]
        public int? StartAddress { set; get; }

        [Required]
        [Display(Name = "Number of Registers")]
        public int? NumberOfRegisters { set; get; }

        [Required]
        [Display(Name = "Types")]
        public string Types { set; get; }
    }
}
