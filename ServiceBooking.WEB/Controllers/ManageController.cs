using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using Microsoft.Owin.Security;
using ServiceBooking.BLL.DTO;
using ServiceBooking.BLL.Interfaces;
using ServiceBooking.DAL.Interfaces;
using ServiceBooking.WEB.Models;
using AutoMapper;
using ServiceBooking.BLL.Infrastructure;

namespace ServiceBooking.WEB.Controllers
{
    [Authorize]
    public class ManageController : Controller
    {
        private  static IUserService _userService;
        private static IUnitOfWork _unitOfWork;

        public ManageController() : this(_userService, _unitOfWork) { }

        public ManageController(IUserService service, IUnitOfWork unitOfWork)
        {
            _userService = service;
            _unitOfWork = unitOfWork;
        }

        //
        // GET: /Manage/Index
        public ActionResult Index()
        {
            Session["adminStatus"] = Global.AdminStatus;
            Session["isPerformer"] = Global.IsPerformer;
            return View("Index");
        }

        //
        // POST: /Manage/ChangePassword
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> ChangePassword(IndexManageViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            Mapper.Initialize(cfg => cfg.CreateMap<IndexManageViewModel, ClientViewModelBLL>()
                .ForMember("Id", opt => opt.MapFrom(c => User.Identity.GetUserId<int>()))
                .ForMember("UserName", opt => opt.MapFrom(c => c.OldPassword))
                .ForMember("Password", opt => opt.MapFrom(c => c.NewPassword))
                );
            ClientViewModelBLL client = Mapper.Map<IndexManageViewModel, ClientViewModelBLL>(model);

            var result = await _userService.ChangePassword(client);
            
            if (result.Succeeded)
            {
                return RedirectToAction("Index", "Home", new { Message = ManageMessageId.ChangePasswordSuccess });
            }
            AddErrors(result);
            return View(model);
        }

        public ActionResult BecomePerformer()
        {
            return View("BecomePerformer");
        }

        public ActionResult Close()
        {
            var userDto = _userService.FindById(User.Identity.GetUserId<int>());
            userDto.AdminStatus = false;
            _userService.Update(userDto);
            return RedirectToAction("Index");
        }



        #region Helpers
        private IAuthenticationManager AuthenticationManager
        {
            get
            {
                return HttpContext.GetOwinContext().Authentication;
            }
        }

        private void AddErrors(IdentityResult result)
        {
            foreach (var error in result.Errors)
            {
                ModelState.AddModelError("", error);
            }
        }

        public enum ManageMessageId
        {
            AddPhoneSuccess,
            ChangePasswordSuccess,
            SetTwoFactorSuccess,
            SetPasswordSuccess,
            RemoveLoginSuccess,
            RemovePhoneSuccess,
            Error
        }
        #endregion
    }
}