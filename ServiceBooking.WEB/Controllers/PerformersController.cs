using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using ServiceBooking.BLL.Interfaces;
using ServiceBooking.DAL.Interfaces;
using ServiceBooking.Util;
using AutoMapper;
using Microsoft.AspNet.Identity;
using PagedList.Mvc;
using PagedList;
using ServiceBooking.BLL.DTO;
using ServiceBooking.WEB.Models;

namespace ServiceBooking.WEB.Controllers
{
    public class PerformersController : Controller
    {
        private static ICategoryService _categoryService;
        private static ICommentService _commentService;
        private static IUserService _userService;
        private static IUnitOfWork _unitOfWork;

        public PerformersController() : this(_categoryService, 
            _commentService, _userService, _unitOfWork) { }

        public PerformersController(ICategoryService categoryService, 
            ICommentService commentService, IUserService userService, IUnitOfWork unitOfWork)
        {
            _categoryService = categoryService;
            _commentService = commentService;
            _userService = userService;
            _unitOfWork = unitOfWork;
        }

        // GET: Performers
        public ActionResult Index(int? page, int? categoryId, string searchName,
            bool newApplications = false, PerformerSorts sort = PerformerSorts.New)
        {
            var perfomersDto = _userService.GetAll().ToList().Where(p => p.IsPerformer);
            if (newApplications && !perfomersDto.Any(o => !o.AdminStatus))
                ViewBag.AdminMessage = "No new performers";

            ViewBag.NewPerformerssAmountString = string.Empty;
            var newPerformersAmount = perfomersDto.Count(model => !model.AdminStatus);
            if (newPerformersAmount > 0)
                ViewBag.NewPerformersAmountString = " + " + newPerformersAmount;

            ViewBag.IsNewPage = false;
            if (newApplications)
            {
                perfomersDto = perfomersDto.Where(model => !model.AdminStatus);
                ViewBag.IsNewPage = true;
            }
            else
                perfomersDto = perfomersDto.Where(model => model.AdminStatus);

            if (categoryId != null)
            {
                perfomersDto = perfomersDto.Where(o => o.CategoriesBll.Select(c => c.Id).Contains(categoryId.Value));
                ViewBag.CurrentCategoryId = categoryId;
            }

            var categoriesDto = _categoryService.GetAll().ToList();
            Mapper.Initialize(cfg => cfg.CreateMap<CategoryViewModelBLL, CategoryViewModel>()
                .ForMember("ItemsAmount", opt => opt.MapFrom(c => c.Performers.Count(o =>
                        o.IsPerformer && o.AdminStatus != newApplications
                )))
            );
            var categories = Mapper.Map<List<CategoryViewModelBLL>, List<CategoryViewModel>>(categoriesDto);
            ViewBag.CategoriesList = categories;

            if (searchName != null)
                perfomersDto = perfomersDto.Where(o => o.Name.Contains(searchName) || 
                o.Surname.Contains(searchName) || o.Company.Contains(searchName));
            ViewBag.SearchName = searchName;

            switch (sort)
            {
                case PerformerSorts.New:
                    perfomersDto = perfomersDto.OrderByDescending(o => o.RegistrationDate);
                    break;
                case PerformerSorts.Best:
                    perfomersDto = perfomersDto.OrderBy(o => o.Rating);
                    break;
            }
            ViewBag.Sort = sort;
            ViewBag.ItemsAmount = perfomersDto.Count();

            Mapper.Initialize(cfg => cfg.CreateMap<ClientViewModelBLL, IndexPerformerViewModel>()
                .ForMember("Name", opt => opt.MapFrom(c => c.Surname + " " + c.Name))
                .ForMember("Category", opt => opt.MapFrom(c => c.CategoriesBll.First().Name + " + " + (c.CategoriesBll.Count - 1).ToString() + " more")));
            var performers = Mapper.Map<IEnumerable<ClientViewModelBLL>, List<IndexPerformerViewModel>>(perfomersDto);

            var performerNames = performers.Select(o => o.Name).ToArray();
            var filteredNames = performerNames.Where(o => o.IndexOf(searchName, StringComparison.InvariantCultureIgnoreCase) >= 0);
            ViewBag.Names = Json(filteredNames, JsonRequestBehavior.AllowGet);

            int pageSize = 3;
            int pageNumber = (page ?? 1);
            return View(performers.ToPagedList(pageNumber, pageSize));
        }

        // GET: Performer/Details/5
        public ActionResult Details(int id, int? categoryId, bool? newApplications, PerformerSorts sort = PerformerSorts.New, bool emptyComment = false)
        {
            var commentsDto = _commentService.GetAllForPerformer(id);
            Mapper.Initialize(cfg => cfg.CreateMap<CommentViewModelBLL, IndexCommentViewModel>()
                .ForMember("CustomerName", opt => opt.MapFrom(c => _userService.FindById(c.CustomerId).Surname
                    + " " + _userService.FindById(c.CustomerId).Name))
                );
            var comments = Mapper.Map<IEnumerable<CommentViewModelBLL>, List<IndexCommentViewModel>>(commentsDto);

            var performerDto = _userService.FindById(id);
            if (performerDto == null)
                return HttpNotFound();

            var categoriesDto =
                _categoryService.GetAll().Where(c => c.Performers.Select(p => p.Id).Contains(id)).ToList();
            Mapper.Initialize(cfg => cfg.CreateMap<CategoryViewModelBLL, CategoryViewModel>()
                .ForMember("ItemsAmount", opt => opt.MapFrom(c => c.Performers.Count(o =>
                        o.IsPerformer && o.AdminStatus != newApplications
                )))
            );
            var categories = Mapper.Map<List<CategoryViewModelBLL>, List<CategoryViewModel>>(categoriesDto);

            Mapper.Initialize(cfg => cfg.CreateMap<ClientViewModelBLL, DetailsPerformerViewModel>()
                .ForMember("Categories", opt => opt.MapFrom(c => categories))
                .ForMember("Comments", opt => opt.MapFrom(c => comments))
                .ForMember("Name", opt => opt.MapFrom(c => c.Surname + " " + c.Name))
                .ForMember("CustomersId", opt => opt.MapFrom(c => c.CommentsBll.Select(m => m.CustomerId)))
                );
            DetailsPerformerViewModel performer = Mapper.Map<ClientViewModelBLL, DetailsPerformerViewModel>(performerDto);

            ViewBag.CurrentCategoryId = categoryId;
            ViewBag.IsNewPage = newApplications;
            ViewBag.CommentIsEmpty = emptyComment;
            ViewBag.Sort = sort;
            ViewBag.ShowContacts = _userService.FindById(User.Identity.GetUserId<int>())
                .OrdersBll.SelectMany(o => o.Responses)
                .Count(o => o.UserId == id) != 0 ||
                User.Identity.GetUserId<int>() == id;
            ViewBag.Rating = new SelectList(new List<string>() { "☆☆☆☆☆", "★☆☆☆☆", "★★☆☆☆", "★★★☆☆", "★★★★☆", "★★★★★" });

            return View(performer);
        }

        [Authorize(Roles = "admin")]
        public ActionResult Confirm(int id, int? currentCategoryId, PerformerSorts performersSort)
        {
            _userService.ConfirmPerformer(id);
            return RedirectToAction("Index", new
            {
                newApplications = _userService.GetAll().Count(u => !u.AdminStatus && u.IsPerformer) > 0,
                categoryId = currentCategoryId,
                sort = performersSort
            });
        }

        [Authorize(Roles = "admin")]
        public ActionResult Reject(int id, int? currentCategoryId, PerformerSorts performersSort)
        {
            _userService.RejectPerformer(id);
            return RedirectToAction("Index", new
            {
                newApplications = _userService.GetAll().Count(u => !u.AdminStatus && u.IsPerformer) > 0,
                categoryId = currentCategoryId,
                sort = performersSort
            });
        }

        [Authorize(Roles = "user")]
        public ActionResult Edit(string message = "", string company = null,
            string info = null, string phoneNumber = null)
        {
            if (!(bool)Session["isPerformer"] || !(bool)Session["adminStatus"])
                return View("~/Views/Error/Forbidden.cshtml");

            var allCategoriesDto = _categoryService.GetAll().ToList();
            Mapper.Initialize(cfg => cfg.CreateMap<CategoryViewModelBLL, CategoryViewModel>());
            var allCategories = Mapper.Map<List<CategoryViewModelBLL>, List<CategoryViewModel>>(allCategoriesDto);
            ViewBag.Categories = allCategories;

            var performerCategoriesDto =
                allCategoriesDto.Where(c => c.Performers.Select(p => p.Id).Contains(User.Identity.GetUserId<int>())).ToList();
            Mapper.Initialize(cfg => cfg.CreateMap<CategoryViewModelBLL, CategoryViewModel>()
                .ForMember("ItemsAmount", opt => opt.MapFrom(c => c.Performers.Count(o => o.IsPerformer && o.AdminStatus
                )))
            );
            var performerCategories = Mapper.Map<List<CategoryViewModelBLL>, List<CategoryViewModel>>(performerCategoriesDto);

            var performerDto = _userService.FindById(User.Identity.GetUserId<int>());
            Mapper.Initialize(cfg => cfg.CreateMap<ClientViewModelBLL, EditPerformerViewModel>()
                .ForMember("Name", opt => opt.MapFrom(c => c.Surname + " " + c.Name))
                .ForMember("Categories", opt => opt.MapFrom(c => performerCategories))
                .ForMember("Company", opt => opt.MapFrom(c => company ?? c.Company))
                .ForMember("PhoneNumber", opt => opt.MapFrom(c => phoneNumber ?? c.PhoneNumber))
                .ForMember("Info", opt => opt.MapFrom(c => info ?? c.Info))
                );
            var performer = Mapper.Map<ClientViewModelBLL, EditPerformerViewModel>(performerDto);
            ViewBag.Message = message;

            return View("Edit", performer);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "user")]
        public ActionResult Edit(EditPerformerViewModel model, int[] selectedCategories)
        {
            if (!(bool)Session["isPerformer"] || !(bool)Session["adminStatus"])
                return View("~/Views/Error/Forbidden.cshtml");

            if (!ModelState.IsValid)
                return View(model);

            if (ReferenceEquals(selectedCategories, null))
                return RedirectToAction("Edit", new
                {
                    message = "At least one category is required",
                    company = model.Company,
                    info = model.Info,
                    phoneNumber = model.PhoneNumber
                });

            ClientViewModelBLL client = _userService.FindById(User.Identity.GetUserId<int>());
            Mapper.Initialize(cfg => cfg.CreateMap<EditPerformerViewModel, ClientViewModelBLL>()
                .ForMember("Name", opt => opt.MapFrom(c => client.Name))
                .ForMember("CategoriesBll", opt => opt.MapFrom(c => c.Categories))
                );
            Mapper.Map(model, client);
            _userService.Update(client, selectedCategories);

            return RedirectToAction("Details", new {id = client.Id});
        }
    }
}