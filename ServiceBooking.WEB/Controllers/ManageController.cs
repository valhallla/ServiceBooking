using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using Microsoft.AspNet.Identity;
using ServiceBooking.BLL.DTO;
using ServiceBooking.BLL.Interfaces;
using ServiceBooking.DAL.Interfaces;
using ServiceBooking.WEB.Models;
using AutoMapper;
using ServiceBooking.DAL.UnitOfWork.DTO;

namespace ServiceBooking.WEB.Controllers
{
    [Authorize]
    public class ManageController : Controller
    {
        private static IUserService _userService;
        private static ICategoryService _categoryService;
        private static IPictureService _pictureService;
        private static IUnitOfWork _unitOfWork;

        private const string DefaultImageName = @"~/Content/default-user.png";

        public ManageController() : this(_userService, _categoryService, _pictureService, _unitOfWork) { }

        public ManageController(IUserService service, ICategoryService 
            categoryService, IPictureService pictureService, IUnitOfWork unitOfWork)
        {
            _userService = service;
            _categoryService = categoryService;
            _pictureService = pictureService;
            _unitOfWork = unitOfWork;
        }

        [Authorize(Roles = "user")]
        public ActionResult Index()
        {
            return View("Index");
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "user")]
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
                return RedirectToAction("Index", "Home", new { Message = ManageMessageId.ChangePasswordSuccess });

            AddErrors(result);
            return View(model);
        }

        [Authorize(Roles = "user")]
        public ActionResult BecomePerformer(string message = "", string company = "",
            string info = "", string phoneNumber = "")
        {
            if ((bool)Session["isPerformer"] && (bool)Session["adminStatus"])
                return View("~/Views/Error/Forbidden.cshtml");

           GetCategoriesList();
            var model = new BecomePerformerViewModel
            {
                Company = company,
                Info = info,
                PhoneNumber = phoneNumber
            };

            ViewBag.DefaultPath =
                $"data: image/png; base64, {Convert.ToBase64String(System.IO.File.ReadAllBytes(Server.MapPath(DefaultImageName)))}";

            return View("BecomePerformer", model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "user")]
        public ActionResult BecomePerformer(BecomePerformerViewModel model, int[] selectedCategories, HttpPostedFileBase loadImage)
        {
            if ((bool)Session["isPerformer"] && (bool)Session["adminStatus"])
                return View("~/Views/Error/Forbidden.cshtml");

            if (!ModelState.IsValid)
                return View(model);

            if (selectedCategories == null)
            {
                ModelState.AddModelError("", "At least one category is required");
                GetCategoriesList();
                return View(model);
            }

            PictureViewModelBLL picture = null;
            if (!ReferenceEquals(loadImage, null))
            {
                byte[] image;
                using (var binaryReader = new BinaryReader(loadImage.InputStream))
                {
                    image = binaryReader.ReadBytes(loadImage.ContentLength);
                }
                picture = new PictureViewModelBLL { Image = image };

                _pictureService.Create(image);
                _unitOfWork.Save();
            }

            ClientViewModelBLL client = _userService.FindById(User.Identity.GetUserId<int>());
            Mapper.Initialize(cfg => cfg.CreateMap<BecomePerformerViewModel, ClientViewModelBLL>()
                .ForMember("RegistrationDate", opt => opt.MapFrom(c => DateTime.Today))
                .ForMember("IsPerformer", opt => opt.MapFrom(c => true))
                .ForMember("AdminStatus", opt => opt.MapFrom(c => false))
                .ForMember("Rating", opt => opt.MapFrom(c => 0))
                .ForMember("PictureId", opt => opt.MapFrom(c => _pictureService.FindByBytes(picture.Image).Value))
                );
            Mapper.Map(model, client);
            _userService.Update(client, selectedCategories);
            _unitOfWork.Save();

            return RedirectToAction("Index");    
        }

        [Authorize(Roles = "user")]
        public ActionResult Close(bool tryOnceAgain)
        {
            var userDto = _userService.FindById(User.Identity.GetUserId<int>());
            if(!(userDto.AdminStatus && !userDto.IsPerformer))
                return View("~/Views/Error/Forbidden.cshtml");

            userDto.AdminStatus = false;
            _userService.Update(userDto);
            _unitOfWork.Save();

            if (tryOnceAgain)
                return RedirectToAction("BecomePerformer");
            return RedirectToAction("Index");
        }

        #region Helpers

        private void AddErrors(IdentityResult result)
        {
            foreach (var error in result.Errors)
            {
                ModelState.AddModelError("", error);
            }
        }

        private void GetCategoriesList()
        {
            var categoriesDto = _categoryService.GetAll().ToList();
            Mapper.Initialize(cfg => cfg.CreateMap<CategoryViewModelBLL, CategoryViewModel>());
            var categories = Mapper.Map<List<CategoryViewModelBLL>, List<CategoryViewModel>>(categoriesDto);
            ViewBag.Categories = categories;
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