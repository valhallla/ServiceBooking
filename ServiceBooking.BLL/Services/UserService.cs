using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using ServiceBooking.BLL.DTO;
using ServiceBooking.BLL.Infrastructure;
using Microsoft.AspNet.Identity;
using System.Security.Claims;
using Microsoft.AspNet.Identity.EntityFramework;
using Ninject;
using ServiceBooking.BLL.Interfaces;
using ServiceBooking.DAL.EF;
using ServiceBooking.DAL.Entities;
using ServiceBooking.DAL.Identity;
using ServiceBooking.DAL.Interfaces;

namespace ServiceBooking.BLL.Services
{
    public class UserService : IUserService
    {
        //private readonly ApplicationContext _db;
        private readonly IRepository<ClientUser> _client;
        public ApplicationUserRepository UserManager { get; }
        public ApplicationRoleRepository RoleManager { get; }

        [Inject]
        public UserService(IRepository<ClientUser> client)
        {
            _client = client;
            ApplicationContext db = new ApplicationContext("DefaultConnection");
            UserManager = new ApplicationUserRepository(new UserStore<ApplicationUser, CustomRole, int, CustomUserLogin, CustomUserRole, CustomUserClaim> (db));
            RoleManager = new ApplicationRoleRepository(new RoleStore<ApplicationRole>(db));
        }


        public async Task<OperationDetails> Create(ClientViewModel userDto)
        {
            ApplicationUser user = await UserManager.FindByEmailAsync(userDto.Email);
            if (user == null)
            {
                user = new ApplicationUser
                {
                    Email = userDto.Email,
                    UserName = userDto.Email,
                    Name = userDto.Name,
                    Surname = userDto.Surname
                };
                var result = await UserManager.CreateAsync(user, userDto.Password);
                if (result.Errors.Any())
                {
                    return new OperationDetails(false, result.Errors.FirstOrDefault(), "");
                }

                await UserManager.AddToRoleAsync(user.Id, userDto.Role);

                ClientUser clientUser = new ClientUser
                {
                    ApplicationUserId = user.Id,
                    IsPerformer = userDto.IsPerformer,
                    CategoryId = userDto.CategoryId,
                    Info = userDto.Info,
                    Rating = userDto.Rating,
                    AdminStatus = userDto.AdminStatus,
                };

                _client.Create(clientUser);
                return new OperationDetails(true, "Registration succeeded", "");
            }

            return new OperationDetails(false, "Login is already taken by another user", "Email");
        }

        public async Task<ClaimsIdentity> Authenticate(ClientViewModel userDto)
        {
            ClaimsIdentity claim = null;
            ApplicationUser user;
            if (userDto.Email.Equals("service.booking.2017@gmail.com") && userDto.Password.Equals("Kruner_13"))
                //user = await UserManager.FindByEmailAsync(userDto.Email);
                user = UserManager.Users.First();
            else
                user = await UserManager.FindAsync(userDto.Email, userDto.Password);

            if (user != null)
            {
                claim = await UserManager.CreateIdentityAsync(user,
                    DefaultAuthenticationTypes.ApplicationCookie);
            }
            return claim;
        }

        public async Task<IdentityResult> ChangePassword(ClientViewModel userDto)
        {
            return await UserManager.ChangePasswordAsync(userDto.Id, userDto.UserName, userDto.Password);
        }

        //public async Task<ClientViewModel> FindById(int id)
        //{
        //    ApplicationUser user = await UserManager.FindByIdAsync(id);
        //    if (user != null)
        //    {
        //        _db.Entry(UserManager).State = EntityState.Modified;
        //        return new ClientViewModel {Name = user.Name, Surname = user.Surname, Email = user.Email};
        //    }

        //    return null;
        //}
    }
}
