using System.Collections.Generic;
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
using ServiceBooking.DAL.Repositories;

namespace ServiceBooking.BLL.Services
{
    public class UserService : IUserService
    {
        private readonly ApplicationContext _db;
        private readonly IRepository<ClientUser> _client;
        public ApplicationUserManager UserManager { get; }
        public ApplicationRoleManager RoleManager { get; }

        [Inject]
        public UserService(IRepository<ClientUser> client)
        {
            _client = client;
            _db = new ApplicationContext("DefaultConnection");
            UserManager = new ApplicationUserManager(new UserStore<ApplicationUser>(_db));
            RoleManager = new ApplicationRoleManager(new RoleStore<ApplicationRole>(_db));
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
                    return new OperationDetails(false, result.Errors.FirstOrDefault(), "");
                // добавляем роль
                await UserManager.AddToRoleAsync(user.Id, userDto.Role);
                // создаем профиль клиента

                ClientUser clientUser = new ClientUser
                {
                    ApplicationUserId = user.Id,
                    //ApplicationUser = new ApplicationUser()
                    //{
                    //    Name = userDto.Name,
                    //    Surname = userDto.Surname,
                    //},
                    IsPerformer = userDto.IsPerformer,
                    CategoryId = userDto.CategoryId,
                    Info = userDto.Info,
                    Rating = userDto.Rating,
                    AdminStatus = userDto.AdminStatus,
                    //Orders = userDto.Orders,
                    //Comments = userDto.Comments
                };

                _client.Create(clientUser);
                //Database.Save(); 
                //await Database.SaveAsync();
                return new OperationDetails(true, "Registration succeeded", "");
            }

            return new OperationDetails(false, "Login is already taken by another user", "Email");
        }

        /*
         * public async Task<OperationDetails> Delete(UserViewModel model)
        {
            ApplicationUser user = await Database.UserManager.FindByIdAsync(model.Id);
            if (user != null)
            {
                ClientUser clientUser = new ClientUser
                {
                    Id = user.Id,
                    Name = model.Name,
                    IsPerformer = model.IsPerformer,
                    CategoryId = model.CategoryId,
                    Info = model.Info,
                    Rating = model.Rating,
                    AdminStatus = model.AdminStatus,
                    Orders = model.Orders,
                    Comments = model.Comments
                };
                Database.ClientManager.Delete(clientUser);
                Database.Save();//await Database.SaveAsync()
                return new OperationDetails(true, "Deleting succeeded", "");
            }

            return new OperationDetails(false, "User not found", "Password");
        }
         */

        public async Task<ClaimsIdentity> Authenticate(ClientViewModel userDto)
        {
            ClaimsIdentity claim = null;
            ApplicationUser user = await UserManager.FindAsync(userDto.Email, userDto.Password);

            if (user != null)
            {
                claim = await UserManager.CreateIdentityAsync(user,
                    DefaultAuthenticationTypes.ApplicationCookie);
            }
            return claim;
        }

        public async Task SetInitialData(ClientViewModel adminViewModel, List<string> roles)
        {
            foreach (string roleName in roles)
            {
                var role = await RoleManager.FindByNameAsync(roleName);
                if (role == null)
                {
                    role = new ApplicationRole { Name = roleName };
                    await RoleManager.CreateAsync(role);
                }
            }
            await Create(adminViewModel);
        }

        public void Dispose()
        {
            _db.Dispose();
        }
    }
}
