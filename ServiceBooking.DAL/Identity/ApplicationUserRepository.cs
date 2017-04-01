using System;
using System.Collections.Generic;
using System.Data.Entity.SqlServer.Utilities;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
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

        //private IUserPasswordStore<ApplicationUser, int> GetPasswordStore()
        //{
        //    var cast = Store as IUserPasswordStore<ApplicationUser, int>;
        //    if (cast == null)
        //    {
        //        throw new NotSupportedException();
        //    }
        //    return cast;
        //}

        //public override async Task<bool> CheckPasswordAsync(ApplicationUser user, string password)
        //{
        //    IUserPasswordStore<ApplicationUser, int> passwordStore = this.GetPasswordStore();
        //    return await this.VerifyPasswordAsync(passwordStore, user, password).WithCurrentCulture<bool>();
        //}

        //protected virtual async Task<bool> VerifyPasswordAsync(IUserPasswordStore<ApplicationUser, int> store, ApplicationUser user, string password)
        //{
        //    string hash = await store.GetPasswordHashAsync(user).WithCurrentCulture<string>();
        //    return this.PasswordHasher.VerifyHashedPassword(hash, password) != PasswordVerificationResult.Failed;
        //}
    }
}
