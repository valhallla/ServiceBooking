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
        [Required]
        [DataType(DataType.Password)]
        [Display(Name = "Current password")]
        public string OldPassword { get; set; }

        [Required]
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

    public class SetPasswordViewModel
    {
        [Required]
        [StringLength(100, ErrorMessage = "The {0} must be at least {2} characters long.", MinimumLength = 6)]
        [DataType(DataType.Password)]
        [Display(Name = "New password")]
        public string NewPassword { get; set; }

        [DataType(DataType.Password)]
        [Display(Name = "Confirm new password")]
        [Compare("NewPassword", ErrorMessage = "The new password and confirmation password do not match.")]
        public string ConfirmPassword { get; set; }
    }

    public class BecomePerformerViewModel
    {
        [Display(Name = "Company (optional)")]
        public string Company { get; set; }

        [Required]
        [Display(Name = "Activity information")]
        [DataType(DataType.MultilineText)]
        public string Info { get; set; }

        [Required]
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