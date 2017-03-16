//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Net;
//using System.Threading.Tasks;
//using System.Web;
//using System.Web.Mvc;
//using Microsoft.AspNet.Identity;
//using Microsoft.AspNet.Identity.Owin;
//using Microsoft.Owin.Security;
//using ServiceBooking.BLL.Services;
//using ServiceBooking.WEB.Models;

//namespace ServiceBooking.WEB.Controllers
//{
//    [Authorize]
//    public class ManageController : Controller
//    {
//        private UserService _userManager;

//        public UserService UserManager
//        {
//            get
//            {
//                return _userManager ?? HttpContext.GetOwinContext().GetUserManager<UserService>();
//            }
//            private set
//            {
//                _userManager = value;
//            }
//        }

//        // GET: Manage
//        public async Task<ActionResult> Index(ManageMessageId? message)
//        {
//            ViewBag.StatusMessage =
//               message == ManageMessageId.ChangePasswordSuccess ? "Your password has been changed."
//               : message == ManageMessageId.SetPasswordSuccess ? "Your password has been set."
//               : message == ManageMessageId.SetTwoFactorSuccess ? "Your two-factor authentication provider has been set."
//               : message == ManageMessageId.Error ? "An error has occurred."
//               : message == ManageMessageId.AddPhoneSuccess ? "Your phone number was added."
//               : message == ManageMessageId.RemovePhoneSuccess ? "Your phone number was removed."
//               : "";

//            var userId = User.Identity.GetUserId();
//            var model = new IndexViewModel
//            {
//                //PhoneNumber = await UserManager.GetPhoneNumberAsync(userId),
//                BrowserRemembered = await AuthenticationManager.TwoFactorBrowserRememberedAsync(userId)
//            };
//            return View(model);
//        }

//        //
//        // GET: /Manage/ChangePassword
//        public ActionResult ChangePassword()
//        {
//            return View();
//        }

//        //
//        // POST: /Manage/ChangePassword
//        //[HttpPost]
//        //[ValidateAntiForgeryToken]
//        //public async Task<ActionResult> ChangePassword(ChangePasswordViewModel model)
//        //{
//        //    if (!ModelState.IsValid)
//        //    {
//        //        return View(model);
//        //    }
//        //    var result = await UserManager.ChangePasswordAsync(User.Identity.GetUserId(), model.OldPassword, model.NewPassword);
//        //    if (result.Succeeded)
//        //    {
//        //        var user = await UserManager.FindByIdAsync(User.Identity.GetUserId());
//        //        if (user != null)
//        //        {
//        //            await SignInManager.SignInAsync(user, isPersistent: false, rememberBrowser: false);
//        //        }
//        //        return RedirectToAction("Index", new { Message = ManageMessageId.ChangePasswordSuccess });
//        //    }
//        //    AddErrors(result);
//        //    return View(model);
//        //}

//        #region Helpers
//        private IAuthenticationManager AuthenticationManager
//        {
//            get
//            {
//                return HttpContext.GetOwinContext().Authentication;
//            }
//        }

//        private void AddErrors(IdentityResult result)
//        {
//            foreach (var error in result.Errors)
//            {
//                ModelState.AddModelError("", error);
//            }
//        }

//        public enum ManageMessageId
//        {
//            AddPhoneSuccess,
//            ChangePasswordSuccess,
//            SetTwoFactorSuccess,
//            SetPasswordSuccess,
//            RemoveLoginSuccess,
//            RemovePhoneSuccess,
//            Error
//        }
//        #endregion
//    }
//}