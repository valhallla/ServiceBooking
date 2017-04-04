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
using ServiceBooking.BLL.DTO;
using ServiceBooking.BLL.Infrastructure;
using ServiceBooking.WEB.Models;

namespace ServiceBooking.WEB.Controllers
{
    public class ResponsesController : Controller
    {
        private static IResponseService _responseService;
        private static IUnitOfWork _unitOfWork;

        public ResponsesController() : this( _responseService, _unitOfWork) { }

        public ResponsesController(IResponseService responseService, IUnitOfWork unitOfWork)
        {
            _responseService = responseService;
            _unitOfWork = unitOfWork;
        }

        public ResponsesController(NinjectDependencyResolver resolver, IResponseService responseService, IUnitOfWork unitOfWork)
        {
            _responseService = responseService;
            _unitOfWork = unitOfWork;
        }

        // POST: Responses/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Send(CreateResponseViewModel response)
        {
            Mapper.Initialize(cfg => cfg.CreateMap<CreateResponseViewModel, ResponseViewModel>()
               .ForMember("Date", opt => opt.MapFrom(c => DateTime.Now))
               .ForMember("PerformerId", opt => opt.MapFrom(c => User.Identity.GetUserId<int>())));
            ResponseViewModel responseDto = Mapper.Map<CreateResponseViewModel, ResponseViewModel>(response);

            if (ModelState.IsValid)
            {
                OperationDetails operationDetails = _responseService.Create(responseDto);
                if (operationDetails.Succedeed)
                    return RedirectToAction("Details", "Orders", new { id = response.OrderId });
                ModelState.AddModelError(operationDetails.Property, operationDetails.Message);
            }
            return View("~/Views/Home/Index.cshtml");
        }
    }
}