using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Microsoft.AspNet.Identity;
using ServiceBooking.BLL.Interfaces;
using ServiceBooking.DAL.Interfaces;
using ServiceBooking.Util;
using ServiceBooking.WEB.Models;

namespace ServiceBooking.WEB.Controllers
{
    public class OrdersController : Controller
    {
        private static IOrderService _orderService;
        private static IUnitOfWork _unitOfWork;

        public OrdersController() : this(_orderService, _unitOfWork) { }

        public OrdersController(IOrderService service, IUnitOfWork unitOfWork)
        {
            _orderService = service;
            _unitOfWork = unitOfWork;
        }

        public OrdersController(NinjectDependencyResolver resolver, IOrderService userService, IUnitOfWork unitOfWork)
        {
            _orderService = userService;
            _unitOfWork = unitOfWork;
        }

        // GET: Orders
        public ActionResult Index(bool? newApplications, bool? myOrders)
        {
            //for creation
            //if (ViewBag.IsAuthorized != null && !ViewBag.IsAuthorized)
            //    return RedirectToAction("Login", "Account");

            var ordersDto = _orderService.GetAll();

            ViewBag.IsNewOrdersPage = false;
            if (newApplications != null && newApplications.Value)
            {
                ordersDto = ordersDto.Where(model => !model.AdminStatus);
                ViewBag.IsNewOrdersPage = true;
            }
            else
            {
                ordersDto = ordersDto.Where(model => model.AdminStatus);
            }
            ordersDto = ordersDto.OrderByDescending(m => m.UploadDate);

            ViewBag.IsMyOrdersPage = false;

            if (myOrders != null && myOrders.Value)
            {
                ordersDto = _orderService.GetAll();
                ordersDto = ordersDto.Where(model => model.ClientUserId == User.Identity.GetUserId<int>()).OrderByDescending(m => m.UploadDate);

                if (!ordersDto.Any())
                {
                    ViewBag.Message = "You have no orders";
                }

                ViewBag.IsMyOrdersPage = true;
            }

            var orders = new List<IndexOrderViewModel>();
            foreach (var order in ordersDto)
            {
               orders.Add(new IndexOrderViewModel
               {
                   
                   Name = order.Name,
                   Category = order.Category,
                   Status = order.Status,
                   AdminStatus = order.AdminStatus,
                   UploadDate = order.UploadDate,
                   CompletionDate = order.CompletionDate,
                   Price = order.Price,
                   ClientUserId = order.ClientUserId
               });
            }

            return View(orders);
        }
    }
}