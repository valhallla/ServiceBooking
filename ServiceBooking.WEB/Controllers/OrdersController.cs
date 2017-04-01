using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Microsoft.AspNet.Identity;
using ServiceBooking.BLL.DTO;
using ServiceBooking.BLL.Interfaces;
using ServiceBooking.DAL.Interfaces;
using ServiceBooking.Util;
using ServiceBooking.WEB.Models;
using AutoMapper;

namespace ServiceBooking.WEB.Controllers
{
    public class OrdersController : Controller
    {
        private static IOrderService _orderService;
        private static ICategoryService _categoryService;
        private static IStatusService _statusService;
        private static IUserService _userService;
        private static IUnitOfWork _unitOfWork;

        public OrdersController() : this(_orderService, _categoryService, _statusService, _userService, _unitOfWork) { }

        public OrdersController(IOrderService orderService, ICategoryService categoryService, 
            IStatusService statusService, IUserService userService, IUnitOfWork unitOfWork)
        {
            _orderService = orderService;
            _categoryService = categoryService;
            _statusService = statusService;
            _userService = userService;
            _unitOfWork = unitOfWork;
        }

        public OrdersController(NinjectDependencyResolver resolver, IOrderService orderService, 
            ICategoryService categoryService, IStatusService statusService, IUserService userService, IUnitOfWork unitOfWork)
        {
            _orderService = orderService;
            _categoryService = categoryService;
            _statusService = statusService;
            _userService = userService;
            _unitOfWork = unitOfWork;
        }

        // GET: Orders
        public ActionResult Index(bool? newApplications, bool? myOrders)
        {
            var ordersDto = _orderService.GetAll();

            ViewBag.NewOrdersAmountString = string.Empty;
            var newOrdersAmount = ordersDto.ToList().Count(model => !model.AdminStatus);
            if (newOrdersAmount > 0)
                ViewBag.NewOrdersAmountString = " + " +  newOrdersAmount.ToString();

            ViewBag.IsNewOrdersPage = false;
            if (newApplications != null && newApplications.Value)
            {
                ordersDto = ordersDto.Where(model => !model.AdminStatus);
                ViewBag.IsNewOrdersPage = true;
            }
            else
                ordersDto = ordersDto.Where(model => model.AdminStatus);
            ordersDto = ordersDto.OrderByDescending(m => m.UploadDate);

            ViewBag.IsMyOrdersPage = false;
            if (myOrders != null && myOrders.Value)
            {
                ordersDto = _orderService.GetAll();
                ordersDto = ordersDto.Where(model => model.ClientUserId == User.Identity.GetUserId<int>()).OrderByDescending(m => m.UploadDate);

                if (!ordersDto.Any())
                    ViewBag.Message = "You have no orders";
                ViewBag.IsMyOrdersPage = true;
            }

            Mapper.Initialize(cfg => cfg.CreateMap<OrderViewModel, IndexOrderViewModel>()
                .ForMember("Category", opt => opt.MapFrom(c => _categoryService.FindById(c.CategoryId).Name))
                .ForMember("Status", opt => opt.MapFrom(c => _statusService.FindById(c.StatusId).Value)));
            var orders = Mapper.Map<IEnumerable<OrderViewModel>, List<IndexOrderViewModel>>(ordersDto);

            return View(orders);
        }

        // GET: Orders/Create
        public ActionResult Create()
        {
            if (!Request.IsAuthenticated)
                return Redirect("/Account/Login");
            ViewBag.Category = new SelectList(_categoryService.GetAll(), "Name", "Name");
            return View();
        }

        // POST: Orders/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(CreateOrderViewModel order)
        {
            Mapper.Initialize(cfg => cfg.CreateMap<CreateOrderViewModel, OrderViewModel>()
                .ForMember("CategoryId", opt => opt.MapFrom(c => _categoryService.FindByName(c.Category).Id))
                .ForMember("StatusId", opt => opt.MapFrom(c => 1))
                .ForMember("AdminStatus", opt => opt.MapFrom(c => false))
                .ForMember("UploadDate", opt => opt.MapFrom(c => DateTime.Today))
                .ForMember("ClientUserId", opt => opt.MapFrom(c => User.Identity.GetUserId<int>()))
                );
            OrderViewModel orderDto = Mapper.Map<CreateOrderViewModel, OrderViewModel>(order);

            if (ModelState.IsValid)
            {
                _orderService.Create(orderDto);
                return RedirectToAction("Index");
            }
            return View(order);
        }

        public ActionResult Confirm(int id)
        {
            _orderService.ConfirmOrder(id);
            _unitOfWork.Save();
            return RedirectToAction("Index");
        }

        public ActionResult Reject(int id)
        {
            _orderService.RejectOrder(id);
            _unitOfWork.Save();
            return RedirectToAction("Index");
        }
    }
}