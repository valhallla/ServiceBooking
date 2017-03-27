using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using ServiceBooking.DAL.EF;
using ServiceBooking.DAL.Entities;
using ServiceBooking.DAL.Identity;

namespace ServiceBooking.DAL.Migrations
{
    using System;
    using System.Data.Entity;
    using System.Data.Entity.Migrations;
    using System.Linq;

    internal sealed class Configuration : DbMigrationsConfiguration<ServiceBooking.DAL.EF.ApplicationContext>
    {
        public Configuration()
        {
            AutomaticMigrationsEnabled = false;
        }

        protected override void Seed(ServiceBooking.DAL.EF.ApplicationContext context)
        {
            //var manager = new UserManager<ApplicationUser, int>(
            //    new UserStore<ApplicationUser, CustomRole, int,
            //        CustomUserLogin, CustomUserRole, CustomUserClaim>(
            //        new ApplicationContext()));

            //// Create 4 test users:

            //var user = new ApplicationUser()
            //{
            //    UserName = "service.booking.2017@gmail.com",
            //    Email = "service.booking.2017@gmail.com",
            //    EmailConfirmed = true,
            //    Name = "Veron",
            //    Surname = "Navros",
            //    IsPasswordClear = true
            //};
            //manager.Create(user, "Kruner_13");

            //  This method will be called after migrating to the latest version.

            //  You can use the DbSet<T>.AddOrUpdate() helper extension method 
            //  to avoid creating duplicate seed data. E.g.
            //
            //    context.People.AddOrUpdate(
            //      p => p.FullName,
            //      new Person { FullName = "Andrew Peters" },
            //      new Person { FullName = "Brice Lambson" },
            //      new Person { FullName = "Rowan Miller" }
            //    );
            //
        }
    }
}
