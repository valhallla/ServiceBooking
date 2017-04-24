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
        OperationDetails Update(ClientViewModelBLL userDto);
        OperationDetails Update(ClientViewModelBLL userDto, int[] selectedCategories);
        Task<OperationDetails> DeleteAccount(ClientViewModelBLL userDto);

        ClientViewModelBLL FindById(int id);
        ClientViewModelBLL FindByUserName(string name);
        IEnumerable<ClientViewModelBLL> GetAll();

        Task<ClaimsIdentity> Authenticate(ClientViewModelBLL userDto);
        Task<IdentityResult> ChangePassword(ClientViewModelBLL userDto);

        OperationDetails ConfirmPerformer(int id);
        OperationDetails RejectPerformer(int id);
    }
}