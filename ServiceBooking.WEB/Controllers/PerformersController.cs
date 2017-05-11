using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using ServiceBooking.BLL.Interfaces;
using ServiceBooking.DAL.Interfaces;
using AutoMapper;
using Microsoft.AspNet.Identity;
using PagedList;
using ServiceBooking.BLL.DTO;
using ServiceBooking.DAL.UnitOfWork.DTO;
using ServiceBooking.WEB.Models;

namespace ServiceBooking.WEB.Controllers
{
    public class PerformersController : Controller
    {
        private static ICategoryService _categoryService;
        private static ICommentService _commentService;
        private static IUserService _userService;
        private static IPictureService _pictureService;
        private static IUnitOfWork _unitOfWork;

        private const string DefaultImageName = @"~/Content/default-user.png";

        public PerformersController() : this(_categoryService, 
            _commentService, _userService, _pictureService, _unitOfWork) { }

        public PerformersController(ICategoryService categoryService, ICommentService commentService, 
            IUserService userService, IPictureService pictureService, IUnitOfWork unitOfWork)
        {
            _categoryService = categoryService;
            _commentService = commentService;
            _userService = userService;
            _pictureService = pictureService;
            _unitOfWork = unitOfWork;
        }

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

            ViewBag.ItemsAmount = perfomersDto.Count();
            var categoriesDto = _categoryService.GetAll().ToList();
            Mapper.Initialize(cfg => cfg.CreateMap<CategoryViewModelBLL, CategoryViewModel>()
                .ForMember("ItemsAmount", opt => opt.MapFrom(c => c.Performers.Count(o =>
                        o.IsPerformer && o.AdminStatus != newApplications
                )))
            );
            var categories = Mapper.Map<List<CategoryViewModelBLL>, List<CategoryViewModel>>(categoriesDto);
            ViewBag.CategoriesList = categories;

            if (categoryId != null)
            {
                perfomersDto = perfomersDto.Where(o => o.CategoriesBll.Select(c => c.Id).Contains(categoryId.Value));
                ViewBag.CurrentCategoryId = categoryId;
            }

            if (searchName != null)
                perfomersDto = perfomersDto.Where(o => o.Name.ToLower().Contains(searchName.ToLower()) || 
                o.Surname.ToLower().Contains(searchName.ToLower()) || o.Company.ToLower().Contains(searchName.ToLower()));
            ViewBag.SearchName = searchName;
            if (!perfomersDto.Any())
                ViewBag.SearchMessage = "No performers found";

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

            Mapper.Initialize(cfg => cfg.CreateMap<ClientViewModelBLL, IndexPerformerViewModel>()
                .ForMember("Name", opt => opt.MapFrom(c => c.Surname + " " + c.Name))
                .ForMember("Category", opt => opt.MapFrom(c => c.CategoriesBll.First().Name + 
                    (c.CategoriesBll.Count > 1 ? " + " + (c.CategoriesBll.Count - 1).ToString() + " more" : string.Empty)))
                .ForMember("Image", opt => opt.MapFrom(c => c.PictureId == null
                    ? System.IO.File.ReadAllBytes(Server.MapPath(DefaultImageName))
                    : _pictureService.FindById(c.PictureId.Value).Image))
            );
            var performers = Mapper.Map<IEnumerable<ClientViewModelBLL>, List<IndexPerformerViewModel>>(perfomersDto);

            var performerNames = performers.Select(o => o.Name).ToArray();
            var filteredNames = performerNames.Where(o => o.IndexOf(searchName, StringComparison.InvariantCultureIgnoreCase) >= 0);
            ViewBag.Names = Json(filteredNames, JsonRequestBehavior.AllowGet);

            int pageSize = 3;
            int pageNumber = (page ?? 1);
            return View(performers.ToPagedList(pageNumber, pageSize));
        }

        public ActionResult Details(int id, int? categoryId, bool? newApplications, 
            PerformerSorts sort = PerformerSorts.New, bool emptyComment = false)
        {
            var performerDto = _userService.FindById(id);
            if (performerDto == null)
                return HttpNotFound();

            var commentsDto = _commentService.GetAllForPerformer(id);
            Mapper.Initialize(cfg => cfg.CreateMap<CommentViewModelBLL, IndexCommentViewModel>()
                .ForMember("CustomerName", opt => opt.MapFrom(c => _userService.FindById(c.CustomerId).Surname
                    + " " + _userService.FindById(c.CustomerId).Name))
                .ForMember("Image", opt => opt.MapFrom(c => _userService.FindById(c.CustomerId).PictureId == null
                    || (_userService.FindById(c.CustomerId).PictureId != null 
                    && (!_userService.FindById(c.CustomerId).AdminStatus || !_userService.FindById(c.CustomerId).IsPerformer))
                    ? System.IO.File.ReadAllBytes(Server.MapPath(DefaultImageName))
                    : _pictureService.FindById(_userService.FindById(c.CustomerId).PictureId.Value).Image))
            );
            var comments = Mapper.Map<IEnumerable<CommentViewModelBLL>, List<IndexCommentViewModel>>(commentsDto);
            var currentCustomer = _userService.FindById(User.Identity.GetUserId<int>());
            ViewBag.CustomerImage = currentCustomer?.PictureId == null || (currentCustomer?.PictureId != null 
                && (!currentCustomer.AdminStatus || !currentCustomer.IsPerformer))
                ? System.IO.File.ReadAllBytes(Server.MapPath(DefaultImageName))
                : _pictureService.FindById(_userService.FindById(User.Identity.GetUserId<int>()).PictureId.Value).Image;

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
                .ForMember("Image", opt => opt.MapFrom(c => c.PictureId == null 
                    ? System.IO.File.ReadAllBytes(Server.MapPath(DefaultImageName))
                    : _pictureService.FindById(c.PictureId.Value).Image))
            );
            DetailsPerformerViewModel performer = Mapper.Map<ClientViewModelBLL, DetailsPerformerViewModel>(performerDto);

            ViewBag.CurrentCategoryId = categoryId;
            ViewBag.IsNewPage = newApplications;
            ViewBag.CommentIsEmpty = emptyComment;
            ViewBag.Sort = sort;
            ViewBag.ShowContacts = Request.IsAuthenticated && (_userService.FindById(User.Identity.GetUserId<int>())
                .OrdersBll.SelectMany(o => o.Responses)
                .Count(o => o.UserId == id) != 0 ||
                User.Identity.GetUserId<int>() == id);
            ViewBag.Rating = new SelectList(new List<string>() { "☆☆☆☆☆", "★☆☆☆☆", "★★☆☆☆", "★★★☆☆", "★★★★☆", "★★★★★" });

            return View(performer);
        }

        [Authorize(Roles = "admin")]
        public ActionResult Confirm(int id, int? currentCategoryId, PerformerSorts performersSort)
        {
            _userService.ConfirmPerformer(id);
            _unitOfWork.Save();
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
            var pictureId = _userService.FindById(id).PictureId;
            _userService.RejectPerformer(id);
            _pictureService.Delete(pictureId);
            _unitOfWork.Save();
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
                .ForMember("Image", opt => opt.MapFrom(c => c.PictureId == null
                    ? System.IO.File.ReadAllBytes(Server.MapPath(DefaultImageName))
                    : _pictureService.FindById(c.PictureId.Value).Image))
            );
            var performer = Mapper.Map<ClientViewModelBLL, EditPerformerViewModel>(performerDto);
            ViewBag.Message = message;
            ViewBag.DefaultPath = $"data: image/png; base64, {Convert.ToBase64String(System.IO.File.ReadAllBytes(Server.MapPath(DefaultImageName)))}";
            if (!ReferenceEquals(performerDto.PictureId, null))
                ViewBag.CloseButtonStyle = string.Empty;

            return View("Edit", performer);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "user")]
        public ActionResult Edit(EditPerformerViewModel model, int[] selectedCategories, HttpPostedFileBase loadImage)
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

            PictureViewModelBLL picture = null;
            if (!ReferenceEquals(loadImage, null))
            {
                byte[] image;
                using (var binaryReader = new BinaryReader(loadImage.InputStream))
                {
                    image = binaryReader.ReadBytes(loadImage.ContentLength);
                }
                picture = new PictureViewModelBLL { Image = image };

                _pictureService.Create(image);
                _unitOfWork.Save();
            }

            ClientViewModelBLL client = _userService.FindById(User.Identity.GetUserId<int>());
            Mapper.Initialize(cfg => cfg.CreateMap<EditPerformerViewModel, ClientViewModelBLL>()
                .ForMember("Name", opt => opt.MapFrom(c => client.Name))
                .ForMember("CategoriesBll", opt => opt.MapFrom(c => c.Categories))
                .ForMember("PictureId", opt => opt.MapFrom(c => _pictureService.FindByBytes(picture.Image).Value))
                );
            Mapper.Map(model, client);
            _userService.Update(client, selectedCategories);
            _unitOfWork.Save();

            return RedirectToAction("Details", new {id = client.Id});
        }
    }
}