using ServiceBooking.DAL.Entities;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using Microsoft.Owin;
using ServiceBooking.DAL.EF;

namespace ServiceBooking.DAL.Identity
{
    public class ApplicationUserRepository : UserManager<ApplicationUser, int>
    {
        public ApplicationUserRepository(IUserStore<ApplicationUser, int> store)
                : base(store)
        {
        }

        public static ApplicationUserRepository Create(
        IdentityFactoryOptions<ApplicationUserRepository> options, IOwinContext context)
        {
            var manager = new ApplicationUserRepository(
                new CustomUserStore(context.Get<ApplicationContext>()));
            
            manager.UserValidator = new UserValidator<ApplicationUser, int>(manager)
            {
                AllowOnlyAlphanumericUserNames = false,
                RequireUniqueEmail = true
            };
            
            manager.PasswordValidator = new PasswordValidator
            {
                RequiredLength = 6,
                RequireNonLetterOrDigit = true,
                RequireDigit = true,
                RequireLowercase = true,
                RequireUppercase = true,
            };
            return manager;
        }
    }
}
