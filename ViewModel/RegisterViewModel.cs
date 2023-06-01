using System.ComponentModel.DataAnnotations;

namespace MVCTutorial.ViewModel
{
    public class RegisterViewModel
    {

        [Required(ErrorMessage = "Email Address is required")]
        [Display(Name = "Email Address")]
        public string EmailAddress { get; set; }
        [Required]
        [DataType(DataType.Password)]
        public string Password { get; set; }
        [Required(ErrorMessage = "Confirm Password is required")]
        [DataType(DataType.Password)]
        [Display (Name = "Confirm Password")]
        public string ConfirmPassword { get; set; }
    }
}
