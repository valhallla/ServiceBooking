using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Microsoft.AspNet.Identity;
using ServiceBooking.BLL.Interfaces;
using ServiceBooking.DAL.Interfaces;
using ServiceBooking.Util;

namespace ServiceBooking.WEB.Controllers
{
    public class HomeController : Controller
    {
        private static IOrderService _orderService;
        private static IUserService _userService;

        public HomeController() : this(_orderService, _userService)
        { }

        public HomeController(IOrderService orderService, IUserService userService)
        {
            _orderService = orderService;
            _userService = userService;
        }

        public HomeController(NinjectDependencyResolver resolver, IOrderService orderService, IUserService userService)
        {
            _orderService = orderService;
            _userService = userService;
        }

        public ActionResult Index()
        {
            var orders = _orderService.GetAll();
            if (orders == null)
                return HttpNotFound();

            ViewBag.NewOrdersAmountString = string.Empty;
            var newOrdersAmount = orders.Count(model => !model.AdminStatus);
            if (newOrdersAmount > 0)
                ViewBag.NewOrdersAmountString = " + " + newOrdersAmount;

            var users = _userService.GetAll();
            if (users == null)
                return HttpNotFound();

            ViewBag.NewPerformerssAmountString = string.Empty;
            var newPerformersAmount = users.Count(model => !model.AdminStatus && model.IsPerformer);
            if (newPerformersAmount > 0)
                ViewBag.NewPerformerssAmountString = " + " + newPerformersAmount;

            return View();
        }
    }
}