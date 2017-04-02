using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using ServiceBooking.DAL.Identity;

namespace ServiceBooking.DAL.Entities
{
    public class ApplicationUser : IdentityUser<int, CustomUserLogin, CustomUserRole, CustomUserClaim>
    {
        public string Name { get; set; }

        public string Surname { get; set; }

        public bool IsPasswordClear { get; set; }

        public virtual ICollection<Order> Orders { get; set; }

        public bool IsPerformer { get; set; }

        public int? CategoryId { get; set; }
        public Category Category { get; set; }

        public string Info { get; set; }

        public int? Rating { get; set; }

        public string AdminStatus { get; set; }

        public virtual ICollection<Comment> Comments { get; set; }

        public async Task<ClaimsIdentity> GenerateUserIdentityAsync(UserManager<ApplicationUser, int> manager)
        {
            // Note the authenticationType must match the one defined in
            // CookieAuthenticationOptions.AuthenticationType 
            var userIdentity = await manager.CreateIdentityAsync(
                this, DefaultAuthenticationTypes.ApplicationCookie);
            // Add custom user claims here 
            return userIdentity;
        }
    }
}
