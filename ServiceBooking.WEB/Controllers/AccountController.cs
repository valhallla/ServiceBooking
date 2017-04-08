﻿using System;
using System.Web;
using System.Web.Mvc;
using Microsoft.Owin.Security;
using System.Threading.Tasks;
using System.Security.Claims;
using Microsoft.AspNet.Identity;
using ServiceBooking.BLL.DTO;
using ServiceBooking.BLL.Infrastructure;
using ServiceBooking.BLL.Interfaces;
using ServiceBooking.WEB.Models;
using ServiceBooking.Util;
using ServiceBooking.DAL.Interfaces;
using AutoMapper;

namespace ServiceBooking.WEB.Controllers
{
    public class AccountController : Controller
    {
        private static IUserService _userService;
        private static IUnitOfWork _unitOfWork;

        public AccountController() : this(_userService, _unitOfWork) { }

        public AccountController(IUserService service, IUnitOfWork unitOfWork) 
        {
            _userService = service;
            _unitOfWork = unitOfWork;
        }

        private IAuthenticationManager AuthenticationManager
        {
            get
            {
                return HttpContext.GetOwinContext().Authentication;
            }
        }
        public ActionResult Login()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Login(LoginViewModel model)
        {
            if (ModelState.IsValid)
            {
                ClientViewModelBLL userViewModel = new ClientViewModelBLL { Email = model.Email, Password = model.Password };
                ClaimsIdentity claim = await _userService.Authenticate(userViewModel);
                if (claim == null)
                    ModelState.AddModelError("", "Wrong login or password");
                else
                {
                    AuthenticationManager.SignOut();
                    AuthenticationManager.SignIn(new AuthenticationProperties
                    {
                        IsPersistent = true
                    }, claim);
                    return RedirectToAction("Index", "Home");
                }
            }
            return View(model);
        }

        public ActionResult Logout()
        {
            AuthenticationManager.SignOut();
            return RedirectToAction("Index", "Home");
        }

        public ActionResult Register()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Register(RegisterViewModel model)
        {
            if (ModelState.IsValid)
            {
                Mapper.Initialize(cfg => cfg.CreateMap<RegisterViewModel, ClientViewModelBLL>()
                    .ForMember("UserName", opt => opt.MapFrom(c => c.Email))
                    .ForMember("EmailConfirmed", opt => opt.MapFrom(c => true))
                    .ForMember("IsPerformer", opt => opt.MapFrom(c => false))
                    .ForMember("Rating", opt => opt.MapFrom(c => 0))
                    .ForMember("RegistrationDate", opt => opt.MapFrom(c => DateTime.Today))
                    .ForMember("AdminStatus", opt => opt.MapFrom(c => false))
                    .ForMember("Role", opt => opt.MapFrom(c => "user")));

                ClientViewModelBLL userViewModel = Mapper.Map<RegisterViewModel, ClientViewModelBLL>(model);

                OperationDetails operationDetails = await _userService.Create(userViewModel);
                if (operationDetails.Succedeed)
                {
                    //HttpContext.GetOwinContext().GetUserManager<ApplicationUserManager>(
                    //string code = await UserManager.GenerateEmailConfirmationTokenAsync(userViewModel.Id);
                    //var callbackUrl = Url.Action("ConfirmEmail", "Account", new { userId = userViewModel.Id, EmailConfirmed = userViewModel.EmailConfirmed, code = code }, protocol: Request.Url.Scheme);
                    //await UserManager.SendEmailAsync(userViewModel.Id, "Confirm your account", "Please confirm your account by clicking <a href=\"" + callbackUrl + "\">here</a>");

                    //return View("DisplayEmail");
                    return View("RegistrationSucceeded");
                }
                else
                    ModelState.AddModelError(operationDetails.Property, operationDetails.Message);
            }
            return View(model);
        }

        public ActionResult DeleteAccount()
        {
            return View("DeleteAccount");
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> DeleteAccount(DeleteAccountViewModel model)
        {
            if (ModelState.IsValid)
            {
                ClientViewModelBLL user = new ClientViewModelBLL {Password = model.Password, Email = User.Identity.Name};
                var operationDetails = await _userService.DeleteAccount(user);
                if (operationDetails.Succedeed)
                {
                    LogOff();
                    return RedirectToAction("Index", "Home");
                }

                ModelState.AddModelError(operationDetails.Property, operationDetails.Message);
            }
            return View(model);
        }

        [AllowAnonymous]
        public async Task<ActionResult> ConfirmEmail(string userId, string code)
        {
            if (userId == null || code == null)
            {
                return View("Error");
            }

            //ClientViewModel user = _userService.Find(userId);
            //var result = await UserManager.ConfirmEmailAsync(userId, code);
            //if (result.Succeeded)
            //{
            //    user.EmailConfirmed = true;
            //    return View("ConfirmEmail");
            //}
            //else
            {
                return View("Error");
            }
            //return View(result.Succeeded ? "ConfirmEmail" : "Error");
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult LogOff()
        {
            AuthenticationManager.SignOut(DefaultAuthenticationTypes.ApplicationCookie);
            return RedirectToAction("Index", "Home");
        }
    }
}
