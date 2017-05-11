using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace ServiceBooking.WEB.Models
{
    public class IndexManageViewModel
    {
        [Required(ErrorMessage = "Current password is required")]
        [DataType(DataType.Password)]
        [Display(Name = "Current password")]
        public string OldPassword { get; set; }

        [Required(ErrorMessage = "New password is required")]
        [StringLength(30, ErrorMessage = "The {0} must be from 8 to 30 characters long.", MinimumLength = 8)]
        [DataType(DataType.Password, ErrorMessage = "Field Confirm Password contains invalid characters")]
        [Display(Name = "New password")]
        public string NewPassword { get; set; }

        [Required(ErrorMessage = "New password is required")]
        [StringLength(30, ErrorMessage = "The {0} must be from 8 to 30 characters long.", MinimumLength = 8)]
        [DataType(DataType.Password, ErrorMessage = "Field Confirm Password contains invalid characters")]
        [Display(Name = "Confirm new password")]
        [Compare("NewPassword", ErrorMessage = "The new password and confirmation password do not match.")]
        public string ConfirmPassword { get; set; }

        public bool IsPerformer { get; set; }

        public bool AdminStatus { get; set; }
    }

    public class BecomePerformerViewModel
    {
        [Display(Name = "Company (optional)")]
        [StringLength(50, ErrorMessage = "The {0} must be from 2 to 50 characters long.", MinimumLength = 2)]
        public string Company { get; set; }

        [Required(ErrorMessage = "Phone number is required")]
        [Display(Name = "Phone number")]
        [DataType(DataType.PhoneNumber, ErrorMessage = "Field Confirm Password contains invalid characters")]
        [StringLength(20, ErrorMessage = "The {0} must be from 5 to 20 characters long.", MinimumLength = 5)]
        public string PhoneNumber { get; set; }

        [Required(ErrorMessage = "Activity information is required")]
        [Display(Name = "Activity information")]
        [DataType(DataType.MultilineText)]
        [StringLength(1000, ErrorMessage = "The {0} must be from 10 to 1000 characters long.", MinimumLength = 10)]
        public string Info { get; set; }

        [Required(ErrorMessage = "At least one category is required")]
        [Display(Name = "Categories")]
        public List<CategoryViewModel> Categories { get; set; }

        public BecomePerformerViewModel()
        {
            Categories = new List<CategoryViewModel>();
        }
    }

    //public class AddPhoneNumberViewModel
    //{
    //    [Required]
    //    [Phone]
    //    [Display(Name = "Phone Number")]
    //    public string Number { get; set; }
    //}
}