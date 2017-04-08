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
using ServiceBooking.WEB.Models;

namespace ServiceBooking.WEB.Controllers
{
    public class PerformersController : Controller
    {
        private static ICategoryService _categoryService;
        private static IUserService _userService;
        private static IUnitOfWork _unitOfWork;

        public PerformersController() : this( _categoryService, _userService, _unitOfWork)
        { }

        public PerformersController(ICategoryService categoryService,
            IUserService userService, IUnitOfWork unitOfWork)
        {
            _categoryService = categoryService;
            _userService = userService;
            _unitOfWork = unitOfWork;
        }

        public PerformersController(NinjectDependencyResolver resolver, 
            ICategoryService categoryService, IUserService userService, IUnitOfWork unitOfWork)
        {
            _categoryService = categoryService;
            _userService = userService;
            _unitOfWork = unitOfWork;
        }

        // GET: Performers
        public ActionResult Index(int? page, int? categoryId, string searchName,
            bool newApplications = false, PerformerSorts sort = PerformerSorts.New)
        {
            var performers = _userService.GetAll();

            return View();
        }
    }
}