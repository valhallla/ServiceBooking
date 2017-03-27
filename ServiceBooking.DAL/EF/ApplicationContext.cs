using System.Data.Entity;
using Microsoft.AspNet.Identity.EntityFramework;
using ServiceBooking.DAL.Entities;
using ServiceBooking.DAL.Identity;
using ServiceBooking.DAL.Migrations;

namespace ServiceBooking.DAL.EF
{
    public class ApplicationContext : IdentityDbContext<ApplicationUser, CustomRole, int, CustomUserLogin, CustomUserRole, CustomUserClaim>
    {
        static ApplicationContext()
        {
            Database.SetInitializer<ApplicationContext>(new AppDbInitializer());
            Database.SetInitializer(new MigrateDatabaseToLatestVersion<ApplicationContext, Configuration>());
        }

        public void FixEfProviderServicesProblem()
        {
            //The Entity Framework provider type 'System.Data.Entity.SqlServer.SqlProviderServices, EntityFramework.SqlServer'
            //for the 'System.Data.SqlClient' ADO.NET provider could not be loaded.
            //Make sure the provider assembly is available to the running application.
            //See http://go.microsoft.com/fwlink/?LinkId=260882 for more information.

            var instance = System.Data.Entity.SqlServer.SqlProviderServices.Instance;
        }

        public ApplicationContext() { }

        public ApplicationContext(string conectionString) : base(conectionString) { }

        public DbSet<ClientUser> Clients { get; set; }
        public DbSet<Order> Orders { get; set; }
        public DbSet<Comment> Comments { get; set; }
        public DbSet<Response> Responses { get; set; }
        public DbSet<Category> Categories { get; set; }
        public DbSet<Status> Status { get; set; }
    }

    public class AppDbInitializer : DropCreateDatabaseAlways<ApplicationContext>
    {
        protected override void Seed(ApplicationContext db)
        {
            //var userManager = new ApplicationUserRepository(new CustomUserStore<ApplicationUser>(db));

            //var roleManager = new RoleManager<IdentityRole>(new RoleStore<IdentityRole>(db));

            //// создаем две роли
            //var role1 = new IdentityRole { Name = "admin" };
            //var role2 = new IdentityRole { Name = "user" };

            //// добавляем роли в бд
            //roleManager.Create(role1);
            //roleManager.Create(role2);

            //// создаем пользователей
            //var admin = new ApplicationUser { Name = "Veron", Surname = "Navros", Email = "kruner.kruner@gmail.com", UserName = "kruner.kruner@gmail.com", EmailConfirmed = true };
            //string password = "Kruner_13";
            //var result = userManager.Create(admin, password);

            //// если создание пользователя прошло успешно
            //if (result.Succeeded)
            //{
            //    // добавляем для пользователя роль
            //    userManager.AddToRole(admin.Id, role1.Name);
            //    userManager.AddToRole(admin.Id, role2.Name);
            //}

           // ----------------------------------------------------------------------------------------------------------------------------------------

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
