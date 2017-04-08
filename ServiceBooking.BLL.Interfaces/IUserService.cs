using System;
using System.Collections.Generic;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNet.Identity;
using ServiceBooking.BLL.DTO;
using ServiceBooking.BLL.Infrastructure;

namespace ServiceBooking.BLL.Interfaces
{
    public interface IUserService
    {
        Task<OperationDetails> Create(ClientViewModelBLL userDto);
        Task<ClaimsIdentity> Authenticate(ClientViewModelBLL userDto);
        Task<IdentityResult> ChangePassword(ClientViewModelBLL userDto);
        ClientViewModelBLL FindById(int id);
        IEnumerable<ClientViewModelBLL> GetAll();
    }
}