using System.ComponentModel.DataAnnotations;

namespace ModbusAppGenerator.ViewModels.AccountViewModels
{
    public class ForgotViewModel
    {
        [Required]
        [Display(Name = "Email")]
        public string Email { get; set; }
    }
}