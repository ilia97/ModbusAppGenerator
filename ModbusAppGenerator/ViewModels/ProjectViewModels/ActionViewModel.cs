using System.ComponentModel.DataAnnotations;
using ModbusAppGenerator.DataAccess.Enums;

namespace ModbusAppGenerator.ViewModels.ProjectViewModels
{
    public class ActionViewModel
    {
        public int? Id { set; get; }

        public int? Number { set; get; }

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

        [Required]
        [Display(Name = "Action Type")]
        public ActionTypes ActionType { set; get; }
        
        [Display(Name = "Formula")]
        public string Formula { set; get; }

        public int ProjectId { set; get; }
    }
}
