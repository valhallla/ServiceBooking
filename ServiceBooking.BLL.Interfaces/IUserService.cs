using System;
using System.Collections.Generic;
using System.Security.Claims;
using System.Threading.Tasks;
using ServiceBooking.BLL.DTO;
using ServiceBooking.BLL.Infrastructure;
using ServiceBooking.BLL;

namespace ServiceBooking.BLL.Interfaces
{
    public interface IUserService : IDisposable
    {
        Task<OperationDetails> Create(ClientViewModel userDto);
        Task<ClaimsIdentity> Authenticate(ClientViewModel userDto);
        Task SetInitialData(ClientViewModel adminDto, List<string> roles);
    }
}