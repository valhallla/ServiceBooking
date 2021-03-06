﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Security.Claims;
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

        public string Company { get; set; }

        public virtual ICollection<Category> Categories { get; set; }

        [Required]
        public DateTime RegistrationDate { get; set; }

        public int? PictureId { get; set; }
        public Picture Picture { get; set; }

        public string Info { get; set; }

        public int? Rating { get; set; }

        public bool AdminStatus { get; set; }

        public virtual ICollection<Comment> Comments { get; set; }

        public async Task<ClaimsIdentity> GenerateUserIdentityAsync(UserManager<ApplicationUser, int> manager)
        {
            var userIdentity = await manager.CreateIdentityAsync(
                this, DefaultAuthenticationTypes.ApplicationCookie);
            return userIdentity;
        }
    }
}
