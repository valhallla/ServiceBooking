using System;
using System.Collections.Generic;
using System.Security.Claims;
using System.Threading.Tasks;
using ServiceBooking.BLL.DTO;
using ServiceBooking.BLL.Infrastructure;

namespace ServiceBooking.BLL.Interfaces
{
    public interface IUserService : IDisposable
    {
        Task<OperationDetails> Create(UserViewModel userDto);
        Task<ClaimsIdentity> Authenticate(UserViewModel userDto);
        Task SetInitialData(UserViewModel adminDto, List<string> roles);
    }
}