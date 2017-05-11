using System.ComponentModel.DataAnnotations;

namespace ServiceBooking.WEB.Models
{
    public class LoginViewModel
    {
        [Required(ErrorMessage = "Email is required")]
        [Display(Name = "Email")]
        [EmailAddress(ErrorMessage = "Field E-mail contains invalid E-mail address")]
        public string Email { get; set; }

        [Required(ErrorMessage = "Password is required")]
        [DataType(DataType.Password, ErrorMessage = "Field Password contains invalid characters")]
        [Display(Name = "Password")]
        public string Password { get; set; }

        [Display(Name = "Remember me?")]
        public bool RememberMe { get; set; }
    }

    public class RegisterViewModel
    {
        [Required(ErrorMessage = "Surname is required")]
        [RegularExpression(@"^[a-zA-Zа-яА-ЯёЁ]{1,20}$", ErrorMessage = "Incorrect surname")]
        [StringLength(50, ErrorMessage = "The {0} must be from 1 to 50 characters long.", MinimumLength = 1)]
        [Display(Name = "Surname")]
        public string Surname { get; set; }

        [Required(ErrorMessage = "Name is required")]
        [RegularExpression(@"^[a-zA-Zа-яА-ЯёЁ]{1,20}$", ErrorMessage = "Incorrect name")]
        [StringLength(50, ErrorMessage = "The {0} must be from 1 to 50 characters long.", MinimumLength = 1)]
        [Display(Name = "Name")]
        public string Name { get; set; }

        [Required(ErrorMessage = "Email is required")]
        [EmailAddress(ErrorMessage = "Field E-mail contains invalid E-mail address")]
        [Display(Name = "Email")]
        public string Email { get; set; }

        [Required(ErrorMessage = "Password is required")]
        [StringLength(30, ErrorMessage = "The {0} must be from 8 to 30 characters long.", MinimumLength = 8)]
        [DataType(DataType.Password, ErrorMessage = "Field Password contains invalid characters")]
        [Display(Name = "Password")]
        public string Password { get; set; }

        [DataType(DataType.Password, ErrorMessage = "Field Confirm Password contains invalid characters")]
        [StringLength(30, ErrorMessage = "The {0} must be from 8 to 30 characters long.", MinimumLength = 8)]
        [Compare("Password", ErrorMessage = "The password and confirmation password do not match.")]
        [Display(Name = "Confirm password")]
        public string ConfirmPassword { get; set; }
    }

    public class DeleteAccountViewModel
    {
        [Required(ErrorMessage = "Password is required")]
        [DataType(DataType.Password, ErrorMessage = "Field Password contains invalid characters")]
        [Display(Name = "Password")]
        public string Password { get; set; }
    }

    public class ForgotPasswordViewModel
    {
        [Required(ErrorMessage = "Email is required")]
        [EmailAddress(ErrorMessage = "Field E-mail contains invalid E-mail address")]
        [Display(Name = "Email")]
        public string Email { get; set; }
    }
}