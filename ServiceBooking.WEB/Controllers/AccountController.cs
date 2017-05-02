using System;
using System.Linq;
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
using ServiceBooking.DAL.Interfaces;
using AutoMapper;
using Microsoft.AspNet.Identity.Owin;
using ServiceBooking.DAL.Entities;
using ServiceBooking.DAL.Identity;

namespace ServiceBooking.WEB.Controllers
{
    public class AccountController : Controller
    {
        private static IUserService _userService;
        private static IPictureService _pictureService;
        private static IResponseService _responseService;
        private static IOrderService _orderService;
        private static ICommentService _commentService;
        private static IUnitOfWork _unitOfWork;

        public AccountController() : this(_userService, _pictureService,
            _responseService, _orderService, _commentService, _unitOfWork)
        {
        }

        public AccountController(IUserService userService, IPictureService pictureService,
            IResponseService responseService, IOrderService orderService,
            ICommentService commentService, IUnitOfWork unitOfWork)
        {
            _userService = userService;
            _pictureService = pictureService;
            _responseService = responseService;
            _orderService = orderService;
            _commentService = commentService;
            _unitOfWork = unitOfWork;
        }

        private IAuthenticationManager AuthenticationManager => HttpContext.GetOwinContext().Authentication;

        public ActionResult Login()
        {
            if (Request.IsAuthenticated)
                return View("~/Views/Error/Forbidden.cshtml");
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Login(LoginViewModel model)
        {
            if (Request.IsAuthenticated)
                return View("~/Views/Error/Forbidden.cshtml");

            if (ModelState.IsValid)
            {
                ClientViewModelBLL userViewModel = new ClientViewModelBLL
                {
                    Email = model.Email,
                    Password = model.Password
                };
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

        public ActionResult Register()
        {
            if (Request.IsAuthenticated)
                return View("~/Views/Error/Forbidden.cshtml");
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Register(RegisterViewModel model)
        {
            if (Request.IsAuthenticated)
                return View("~/Views/Error/Forbidden.cshtml");

            if (ModelState.IsValid)
            {
                Mapper.Initialize(cfg => cfg.CreateMap<RegisterViewModel, ClientViewModelBLL>()
                    .ForMember("UserName", opt => opt.MapFrom(c => c.Email))
                    .ForMember("EmailConfirmed", opt => opt.MapFrom(c => false))
                    .ForMember("IsPerformer", opt => opt.MapFrom(c => false))
                    .ForMember("Rating", opt => opt.MapFrom(c => 0))
                    .ForMember("RegistrationDate", opt => opt.MapFrom(c => DateTime.Today))
                    .ForMember("AdminStatus", opt => opt.MapFrom(c => false))
                    .ForMember("Role", opt => opt.MapFrom(c => "user")));

                ClientViewModelBLL userViewModel = Mapper.Map<RegisterViewModel, ClientViewModelBLL>(model);

                OperationDetails operationDetails = await _userService.Create(userViewModel);
                _unitOfWork.Save();
                if (operationDetails.Succedeed)
                {
                    int userId = _userService.FindByUserName(userViewModel.UserName).Id;
                    string code = await _userService.GenerateEmailConfirmationToken(userId);
                    var callbackUrl = Url.Action("ConfirmEmail", "Account", new { userId = userViewModel.Id, EmailConfirmed = userViewModel.EmailConfirmed, code = code }, protocol: Request.Url.Scheme);
                    _userService.SendEmail(userId, callbackUrl);

                    //return View("DisplayEmail");

                    return View("RegistrationSucceeded");
                }

                ModelState.AddModelError(operationDetails.Property, operationDetails.Message);
            }
            return View(model);
        }

        [Authorize(Roles = "user")]
        public ActionResult DeleteAccount()
        {
            return View("DeleteAccount");
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "user")]
        public async Task<ActionResult> DeleteAccount(DeleteAccountViewModel model)
        {
            if (ModelState.IsValid)
            {
                var user = _userService.FindById(User.Identity.GetUserId<int>());

                var picture = _userService.FindById(user.Id).PictureId;
                if (picture.HasValue)
                    _pictureService.Delete(picture.Value);

                var ordersId = _orderService.GetAll().Where(o => o.UserId == user.Id).Select(o => o.Id).ToList();
                if (ordersId.Any())
                {
                    var responses = _responseService.GetAll()
                        .Select(r => r.OrderId.Value)
                        .Where(r => ordersId.Contains(r)).ToList();
                    if (responses.Any())
                    {
                        foreach (var response in responses)
                        {
                            _responseService.Delete(response);
                        }
                    }
                }

                var comments = _commentService.GetAll().Where(c => c.CustomerId == user.Id).ToList();
                if (comments.Any())
                {
                    foreach (var comment in comments)
                    {
                        _commentService.Delete(comment.Id);
                    }
                }

                ClientViewModelBLL userDto = new ClientViewModelBLL
                {
                    Password = model.Password,
                    Email = User.Identity.Name
                };
                var operationDetails = await _userService.DeleteAccount(userDto);
                if (operationDetails.Succedeed)
                {
                    _unitOfWork.Save();
                    LogOff();
                    return RedirectToAction("Index", "Home");
                }

                ModelState.AddModelError(operationDetails.Property, operationDetails.Message);
            }
            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult LogOff()
        {
            AuthenticationManager.SignOut(DefaultAuthenticationTypes.ApplicationCookie);
            Session["adminStatus"] = Session["isPerformer"] = false;
            return RedirectToAction("Index", "Home");
        }

        [AllowAnonymous]
        public async Task<ActionResult> ConfirmEmail(int userId, string code)
        {
            var result = await _userService.ConfirmEmail(userId, code);
            if (result.Succeeded)
            {
                var user = _userService.FindById(userId);
                user.EmailConfirmed = true;
                _userService.Update(user);
                _unitOfWork.Save();
                return View("ConfirmEmail");
            }
            return View("Register");
        }
    }
}