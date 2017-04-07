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
        public ActionResult Index(int? categoryId, string searchName, bool newApplications = false, 
            bool myOrders = false, OrderSorts sort = OrderSorts.New)
        {
            var ordersDto = _orderService.GetAll();

            ViewBag.NewOrdersAmountString = string.Empty;
            var newOrdersAmount = ordersDto.Count(model => !model.AdminStatus);
            if (newOrdersAmount > 0)
                ViewBag.NewOrdersAmountString = " + " +  newOrdersAmount;

            ViewBag.IsNewOrdersPage = false;
            if (newApplications)
            {
                ordersDto = ordersDto.Where(model => !model.AdminStatus);
                ViewBag.IsNewOrdersPage = true;
            }
            else
                ordersDto = ordersDto.Where(model => model.AdminStatus);

            ViewBag.IsMyOrdersPage = false;
            if (myOrders)
            {
                ordersDto = _orderService.GetAll();
                ordersDto = ordersDto.Where(model => model.UserId == User.Identity.GetUserId<int>());

                if (!ordersDto.Any())
                    ViewBag.Message = "You have no orders";
                ViewBag.IsMyOrdersPage = true;  
            }

            if (categoryId != null)
            {
                ordersDto = ordersDto.Where(o => o.CategoryId == categoryId);
                ViewBag.CurrentCategoryId = categoryId;
            }

            var categoriesDto = _categoryService.GetAll().ToList();
            Mapper.Initialize(cfg => cfg.CreateMap<CategoryViewModelBLL, CategoryViewModel>()
                .ForMember("ItemsAmount", opt => opt.MapFrom(c => c.Orders.Count(o =>
                        newApplications ? o.AdminStatus != newApplications :
                        (myOrders ? o.UserId == User.Identity.GetUserId<int>() : o.AdminStatus)
                )))
            );
            var categories = Mapper.Map<List<CategoryViewModelBLL>, List<CategoryViewModel>>(categoriesDto);
            ViewBag.CategoriesList = categories;

            if (searchName != null)
                ordersDto = ordersDto.Where(o => o.Name.Contains(searchName));
            ViewBag.SearchName = searchName;

            switch (sort)
            {
                case OrderSorts.New:
                    ordersDto = ordersDto.OrderByDescending(o => o.UploadDate); break;
                case OrderSorts.Expensive:
                    ordersDto = ordersDto.OrderByDescending(o => o.Price); break;
                case OrderSorts.Active:
                    ordersDto = ordersDto.OrderBy(o => o.StatusId); break;
            }
            ViewBag.Sort = sort;

            Mapper.Initialize(cfg => cfg.CreateMap<OrderViewModelBLL, IndexOrderViewModel>()
                .ForMember("Category", opt => opt.MapFrom(c => _categoryService.FindById(c.CategoryId).Name))
                .ForMember("Status", opt => opt.MapFrom(c => _statusService.FindById(c.StatusId).Value)));
            var orders = Mapper.Map<IEnumerable<OrderViewModelBLL>, List<IndexOrderViewModel>>(ordersDto);

            //var names = orders.Select(o => o.Name).ToArray();
            //ViewBag.Names = names;

            return View(orders);
        }

        // GET: Orders/Details/5
        public ActionResult Details(int id, int? categoryId, bool? newApplications, bool? myOrders)
        {
            var responsesDto = _responseService.GetAllForOrder(id);
            Mapper.Initialize(cfg => cfg.CreateMap<ResponseViewModelBLL, IndexResponseViewModel>()
                .ForMember("PerformerId", opt => opt.MapFrom(c => c.PerformerId))
                .ForMember("PerformerName", opt => opt.MapFrom(c => _userService.FindById(c.PerformerId).Surname
                    + " " + _userService.FindById(c.PerformerId).Name))
                .ForMember("PerformerRating", opt => opt.MapFrom(c => _userService.FindById(c.PerformerId).Rating))
                );
            var responses = Mapper.Map<IEnumerable<ResponseViewModelBLL>, List<IndexResponseViewModel>>(responsesDto);

            var orderDto = _orderService.Find(id);
            if (orderDto == null)
                return HttpNotFound();
            var clientUser = _userService.FindById(orderDto.UserId);

            Mapper.Initialize(cfg => cfg.CreateMap<OrderViewModelBLL, DetailsOrderViewModel>()
                .ForMember("CustomerId", opt => opt.MapFrom(c => clientUser.Id))
                .ForMember("CustomerName", opt => opt.MapFrom(c => clientUser.Surname + " " + clientUser.Name))
                .ForMember("Category", opt => opt.MapFrom(c => _categoryService.FindById(c.CategoryId).Name))
                .ForMember("Status", opt => opt.MapFrom(c => _statusService.FindById(c.StatusId).Value))
                .ForMember("Responses", opt => opt.MapFrom(c => responses))
                );
            DetailsOrderViewModel order = Mapper.Map<OrderViewModelBLL, DetailsOrderViewModel>(orderDto);

            var currentUser = _userService.FindById(User.Identity.GetUserId<int>());
            ViewBag.IsPerformer = currentUser.IsPerformer;
            ViewBag.Rating = currentUser.Rating;
            if (order.StatusId < 3)
                ViewBag.StatusMessage = "Mark as" + _statusService.FindById(order.StatusId + 1).Value;

            ViewBag.CurrentCategoryId = categoryId;
            ViewBag.IsMyOrdersPage = myOrders;
            ViewBag.IsNewOrdersPage = newApplications;

            return View(order);
        }

        public ActionResult ChangeStatus(int orderId)
        {
            _orderService.ChangeStatus(orderId);
            return RedirectToAction("Details", new { id = orderId });
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
            Mapper.Initialize(cfg => cfg.CreateMap<CreateOrderViewModel, OrderViewModelBLL>()
                .ForMember("CategoryId", opt => opt.MapFrom(c => _categoryService.FindByName(c.Category).Id))
                .ForMember("StatusId", opt => opt.MapFrom(c => 1))
                .ForMember("AdminStatus", opt => opt.MapFrom(c => false))
                .ForMember("UploadDate", opt => opt.MapFrom(c => DateTime.Now))
                .ForMember("UserId", opt => opt.MapFrom(c => User.Identity.GetUserId<int>())));
            OrderViewModelBLL orderDto = Mapper.Map<CreateOrderViewModel, OrderViewModelBLL>(order);

            if (ModelState.IsValid)
            {
                OperationDetails operationDetails = _orderService.Create(orderDto);
                if (operationDetails.Succedeed)
                    return RedirectToAction("Index");
                ModelState.AddModelError(operationDetails.Property, operationDetails.Message);
            }
            return View(order);
        }

        public ActionResult Confirm(int id, int? categoryId)
        {
            _orderService.ConfirmOrder(id);
            return RedirectToAction("Index", new
            {
                newApplications = _orderService.GetAll().Count(o => !o.AdminStatus) > 0,
                currentCategoryId = categoryId
            });
        }

        public ActionResult Reject(int id, int? categoryId)
        {
            _orderService.DeleteOrder(id);
            return RedirectToAction("Index", new
            {
                newApplications = _orderService.GetAll().Count(o => !o.AdminStatus) > 0,
                currentCategoryId = categoryId
            });
        }

        // GET: Orders/Delete/5
        public ActionResult Delete(int id, bool? isMyOrdersPage, bool? isNewOrdersPage, int? currentCategoryId)
        {
            var orderDto = _orderService.Find(id);
            if (orderDto == null)
                return HttpNotFound();
            var clientUser = _userService.FindById(orderDto.UserId);

            Mapper.Initialize(cfg => cfg.CreateMap<OrderViewModelBLL, DeleteOrderViewModel>()
                .ForMember("CustomerName", opt => opt.MapFrom(c => clientUser.Id == User.Identity.GetUserId<int>() 
                    ? "you" : clientUser.Surname + " " + clientUser.Name)));
            DeleteOrderViewModel order = Mapper.Map<OrderViewModelBLL, DeleteOrderViewModel>(orderDto);

            ViewBag.IsMyOrdersPage = isMyOrdersPage;
            ViewBag.ISnEWoRDERSpAGE = isNewOrdersPage;
            ViewBag.CurrentCategoryId = currentCategoryId;

            return View(order);
        }

        // POST: Orders/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id, bool? isMyOrdersPage, bool? isNewOrdersPage, int? currentCategoryId)
        {
            _orderService.DeleteOrder(id);
            return RedirectToAction("Index", new
            {
                newApplications = isNewOrdersPage,
                myOrders = isMyOrdersPage,
                categoryId = currentCategoryId
            });
        }

        public ActionResult AutocompleteSearch(string orderName)
        {
            var orders = _orderService.GetAll().Where(o => o.Name.Contains(orderName))
                            .Select(o => new { value = o.Name })
                            .Distinct();

            return Json(orders, JsonRequestBehavior.AllowGet);
        }
    }
}