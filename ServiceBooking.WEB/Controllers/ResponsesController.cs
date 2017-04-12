using System;
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
        [Authorize(Roles = "user")]
        public ActionResult Send(CreateResponseViewModel response)
        {
            bool responseIsEmpty = response.Text == null || response.Text.Equals(string.Empty);

            Mapper.Initialize(cfg => cfg.CreateMap<CreateResponseViewModel, ResponseViewModelBLL>()
               .ForMember("Date", opt => opt.MapFrom(c => DateTime.Now))
               .ForMember("PerformerId", opt => opt.MapFrom(c => User.Identity.GetUserId<int>())));
            ResponseViewModelBLL responseDto = Mapper.Map<CreateResponseViewModel, ResponseViewModelBLL>(response);

            if (ModelState.IsValid)
            {
                OperationDetails operationDetails = _responseService.Create(responseDto);
                if (!operationDetails.Succedeed)
                    ModelState.AddModelError(operationDetails.Property, operationDetails.Message);
            }
            return RedirectToAction("Details", "Orders", new { id = response.OrderId, emptyResponse = responseIsEmpty});
        }
    }
}