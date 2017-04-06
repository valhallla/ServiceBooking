﻿using System.Linq;
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

namespace ServiceBooking.BLL.Services
{
    public class UserService : IUserService
    {
        private readonly IRepository<ApplicationUser> _clientRepository;
        public ApplicationUserRepository UserManager { get; }
        public ApplicationRoleRepository RoleManager { get; }

        [Inject]
        public UserService(IRepository<ApplicationUser> client)
        {
            _clientRepository = client;
            ApplicationContext db = new ApplicationContext("DefaultConnection");
            UserManager = new ApplicationUserRepository(new UserStore<ApplicationUser, 
                CustomRole, int, CustomUserLogin, CustomUserRole, CustomUserClaim> (db));
            RoleManager = new ApplicationRoleRepository(new RoleStore<ApplicationRole>(db));
        }

        public async Task<OperationDetails> Create(ClientViewModelBLL userDto)
        {
            ApplicationUser user = await UserManager.FindByEmailAsync(userDto.Email);
            if (user == null)
            {
                Mapper.Initialize(cfg => cfg.CreateMap<ClientViewModelBLL, ApplicationUser>());
                user = Mapper.Map<ClientViewModelBLL, ApplicationUser>(userDto);

                var result = await UserManager.CreateAsync(user, userDto.Password);
                if (result.Errors.Any())
                    return new OperationDetails(false, result.Errors.FirstOrDefault(), "");

                await UserManager.AddToRoleAsync(user.Id, userDto.Role);

                //Mapper.Initialize(cfg => cfg.CreateMap<ClientViewModel, ClientUser>()
                //    .ForMember("ApplicationUserId", opt => opt.MapFrom(c => user.Id)));
                //ClientUser clientUser = Mapper.Map<ClientViewModel, ClientUser>(userDto);

                //_clientRepository.Create(clientUser);
                return new OperationDetails(true, "Registration succeeded", "");
            }

            return new OperationDetails(false, "Login is already taken by another user", "Email");
        }

        public async Task<ClaimsIdentity> Authenticate(ClientViewModelBLL userDto)
        {
            ClaimsIdentity claim = null;
            ApplicationUser user = await UserManager.FindByEmailAsync(userDto.Email);

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

        public async Task<IdentityResult> ChangePassword(ClientViewModelBLL userDto)
        {
            return await UserManager.ChangePasswordAsync(userDto.Id, userDto.UserName, userDto.Password);
        }

        public ClientViewModelBLL FindById(int id)
        {
            ApplicationUser user = UserManager.FindById(id);
            if (user != null)
            {
                Mapper.Initialize(cfg => cfg.CreateMap<ApplicationUser, ClientViewModelBLL>());
                return Mapper.Map<ApplicationUser, ClientViewModelBLL>(user);
            }
            return null;
        }
    }
}
