using System.Collections.Generic;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNet.Identity;
using ServiceBooking.BLL.DTO;
using ServiceBooking.BLL.Infrastructure;
using ServiceBooking.DAL.Identity;

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
        Task<string> GenerateEmailConfirmationToken(int userId);
        void SendEmail(int userId, string callbackUrl);
        Task<IdentityResult> ConfirmEmail(int userId, string code);

        OperationDetails ConfirmPerformer(int id);
        OperationDetails RejectPerformer(int id);
    }
}