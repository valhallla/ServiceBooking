using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Microsoft.AspNet.Identity;
using ServiceBooking.BLL.DTO;
using ServiceBooking.BLL.Interfaces;
using ServiceBooking.DAL.Interfaces;
using ServiceBooking.WEB.Models;
using AutoMapper;
using ServiceBooking.BLL.Infrastructure;
using PagedList;
using ServiceBooking.DAL.UnitOfWork.DTO;

namespace ServiceBooking.WEB.Controllers
{
    public class OrdersController : Controller
    {
        private static IOrderService _orderService;
        private static ICategoryService _categoryService;
        private static IStatusService _statusService;
        private static IResponseService _responseService;
        private static IUserService _userService;
        private static IPictureService _pictureService;
        private static IUnitOfWork _unitOfWork;

        private const string DefaultImageName = @"~/Content/default-order.jpg";

        public OrdersController() : this(_orderService, _categoryService,
            _statusService, _responseService, _userService, _pictureService, _unitOfWork) { }

        public OrdersController(IOrderService orderService, ICategoryService categoryService,
            IStatusService statusService, IResponseService responseService,
            IUserService userService, IPictureService pictureService, IUnitOfWork unitOfWork)
        {
            _orderService = orderService;
            _categoryService = categoryService;
            _statusService = statusService;
            _responseService = responseService;
            _userService = userService;
            _pictureService = pictureService;
            _unitOfWork = unitOfWork;
        }

        public ActionResult Index(int? page, int? categoryId, string searchName, 
            bool newApplications = false, bool myOrders = false, OrderSorts sort = OrderSorts.New)
        {
            var ordersDto = _orderService.GetAll();
            if (ordersDto == null)
                return HttpNotFound();
            if (newApplications && !ordersDto.Any(o => !o.AdminStatus))
                ViewBag.AdminMessage = "No new orders";

            ViewBag.NewOrdersAmountString = string.Empty;
            var newOrdersAmount = ordersDto.Count(model => !model.AdminStatus);
            if (newOrdersAmount > 0)
                ViewBag.NewOrdersAmountString = " + " + newOrdersAmount;

            ViewBag.IsNewPage = false;
            if (newApplications)
            {
                ordersDto = ordersDto.Where(model => !model.AdminStatus);
                ViewBag.IsNewPage = true;
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
            if (!ordersDto.Any())
                ViewBag.SearchMessage = "No orders found";

            switch (sort)
            {
                case OrderSorts.New:
                    ordersDto = ordersDto.OrderByDescending(o => o.UploadDate); break;
                case OrderSorts.Urgent:
                    ordersDto = ordersDto.OrderBy(o => o.CompletionDate); break;
                case OrderSorts.Expensive:
                    ordersDto = ordersDto.OrderByDescending(o => o.Price); break;
                case OrderSorts.Active:
                    ordersDto = ordersDto.OrderBy(o => o.StatusId); break;
            }
            ViewBag.Sort = sort;
            ViewBag.ItemsAmount = ordersDto.Count();

            Mapper.Initialize(cfg => cfg.CreateMap<OrderViewModelBLL, IndexOrderViewModel>()
                .ForMember("Category", opt => opt.MapFrom(c => _categoryService.FindById(c.CategoryId).Name))
                .ForMember("Status", opt => opt.MapFrom(c => _statusService.FindById(c.StatusId).Value))
                .ForMember("Image", opt => opt.MapFrom(c => c.PictureId == null
                    ? System.IO.File.ReadAllBytes(Server.MapPath(DefaultImageName))
                    : _pictureService.FindById(c.PictureId.Value).Image))
            );
            var orders = Mapper.Map<IEnumerable<OrderViewModelBLL>, List<IndexOrderViewModel>>(ordersDto);

            var orderNames = _orderService.GetAll().Select(o => o.Name).ToArray();
            var filteredNames = orderNames.Where(o => o.IndexOf(searchName, StringComparison.InvariantCultureIgnoreCase) >= 0);
            ViewBag.Names = Json(filteredNames, JsonRequestBehavior.AllowGet);

            int pageSize = 3;
            int pageNumber = (page ?? 1);
            return View(orders.ToPagedList(pageNumber, pageSize));
        }

        public ActionResult Details(int id, int? categoryId, bool? newApplications, 
            bool? myOrders, OrderSorts sort = OrderSorts.New, bool emptyResponse = false)
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
                .ForMember("Image", opt => opt.MapFrom(c => c.PictureId == null
                ? System.IO.File.ReadAllBytes(Server.MapPath(DefaultImageName))
                : _pictureService.FindById(c.PictureId.Value).Image))
            );
            DetailsOrderViewModel order = Mapper.Map<OrderViewModelBLL, DetailsOrderViewModel>(orderDto);

            var currentUser = _userService.FindById(User.Identity.GetUserId<int>());
            if (!ReferenceEquals(currentUser, null))
                ViewBag.Rating = currentUser.Rating;
            if (order.StatusId < 3)
                ViewBag.StatusMessage = "Mark as" + _statusService.FindById(order.StatusId + 1).Value;

            ViewBag.CurrentCategoryId = categoryId;
            ViewBag.IsMyOrdersPage = myOrders;
            ViewBag.IsNewPage = newApplications;
            ViewBag.ResponseIsEmpty = emptyResponse;
            ViewBag.Sort = sort;

            return View(order);
        }

        [Authorize(Roles = "user")]
        public ActionResult ChangeStatus(int orderId)
        {
            if (User.Identity.GetUserId<int>() != _orderService.Find(orderId).UserId)
                return View("~/Views/Error/Forbidden.cshtml");

            _orderService.ChangeStatus(orderId);
            _unitOfWork.Save();
            return RedirectToAction("Details", new { id = orderId });
        }

        [Authorize(Roles = "user")]
        public ActionResult Create()
        {
            ViewBag.Category = new SelectList(_categoryService.GetAll(), "Name", "Name");
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "user")]
        public ActionResult Create(CreateOrderViewModel order, HttpPostedFileBase loadImage)
        {
            if (ModelState.IsValid)
            {
                PictureViewModelBLL picture = null;
                if (!ReferenceEquals(loadImage, null))
                {
                    byte[] image;
                    using (var binaryReader = new BinaryReader(loadImage.InputStream))
                    {
                        image = binaryReader.ReadBytes(loadImage.ContentLength);
                    }
                    picture = new PictureViewModelBLL {Image = image};
                    _pictureService.Create(image);
                    _unitOfWork.Save();
                }

                Mapper.Initialize(cfg => cfg.CreateMap<CreateOrderViewModel, OrderViewModelBLL>()
                    .ForMember("CategoryId", opt => opt.MapFrom(c => _categoryService.FindByName(c.Category).Id))
                    .ForMember("StatusId", opt => opt.MapFrom(c => 1))
                    .ForMember("AdminStatus", opt => opt.MapFrom(c => false))
                    .ForMember("UploadDate", opt => opt.MapFrom(c => DateTime.Now))
                    .ForMember("UserId", opt => opt.MapFrom(c => User.Identity.GetUserId<int>()))
                    .ForMember("PictureId", opt => opt.MapFrom(c => _pictureService.FindByBytes(picture.Image).Value))
                );
                OrderViewModelBLL orderDto = Mapper.Map<CreateOrderViewModel, OrderViewModelBLL>(order);

                OperationDetails operationDetails = _orderService.Create(orderDto);
                _unitOfWork.Save();
                if (operationDetails.Succedeed)
                    return RedirectToAction("Index");
                ModelState.AddModelError(operationDetails.Property, operationDetails.Message);
            }

            ViewBag.Category = new SelectList(_categoryService.GetAll(), "Name", "Name");
            ViewBag.DefaultPath =
                $"data: image/png; base64, {Convert.ToBase64String(System.IO.File.ReadAllBytes(Server.MapPath(DefaultImageName)))}";
            return View(order);
        }

        [Authorize(Roles = "admin")]
        public ActionResult Confirm(int id, int? currentCategoryId, OrderSorts ordersSort)
        {
            _orderService.ConfirmOrder(id);
            _unitOfWork.Save();
            return RedirectToAction("Index", new
            {
                newApplications = _orderService.GetAll().Count(o => !o.AdminStatus) > 0,
                categoryId = currentCategoryId,
                sort = ordersSort
            });
        }

        [Authorize(Roles = "admin")]
        public ActionResult Reject(int id, int? currentCategoryId, OrderSorts ordersSort)
        {
            var pictureId = _userService.FindById(id).PictureId;
            _orderService.DeleteOrder(id);
            _pictureService.Delete(pictureId);
            _unitOfWork.Save();
            return RedirectToAction("Index", new
            {
                newApplications = _orderService.GetAll().Count(o => !o.AdminStatus) > 0,
                categoryId = currentCategoryId,
                sort = ordersSort
            });
        }

        [Authorize]
        public ActionResult Delete(int id, bool? isMyOrdersPage, bool? isNewOrdersPage, int? currentCategoryId)
        {
            if (User.Identity.GetUserId<int>() != _orderService.Find(id).UserId)
                return View("~/Views/Error/Forbidden.cshtml");

            var orderDto = _orderService.Find(id);
            if (orderDto == null)
                return HttpNotFound();
            if (orderDto.UserId != User.Identity.GetUserId<int>())
                return RedirectToAction("Index");
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

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        [Authorize]
        public ActionResult DeleteConfirmed(int id, bool? isMyOrdersPage, bool? isNewOrdersPage, 
            int? currentCategoryId, OrderSorts ordersSort = OrderSorts.New)
        {
            if (User.Identity.GetUserId<int>() != _orderService.Find(id).UserId)
                return View("~/Views/Error/Forbidden.cshtml");

            var responses = _responseService.GetAllForOrder(id);
            if (responses.Any())
            {
                foreach (var response in responses)
                {
                    _responseService.Delete(response.Id);
                }
            }
            _orderService.DeleteOrder(id);

            var picture = _orderService.Find(id).PictureId;
            if (picture.HasValue)
                _pictureService.Delete(picture.Value);
            _unitOfWork.Save();

            return RedirectToAction("Index", new
            {
                newApplications = isNewOrdersPage,
                myOrders = isMyOrdersPage,
                categoryId = currentCategoryId,
                sort = ordersSort
            });
        }

        public ActionResult AutocompleteSearch(string orderName)
        {
            var orderNames = _orderService.GetAll().Select(o => o.Name).ToArray();
            var filteredNames = orderNames.Where(o => 
                o.IndexOf(orderName, StringComparison.InvariantCultureIgnoreCase) >= 0);
            return Json(filteredNames, JsonRequestBehavior.AllowGet);
        }
    }
}