using System.Data.Entity;
using Microsoft.AspNet.Identity.EntityFramework;
using ServiceBooking.DAL.Entities;
using ServiceBooking.DAL.Identity;

namespace ServiceBooking.DAL.EF
{
    public class ApplicationContext : IdentityDbContext<ApplicationUser, 
        CustomRole, int, CustomUserLogin, CustomUserRole, CustomUserClaim>
    {
        public ApplicationContext() { }

        public ApplicationContext(string conectionString) : base(conectionString) { }

        public DbSet<Order> Orders { get; set; }
        public DbSet<Comment> Comments { get; set; }
        public DbSet<Response> Responses { get; set; }
        public DbSet<Category> Categories { get; set; }
        public DbSet<Status> Status { get; set; }
        public DbSet<Picture> Pictures { get; set; }
        public DbSet<ExceptionDetail> ExceptionDetails { get; set; }
    }
}
