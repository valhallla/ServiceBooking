using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using Microsoft.AspNet.Identity;
using ServiceBooking.BLL.DTO;
using ServiceBooking.BLL.Interfaces;
using ServiceBooking.DAL.Interfaces;
using ServiceBooking.Util;
using ServiceBooking.WEB.Models;
using AutoMapper;
using ServiceBooking.BLL.Infrastructure;

namespace ServiceBooking.WEB.Controllers
{
    public class OrdersController : Controller
    {
        private static IOrderService _orderService;
        private static ICategoryService _categoryService;
        private static IStatusService _statusService;
        private static IResponseService _responseService;
        private static IUserService _userService;
        private static IUnitOfWork _unitOfWork;

        public OrdersController() : this(_orderService, _categoryService, 
            _statusService, _responseService, _userService, _unitOfWork) { }

        public OrdersController(IOrderService orderService, ICategoryService categoryService, 
            IStatusService statusService, IResponseService responseService, 
            IUserService userService, IUnitOfWork unitOfWork)
        {
            _orderService = orderService;
            _categoryService = categoryService;
            _statusService = statusService;
            _responseService = responseService;
            _userService = userService;
            _unitOfWork = unitOfWork;
        }

        public OrdersController(NinjectDependencyResolver resolver, IOrderService orderService, 
            ICategoryService categoryService, IStatusService statusService, IResponseService responseService, 
            IUserService userService, IUnitOfWork unitOfWork)
        {
            _orderService = orderService;
            _categoryService = categoryService;
            _statusService = statusService;
            _responseService = responseService;
            _userService = userService;
            _unitOfWork = unitOfWork;
        }

        // GET: Orders
        public ActionResult Index(bool? newApplications, bool? myOrders)
        {
            var ordersDto = _orderService.GetAll();

            ViewBag.NewOrdersAmountString = string.Empty;
            var newOrdersAmount = ordersDto.Count(model => !model.AdminStatus);
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
                ordersDto = ordersDto.Where(model => model.UserId == User.Identity.GetUserId<int>()).OrderByDescending(m => m.UploadDate);

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

        // GET: Orders/Details/5
        public ActionResult Details(int id)
        {
            var responsesDto = _responseService.GetAllForOrder(id);
            Mapper.Initialize(cfg => cfg.CreateMap<ResponseViewModel, IndexResponseViewModel>()
                .ForMember("PerformerId", opt => opt.MapFrom(c => c.PerformerId))
                .ForMember("PerformerName", opt => opt.MapFrom(c => _userService.FindById(c.PerformerId).Surname
                    + " " + _userService.FindById(c.PerformerId).Name))
                .ForMember("PerformerRating", opt => opt.MapFrom(c => _userService.FindById(c.PerformerId).Rating))
                );
            var responses = Mapper.Map<IEnumerable<ResponseViewModel>, List<IndexResponseViewModel>>(responsesDto);

            var orderDto = _orderService.Find(id);
            if (orderDto == null)
                return HttpNotFound();
            var clientUser = _userService.FindById(orderDto.UserId);

            Mapper.Initialize(cfg => cfg.CreateMap<OrderViewModel, DetailsOrderViewModel>()
                .ForMember("CustomerId", opt => opt.MapFrom(c => clientUser.Id))
                .ForMember("CustomerName", opt => opt.MapFrom(c => clientUser.Surname + " " + clientUser.Name))
                .ForMember("Category", opt => opt.MapFrom(c => _categoryService.FindById(c.CategoryId).Name))
                .ForMember("Status", opt => opt.MapFrom(c => _statusService.FindById(c.StatusId).Value))
                .ForMember("Responses", opt => opt.MapFrom(c => responses))
                );
            DetailsOrderViewModel order = Mapper.Map<OrderViewModel, DetailsOrderViewModel>(orderDto);

            var currentUser = _userService.FindById(User.Identity.GetUserId<int>());
            ViewBag.IsPerformer = currentUser.IsPerformer;
            ViewBag.Rating = currentUser.Rating;

            return View(order);
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
                .ForMember("UploadDate", opt => opt.MapFrom(c => DateTime.Now))
                .ForMember("UserId", opt => opt.MapFrom(c => User.Identity.GetUserId<int>())));
            OrderViewModel orderDto = Mapper.Map<CreateOrderViewModel, OrderViewModel>(order);

            if (ModelState.IsValid)
            {
                OperationDetails operationDetails = _orderService.Create(orderDto);
                if (operationDetails.Succedeed)
                    return RedirectToAction("Index");
                ModelState.AddModelError(operationDetails.Property, operationDetails.Message);
            }
            return View(order);
        }

        public ActionResult Confirm(int id)
        {
            _orderService.ConfirmOrder(id);
            return RedirectToAction("Index", new {newApplications = true});
        }

        public ActionResult Reject(int id)
        {
            _orderService.DeleteOrder(id);
            return RedirectToAction("Index", new { newApplications = true });
        }

        // GET: Orders/Delete/5
        public ActionResult Delete(int id, bool? isMyOrdersPage)
        {
            var orderDto = _orderService.Find(id);
            if (orderDto == null)
                return HttpNotFound();
            var clientUser = _userService.FindById(orderDto.UserId);

            Mapper.Initialize(cfg => cfg.CreateMap<OrderViewModel, DeleteOrderViewModel>()
                .ForMember("CustomerName", opt => opt.MapFrom(c => clientUser.Id == User.Identity.GetUserId<int>() 
                    ? "you" : clientUser.Surname + " " + clientUser.Name)));
            DeleteOrderViewModel order = Mapper.Map<OrderViewModel, DeleteOrderViewModel>(orderDto);

            return View(order);
        }

        // POST: Orders/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Delete(int id)
        {
            _orderService.DeleteOrder(id);
            return RedirectToAction("Index");
        }
    }
}