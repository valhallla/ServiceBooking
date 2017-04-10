using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using AuthFilterApp.Filters;
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
        private static IUserService _userService;
        private static ICategoryService _categoryService;
        private static IUnitOfWork _unitOfWork;

        public ManageController() : this(_userService, _categoryService, _unitOfWork) { }

        public ManageController(IUserService service, ICategoryService 
            categoryService, IUnitOfWork unitOfWork)
        {
            _userService = service;
            _categoryService = categoryService;
            _unitOfWork = unitOfWork;
        }

        //
        // GET: /Manage/Index
        [Authorize(Roles = "user")]
        [AdminAccessDenied]
        public ActionResult Index()
        {
            //Session["adminStatus"] = Global.AdminStatus;
            //Session["isPerformer"] = Global.IsPerformer;
            return View("Index");
        }

        //
        // POST: /Manage/ChangePassword
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "user")]
        [AdminAccessDenied]
        public async Task<ActionResult> ChangePassword(IndexManageViewModel model)
        {
            if (!ModelState.IsValid)
                return View(model);

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

        [Authorize(Roles = "user")]
        [AdminAccessDenied]
        public ActionResult BecomePerformer()
        {
            var categoriesDto = _categoryService.GetAll().ToList();
            Mapper.Initialize(cfg => cfg.CreateMap<CategoryViewModelBLL, CategoryViewModel>());
            var categories = Mapper.Map<List<CategoryViewModelBLL>, List<CategoryViewModel>>(categoriesDto);
            ViewBag.Categories = categories;
            return View("BecomePerformer", new BecomePerformerViewModel());
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "user")]
        [AdminAccessDenied]
        public ActionResult BecomePerformer(BecomePerformerViewModel model, int[] selectedCategories)
        {
            if (!ModelState.IsValid)
                return View(model);

            ClientViewModelBLL client = _userService.FindById(User.Identity.GetUserId<int>());
            Mapper.Initialize(cfg => cfg.CreateMap<BecomePerformerViewModel, ClientViewModelBLL>()
                .ForMember("RegistrationDate", opt => opt.MapFrom(c => DateTime.Today))
                .ForMember("IsPerformer", opt => opt.MapFrom(c => true))
                .ForMember("AdminStatus", opt => opt.MapFrom(c => false))
                .ForMember("Rating", opt => opt.MapFrom(c => 0))
                );
            Mapper.Map(model, client);
            _userService.Update(client, selectedCategories);

            return RedirectToAction("Index");
            
        }

        [Authorize(Roles = "user")]
        [AdminAccessDenied]
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