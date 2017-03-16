using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.Entity;
using Microsoft.AspNet.Identity.EntityFramework;
using ServiceBooking.DAL.Entities;

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
