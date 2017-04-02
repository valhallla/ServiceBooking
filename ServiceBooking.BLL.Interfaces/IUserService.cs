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
        Task<OperationDetails> Create(ClientViewModel userDto);
        Task<ClaimsIdentity> Authenticate(ClientViewModel userDto);
        Task<IdentityResult> ChangePassword(ClientViewModel userDto);
        ClientViewModel FindById(int id);
    }
}