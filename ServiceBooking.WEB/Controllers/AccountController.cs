﻿using System.Collections.Generic;
using System.Web;
using System.Web.Mvc;
using Microsoft.Owin.Security;
using System.Threading.Tasks;
using System.Security.Claims;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using ServiceBooking.BLL.DTO;
using ServiceBooking.BLL.Infrastructure;
using ServiceBooking.BLL.Interfaces;
using ServiceBooking.WEB.Models;
using ServiceBooking.Util;
using Microsoft.Owin;
using Microsoft.Owin.Host.SystemWeb;

namespace ServiceBooking.WEB.Controllers
{
    public class AccountController : Controller
    {
        private readonly IUserService _userService;

        public AccountController(IUserService service)
        {
            _userService = service;
        }

        //private IUserService UserService
        //{
        //    get
        //    {
        //        return HttpContext.GetOwinContext().GetUserManager<IUserService>();
        //    }
        //}

        public AccountController(NinjectDependencyResolver resolver, IUserService userService)
        {
            _userService = userService;
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
            await SetInitialDataAsync();
            if (ModelState.IsValid)
            {
                ClientViewModel userViewModel = new ClientViewModel { Email = model.Email, Password = model.Password };
                ClaimsIdentity claim = await _userService.Authenticate(userViewModel);
                if (claim == null)
                {
                    ModelState.AddModelError("", "Wrong login or password");
                }
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
            await SetInitialDataAsync();
            if (ModelState.IsValid)
            {
                ClientViewModel userViewModel = new ClientViewModel
                {
                    Email = model.Email,
                    Password = model.Password,
                    EmailConfirmed = false,
                    UserName = model.Email,
                    Name = model.Name,
                    Surname = model.Surname,
                    IsPerformer = false,
                    Role = "user"
                };

                OperationDetails operationDetails = await _userService.Create(userViewModel);
                if (operationDetails.Succedeed)
                {
                    //string code = await UserManager.GenerateEmailConfirmationTokenAsync(userViewModel.Id);
                    //var callbackUrl = Url.Action("ConfirmEmail", "Account", new { userId = userViewModel.Id, EmailConfirmed = userViewModel.EmailConfirmed, code = code }, protocol: Request.Url.Scheme);
                    //await UserManager.SendEmailAsync(userViewModel.Id, "Confirm your account", "Please confirm your account by clicking <a href=\"" + callbackUrl + "\">here</a>");

                    return View("DisplayEmail");
                }
                else
                    ModelState.AddModelError(operationDetails.Property, operationDetails.Message);
            }
            return View(model);
        }

        /*
         * [HttpGet]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteAccount()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> DeleteAccount(DeleteAccountViewModel model)
        {
            if (ModelState.IsValid)
            {
                UserViewModel userViewModel = new UserViewModel
                {
                    Id = User.Identity.GetUserId(),
                    Password = model.Password,
                    Role = "user"
                };

                OperationDetails operationDetails = await UserService.Create(userViewModel);
                if (operationDetails.Succedeed)
                {
                    //string code = await UserManager.GenerateEmailConfirmationTokenAsync(userViewModel.Id);
                    //var callbackUrl = Url.Action("ConfirmEmail", "Account", new { userId = userViewModel.Id, EmailConfirmed = userViewModel.EmailConfirmed, code = code }, protocol: Request.Url.Scheme);
                    //await UserManager.SendEmailAsync(userViewModel.Id, "Confirm your account", "Please confirm your account by clicking <a href=\"" + callbackUrl + "\">here</a>");

                    return View("~/Views/Home/Index.cshtml");
                }
                else
                    ModelState.AddModelError(operationDetails.Property, operationDetails.Message);
            }
            return View(model);
        }
         */

        [AllowAnonymous]
        public async Task<ActionResult> ConfirmEmail(string userId, string code)
        {
            if (userId == null || code == null)
            {
                return View("Error");
            }

            //UserViewModel user = this.UserManager.FindById(userId);
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

        private async Task SetInitialDataAsync()
        {
            await _userService.SetInitialData(new ClientViewModel
            {
                Email = "kruner.kruner@gmail.com",
                UserName = "kruner.kruner@gmail.com",
                Password = "Kruner_13",
                EmailConfirmed = true,
                Name = "Veronika Navros",
                IsPerformer = false,
                Role = "admin",
            }, new List<string> { "user", "admin" });
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