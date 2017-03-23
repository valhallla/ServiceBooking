using System;
using System.Collections.Generic;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNet.Identity;
using ServiceBooking.BLL.DTO;
using ServiceBooking.BLL.Infrastructure;

namespace ServiceBooking.BLL.Interfaces
{
    public interface IUserService : IDisposable
    {
        Task<OperationDetails> Create(ClientViewModel userDto);
        Task<ClaimsIdentity> Authenticate(ClientViewModel userDto);
        Task SetInitialData(ClientViewModel adminDto, List<string> roles);
        Task<IdentityResult> ChangePassword(ClientViewModel userDto);
        Task<ClientViewModel> FindById(string id);
    }
}