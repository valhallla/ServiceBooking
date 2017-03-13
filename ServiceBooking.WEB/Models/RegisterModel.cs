using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace ServiceBooking.WEB.Models
{
    public class RegisterModel
    {
        [Required]
        [RegularExpression(@"^[a-zA-Zа-яА-ЯёЁ]{1,20}\s[a-zA-Zа-яА-ЯёЁ]{1,20}$", ErrorMessage = "Incorrect name")]
        [Display(Name = "Name")]
        public string Name { get; set; }

        [Display(Name = "Info")]
        public string Info { get; set; }

        [Display(Name = "Rating")]
        public string Rating { get; set; }

        [Required]
        [EmailAddress]
        [Display(Name = "Email")]
        public string Email { get; set; }

        [Required]
        [StringLength(100, ErrorMessage = "The {0} must be at least {2} characters long.", MinimumLength = 6)]
        [DataType(DataType.Password)]
        [Display(Name = "Password")]
        public string Password { get; set; }

        [DataType(DataType.Password)]
        [Display(Name = "Confirm password")]
        [Compare("Password", ErrorMessage = "The password and confirmation password do not match.")]
        public string ConfirmPassword { get; set; }
    }
}