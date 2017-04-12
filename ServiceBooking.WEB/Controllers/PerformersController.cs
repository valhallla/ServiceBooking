using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using ServiceBooking.BLL.Interfaces;
using ServiceBooking.DAL.Interfaces;
using ServiceBooking.Util;
using AutoMapper;
using PagedList.Mvc;
using PagedList;
using ServiceBooking.BLL.DTO;
using ServiceBooking.WEB.Models;

namespace ServiceBooking.WEB.Controllers
{
    public class PerformersController : Controller
    {
        private static ICategoryService _categoryService;
        private static IUserService _userService;
        private static IUnitOfWork _unitOfWork;

        public PerformersController() : this(_categoryService, _userService, _unitOfWork)
        {
        }

        public PerformersController(ICategoryService categoryService,
            IUserService userService, IUnitOfWork unitOfWork)
        {
            _categoryService = categoryService;
            _userService = userService;
            _unitOfWork = unitOfWork;
        }

        // GET: Performers
        public ActionResult Index(int? page, int? categoryId, string searchName,
            bool newApplications = false, PerformerSorts sort = PerformerSorts.New)
        {
            var usersDto = _userService.GetAll();
            if (usersDto == null)
                return HttpNotFound();

            ViewBag.NewPerformerssAmountString = string.Empty;
            var newPerformersAmount = usersDto.Count(model => !model.AdminStatus && model.IsPerformer);
            if (newPerformersAmount > 0)
                ViewBag.NewPerformersAmountString = " + " + newPerformersAmount;

            ViewBag.IsNewPerformersPage = false;
            if (newApplications)
            {
                usersDto = usersDto.Where(model => !model.AdminStatus && model.IsPerformer);
                ViewBag.IsNewPerformersPage = true;
            }
            else
                usersDto = usersDto.Where(model => model.AdminStatus && model.IsPerformer);

            if (categoryId != null)
            {
                usersDto = usersDto.Where(o => o.Categories.Select(c => c.Id).Contains(categoryId.Value));
                ViewBag.CurrentCategoryId = categoryId;
            }

            //var categoriesDto = _categoryService.GetAll().ToList();
            //Mapper.Initialize(cfg => cfg.CreateMap<CategoryViewModelBLL, CategoryViewModel>()
            //    .ForMember("ItemsAmount", opt => opt.MapFrom(c => c.Performers.Count(o =>
            //            newApplications ? o.AdminStatus != newApplications :
            //            (myOrders ? o.UserId == User.Identity.GetUserId<int>() : o.AdminStatus)
            //    )))
            //);
            //var categories = Mapper.Map<List<CategoryViewModelBLL>, List<CategoryViewModel>>(categoriesDto);
            //ViewBag.CategoriesList = categories;

            if (searchName != null)
                usersDto = usersDto.Where(o => o.Name.Contains(searchName));
            ViewBag.SearchName = searchName;

            switch (sort)
            {
                case PerformerSorts.New:
                    usersDto = usersDto.OrderByDescending(o => o.RegistrationDate);
                    break;
                case PerformerSorts.Best:
                    usersDto = usersDto.OrderBy(o => o.Rating);
                    break;
            }
            ViewBag.Sort = sort;

            //Mapper.Initialize(cfg => cfg.CreateMap<ClientViewModelBLL, IndexPerformerViewModel>()
            //    .ForMember("Category", opt => opt.MapFrom(c => _categoryService.FindById(c.CategoryId).Name))
            //    .ForMember("Status", opt => opt.MapFrom(c => _statusService.FindById(c.StatusId).Value)));
            //var orders = Mapper.Map<IEnumerable<OrderViewModelBLL>, List<IndexOrderViewModel>>(usersDto);

            //var orderNames = _orderService.GetAll().Select(o => o.Name).ToArray();
            //var filteredNames = orderNames.Where(o => o.IndexOf(searchName, StringComparison.InvariantCultureIgnoreCase) >= 0);
            //ViewBag.Names = Json(filteredNames, JsonRequestBehavior.AllowGet);

            //    int pageSize = 3;
            //    int pageNumber = (page ?? 1);
            //    return View(orders.ToPagedList(pageNumber, pageSize));
            //}
            return View();
        }
    }
}