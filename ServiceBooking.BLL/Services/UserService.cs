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
using AutoMapper;
using System.Web.Helpers;

namespace ServiceBooking.BLL.Services
{
    public class UserService : IUserService
    {
        private readonly IRepository<ClientUser> _clientRepository;
        public ApplicationUserRepository UserManager { get; }
        public ApplicationRoleRepository RoleManager { get; }

        [Inject]
        public UserService(IRepository<ClientUser> client)
        {
            _clientRepository = client;
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

                _clientRepository.Create(clientUser);
                return new OperationDetails(true, "Registration succeeded", "");
            }

            return new OperationDetails(false, "Login is already taken by another user", "Email");
        }

        public async Task<ClaimsIdentity> Authenticate(ClientViewModel userDto)
        {
            ClaimsIdentity claim = null;
            ApplicationUser user = /*_clientRepository.GetAll().Where(u => u.Email.Equals(userDto.Email)).ToArray().FirstOrDefault();*/await UserManager.FindByEmailAsync(userDto.Email);

            if (user != null)
            {
                if (user.IsPasswordClear && user.PasswordHash.Equals(userDto.Password)) ;
                else if (!user.IsPasswordClear)
                    user = await UserManager.FindAsync(userDto.Email, userDto.Password);
                else user = null;
            }

            //var user = UserManager.Users.ToArray(


            //).FirstOrDefault(u => (u.Email.Equals(userDto.Email)) && 
            //        (
            //            (u.IsPasswordClear && Crypto.VerifyHashedPassword(u.PasswordHash, userDto.Password)) ||
            //            (!u.IsPasswordClear && Crypto.VerifyHashedPassword(u.PasswordHash, Crypto.HashPassword(userDto.Password)))
            //        ));

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

        public async Task<ClientViewModel> FindById(int id)
        {
            ClientUser clientUser = _clientRepository.Get(id);

            if (clientUser != null)
                return new ClientViewModel { Id = clientUser.Id };

            return null;

            //ApplicationUser user = await UserManager.FindByIdAsync(id);
            //if (user != null)
            //{
            //    Db.Entry(UserManager).State = EntityState.Modified;
            //    return new ClientViewModel { Name = user.Name, Surname = user.Surname, Email = user.Email };
            //}

            //return null;
        }

        public async Task<OperationDetails> AddOrder(int clientId, OrderViewModel orderVM)
        {
            ClientUser client = _clientRepository.Get(clientId);
            Mapper.Initialize(cfg => cfg.CreateMap<OrderViewModel, Order>());
            Order order = Mapper.Map<OrderViewModel, Order>(orderVM);
            client.Orders.Add(order);
            _clientRepository.Update(client);
            return new OperationDetails(true, "Order creation succeeded", "");
        }
    }
}
