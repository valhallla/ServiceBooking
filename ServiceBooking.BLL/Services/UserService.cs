using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ServiceBooking.BLL.DTO;
using ServiceBooking.BLL.Infrastructure;
using Microsoft.AspNet.Identity;
using System.Security.Claims;
using System.Web;
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
       // private readonly IManyToManyResolver _manyToManyResolver

        public ApplicationUserRepository UserManager { get; }
        public ApplicationRoleRepository RoleManager { get; }

        [Inject]
        public UserService(IRepository<ApplicationUser> clientRepository)
        {
            _clientRepository = clientRepository;
            ApplicationContext db = new ApplicationContext("DefaultConnection");
            UserManager = new ApplicationUserRepository(new UserStore<ApplicationUser,
                CustomRole, int, CustomUserLogin, CustomUserRole, CustomUserClaim>(db));
            RoleManager = new ApplicationRoleRepository(new RoleStore<ApplicationRole>(db));

            var currentUser = _clientRepository.Get(HttpContext.Current.User.Identity.GetUserId<int>());
            if (currentUser != null)
            {
                HttpContext.Current.Session["isPerformer"] = currentUser.IsPerformer;
                HttpContext.Current.Session["adminStatus"] = currentUser.AdminStatus;
            }
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
                return new OperationDetails(true, "Registration succeeded", string.Empty);
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

            if (user != null)
            {
                claim = await UserManager.CreateIdentityAsync(user,
                    DefaultAuthenticationTypes.ApplicationCookie);
                HttpContext.Current.Session["isPerformer"] = user.IsPerformer;
                HttpContext.Current.Session["adminStatus"] = user.AdminStatus;
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

        public IEnumerable<ClientViewModelBLL> GetAll()
        {
            var users = _clientRepository.GetAll().ToList();
            Mapper.Initialize(cfg => cfg.CreateMap<ApplicationUser, ClientViewModelBLL>());
            return Mapper.Map<List<ApplicationUser>, List<ClientViewModelBLL>>(users);
        }

        public OperationDetails Update(ClientViewModelBLL userDto)
        {
            ApplicationUser user = _clientRepository.Get(userDto.Id);
            if (user != null)
            {
                Mapper.Initialize(cfg => cfg.CreateMap<ClientViewModelBLL, ApplicationUser>()
                    .IgnoreAllSourcePropertiesWithAnInaccessibleSetter()
                    .IgnoreAllPropertiesWithAnInaccessibleSetter());
                Mapper.Map(userDto, user);
                _clientRepository.Update(user);

                HttpContext.Current.Session["adminStatus"] = userDto.AdminStatus;
                HttpContext.Current.Session["isPerformer"] = userDto.IsPerformer;

                return new OperationDetails(true, @"User information updated", string.Empty);
            }
            return new OperationDetails(false, @"User doesn't exist", "Id");
        }

        public OperationDetails Update(ClientViewModelBLL userDto, int[] selectedCategories)
        {
            (_clientRepository as IManyToManyResolver)?.Update(userDto.Id, selectedCategories);
            return Update(userDto);
        }

        public async Task<OperationDetails> DeleteAccount(ClientViewModelBLL userDto)
        {
            var user = await UserManager.FindAsync(userDto.Email, userDto.Password);
            if (user != null)
            {
                _clientRepository.Delete(user.Id);
                return new OperationDetails(true, "Deleting account succeeded", string.Empty);
            }
            return new OperationDetails(false, "Incorrect password", "Password");
        }
    }
}