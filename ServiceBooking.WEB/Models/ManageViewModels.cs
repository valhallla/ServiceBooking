using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using Microsoft.AspNet.Identity;
using Microsoft.Owin.Security;

namespace ServiceBooking.WEB.Models
{
    public class IndexManageViewModel
    {
        [Required(ErrorMessage = "Current password is required")]
        [DataType(DataType.Password)]
        [Display(Name = "Current password")]
        public string OldPassword { get; set; }

        [Required(ErrorMessage = "New password is required")]
        [StringLength(100, ErrorMessage = "The {0} must be at least {2} characters long.", MinimumLength = 6)]
        [DataType(DataType.Password)]
        [Display(Name = "New password")]
        public string NewPassword { get; set; }

        [DataType(DataType.Password)]
        [Display(Name = "Confirm new password")]
        [Compare("NewPassword", ErrorMessage = "The new password and confirmation password do not match.")]
        public string ConfirmPassword { get; set; }

        public bool IsPerformer { get; set; }

        public bool AdminStatus { get; set; }
    }

    public class BecomePerformerViewModel
    {
        [Display(Name = "Company (optional)")]
        public string Company { get; set; }

        [Required(ErrorMessage = "Phone number is required")]
        [Display(Name = "Phone number")]
        [DataType(DataType.PhoneNumber)]
        public string PhoneNumber { get; set; }

        [Required(ErrorMessage = "Activity information is required")]
        [Display(Name = "Activity information")]
        [DataType(DataType.MultilineText)]
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