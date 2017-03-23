using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Security.Claims;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using Microsoft.Owin.Security;
using ServiceBooking.BLL.DTO;
using ServiceBooking.BLL.Interfaces;
using ServiceBooking.WEB.Models;

namespace ServiceBooking.WEB.Controllers
{
    [Authorize]
    public class ManageController : Controller
    {
        private IUserService _userService;

        public IUserService UserService
        {
            get
            {
                return _userService ?? HttpContext.GetOwinContext().GetUserManager<IUserService>();
            }
            private set
            {
                _userService = value;
            }
        }

        // GET: Manage
        public async Task<ActionResult> Index(ManageMessageId? message)
        {
            ViewBag.StatusMessage =
               message == ManageMessageId.ChangePasswordSuccess ? "Your password has been changed."
               : message == ManageMessageId.SetPasswordSuccess ? "Your password has been set."
               : message == ManageMessageId.SetTwoFactorSuccess ? "Your two-factor authentication provider has been set."
               : message == ManageMessageId.Error ? "An error has occurred."
               : message == ManageMessageId.AddPhoneSuccess ? "Your phone number was added."
               : message == ManageMessageId.RemovePhoneSuccess ? "Your phone number was removed."
               : "";

            var userId = User.Identity.GetUserId();
            var model = new IndexViewModel
            {
                BrowserRemembered = await AuthenticationManager.TwoFactorBrowserRememberedAsync(userId)
            };
            return View(model);
        }

        //
        // GET: /Manage/ChangePassword
        public ActionResult ChangePassword()
        {
            return View("ChangePassword");
        }

        //
        // POST: /Manage/ChangePassword
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> ChangePassword(ChangePasswordViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            ClientViewModel client = new ClientViewModel
            {
                Id = User.Identity.GetUserId(),
                UserName = model.OldPassword,
                Password = model.NewPassword
            };
            var result = await UserService.ChangePassword(client);
            if (result.Succeeded)
            {
                //var user = await UserService.FindById(User.Identity.GetUserId());
                //if (user != null)
                //{
                //    ClaimsIdentity claim = await _userService.Authenticate(client);
                //    if (claim == null)
                //    {
                //        ModelState.AddModelError("", "Wrong password");
                //    }
                //    else
                //    {
                //        AuthenticationManager.SignOut();
                //        AuthenticationManager.SignIn(new AuthenticationProperties
                //        {
                //            IsPersistent = true
                //        }, claim);
                //        return RedirectToAction("Index", new { Message = ManageMessageId.ChangePasswordSuccess });
                //    }
                //}
                return RedirectToAction("Index", new { Message = ManageMessageId.ChangePasswordSuccess });
            }
            AddErrors(result);
            return View(model);
        }

        //
        // GET: /Manage/ChangePassword
        public ActionResult DeleteAccount()
        {
            return View("DeleteAccount");
        }

        //
        // POST: /Manage/ChangePassword
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> DeleteAccount(ChangePasswordViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            ClientViewModel client = new ClientViewModel
            {
                Id = User.Identity.GetUserId(),
                UserName = model.OldPassword,
                Password = model.NewPassword
            };
            var result = await UserService.ChangePassword(client);
            if (result.Succeeded)
            {
                var user = await UserService.FindById(User.Identity.GetUserId());
                if (user != null)
                {
                    ClaimsIdentity claim = await _userService.Authenticate(client);
                    if (claim == null)
                    {
                        ModelState.AddModelError("", "Wrong password");
                    }
                    else
                    {
                        AuthenticationManager.SignOut();
                        AuthenticationManager.SignIn(new AuthenticationProperties
                        {
                            IsPersistent = true
                        }, claim);
                        return RedirectToAction("Index", new { Message = ManageMessageId.ChangePasswordSuccess });
                    }
                }
                return RedirectToAction("Index", new { Message = ManageMessageId.ChangePasswordSuccess });
            }
            AddErrors(result);
            return View(model);
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