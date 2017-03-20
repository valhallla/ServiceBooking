using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.Entity;
using System.IO.Pipes;
using System.Security.Cryptography;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using ServiceBooking.DAL.Entities;
using ServiceBooking.DAL.Identity;

namespace ServiceBooking.DAL.EF
{
    public class ApplicationContext : IdentityDbContext<ApplicationUser>
    {
        static ApplicationContext()
        {
            Database.SetInitializer<ApplicationContext>(new AppDbInitializer());
        }

        public ApplicationContext(string conectionString) : base(conectionString) { }

        public DbSet<ClientUser> Clients { get; set; }
        public DbSet<Order> Orders { get; set; }
        public DbSet<Comment> Comments { get; set; }
        public DbSet<Response> Responses { get; set; }
        public DbSet<Category> Categories { get; set; }
        public DbSet<Status> Status { get; set; }
    }

    public class AppDbInitializer : DropCreateDatabaseIfModelChanges<ApplicationContext>
    {
        protected override void Seed(ApplicationContext db)
        {
            //var userManager = new ApplicationUserManager(new UserStore<ApplicationUser>(db));
            //var roleManager = new RoleManager<IdentityRole>(new RoleStore<IdentityRole>(db));

            //var role1 = new IdentityRole { Name = "admin" };
            //var role2 = new IdentityRole { Name = "user" };

            //roleManager.Create(role1);
            //roleManager.Create(role2);

            //var admin = new ApplicationUser
            //{
            //    Name = "Veron",
            //    Surname = "Navros",
            //    Email = "kruner.kruner@gmail.com",
            //    UserName = "kruner.kruner@gmail.com",
            //    EmailConfirmed = true
            //};
            //string password = "Kruner_13";
            //var result = userManager.Create(admin, password);

            //// если создание пользователя прошло успешно
            //if (result.Succeeded)
            //{
            //    // добавляем для пользователя роль
            //    userManager.AddToRole(admin.Id, role1.Name);
            //}

            //var user = new ApplicationUser
            //{
            //    Name = "Veron",
            //    Surname = "Navros",
            //    Email = "veronika.navros@gmail.com",
            //    UserName = "veronika.navros@gmail.com",
            //    EmailConfirmed = true,
            //};
            //password = "Kruner_13";
            //result = userManager.Create(user, password);

            //if (result.Succeeded)
            //{
            //    userManager.AddToRole(user.Id, role2.Name);
            //}

            //db.Clients.Add(new ClientUser
            //{
            //    ApplicationUser = user,
            //    IsPerformer = false
            //});

            db.Categories.Add(new Category
            {
                Id = 1,
                Name = "Ritual services"
            });

            db.Categories.Add(new Category
            {
                Id = 2,
                Name = "Cleaning"
            });

            db.Categories.Add(new Category
            {
                Id = 1,
                Name = "Photo and video services"
            });

            db.Status.Add(new Status
            {
                Id = 1,
                Value = "Active"
            });

            db.Status.Add(new Status
            {
                Id = 1,
                Value = "Confirmed"
            });

            db.Status.Add(new Status
            {
                Id = 1,
                Value = "Completed"
            });

            db.Status.Add(new Status
            {
                Id = 1,
                Value = "Outstanding"
            });

            db.SaveChanges();
        }
    }
}
