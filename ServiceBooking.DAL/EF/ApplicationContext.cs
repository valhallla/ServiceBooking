using System.Data.Entity;
using Microsoft.AspNet.Identity.EntityFramework;
using ServiceBooking.DAL.Entities;
using ServiceBooking.DAL.Identity;
using ServiceBooking.DAL.Migrations;

namespace ServiceBooking.DAL.EF
{
    public class ApplicationContext : IdentityDbContext<ApplicationUser, CustomRole, int, CustomUserLogin, CustomUserRole, CustomUserClaim>
    {
        //static ApplicationContext()
        //{
        //    Database.SetInitializer(new DropCreateDatabaseIfModelChanges<ApplicationContext>());
        //}

        //public void FixEfProviderServicesProblem()
        //{
        //    //The Entity Framework provider type 'System.Data.Entity.SqlServer.SqlProviderServices, EntityFramework.SqlServer'
        //    //for the 'System.Data.SqlClient' ADO.NET provider could not be loaded.
        //    //Make sure the provider assembly is available to the running application.
        //    //See http://go.microsoft.com/fwlink/?LinkId=260882 for more information.

        //    var instance = System.Data.Entity.SqlServer.SqlProviderServices.Instance;
        //}

        public ApplicationContext() { }

        public ApplicationContext(string conectionString) : base(conectionString) { }

        public DbSet<ClientUser> Clients { get; set; }
        public DbSet<Order> Orders { get; set; }
        public DbSet<Comment> Comments { get; set; }
        public DbSet<Response> Responses { get; set; }
        public DbSet<Category> Categories { get; set; }
        public DbSet<Status> Status { get; set; }
    }
}
