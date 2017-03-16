﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ServiceBooking.BLL.DTO;
using ServiceBooking.BLL.Infrastructure;
using ServiceBooking.DAL.Entities;
using Microsoft.AspNet.Identity;
using System.Security.Claims;
using Microsoft.AspNet.Identity.EntityFramework;
using ServiceBooking.BLL.Interfaces;
using ServiceBooking.DAL.Interfaces;

namespace ServiceBooking.BLL.Services
{
    public class UserService : /*UserManager<ApplicationUser>, */IUserService
    {
        IUnitOfWork Database { get; }

        //public UserService(IUserStore<ApplicationUser> store, IUnitOfWork uow)
        //    : base(store)
        //{
        //    Database = uow;
        //}

        public UserService(IUnitOfWork uow)
        {
            Database = uow;
        }


        public async Task<OperationDetails> Create(UserViewModel userDto)
        {
            ApplicationUser user = await Database.UserManager.FindByEmailAsync(userDto.Email);
            if (user == null)
            {
                user = new ApplicationUser { Email = userDto.Email, UserName = userDto.Email };
                var result = await Database.UserManager.CreateAsync(user, userDto.Password);
                if (result.Errors.Any())
                    return new OperationDetails(false, result.Errors.FirstOrDefault(), "");
                // добавляем роль
                await Database.UserManager.AddToRoleAsync(user.Id, userDto.Role);
                // создаем профиль клиента
                ClientUser clientProfile = new ClientUser
                {
                    ApplicationUser = new ApplicationUser()
                    {
                        Name = userDto.Name,
                        Surname = userDto.Surname,
                    },
                    IsPerformer = userDto.IsPerformer,
                    CategoryId = userDto.CategoryId,
                    Info = userDto.Info,
                    Rating = userDto.Rating,
                    AdminStatus = userDto.AdminStatus,
                    Orders = userDto.Orders,
                    Comments = userDto.Comments
                };
                Database.ClientManager.Create(clientProfile);
                Database.Save();//await Database.SaveAsync()
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

        public async Task<ClaimsIdentity> Authenticate(UserViewModel userDto)
        {
            ClaimsIdentity claim = null;
            // находим пользователя
            ApplicationUser user = await Database.UserManager.FindAsync(userDto.Email, userDto.Password);
            // авторизуем его и возвращаем объект ClaimsIdentity
            if (user != null)
                claim = await Database.UserManager.CreateIdentityAsync(user,
                                            DefaultAuthenticationTypes.ApplicationCookie);
            return claim;
        }

        // начальная инициализация бд
        public async Task SetInitialData(UserViewModel adminViewModel, List<string> roles)
        {
            foreach (string roleName in roles)
            {
                var role = await Database.RoleManager.FindByNameAsync(roleName);
                if (role == null)
                {
                    role = new ApplicationRole { Name = roleName };
                    await Database.RoleManager.CreateAsync(role);
                }
            }
            await Create(adminViewModel);
        }

        public void Dispose()
        {
            Database.Dispose();
        }
    }
}
