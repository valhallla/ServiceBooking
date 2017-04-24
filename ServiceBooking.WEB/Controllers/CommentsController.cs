using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using AutoMapper;
using Microsoft.AspNet.Identity;
using ServiceBooking.BLL.DTO;
using ServiceBooking.BLL.Infrastructure;
using ServiceBooking.BLL.Interfaces;
using ServiceBooking.DAL.Interfaces;
using ServiceBooking.Util;
using ServiceBooking.WEB.Models;

namespace ServiceBooking.WEB.Controllers
{
    public class CommentsController : Controller
    {
        private static ICommentService _commentService;
        private static IUserService _userService;
        private static IUnitOfWork _unitOfWork;

        public CommentsController() : this( _commentService, _userService, _unitOfWork) { }

        public CommentsController(ICommentService commentService, IUserService userService, IUnitOfWork unitOfWork)
        {
            _commentService = commentService;
            _userService = userService;
            _unitOfWork = unitOfWork;
        }

        // POST: Comments/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "user")]
        public ActionResult Send(CreateCommentViewModel comment)
        {
            bool commentIsEmpty = comment.Text == null || comment.Text.Equals(string.Empty);

            Mapper.Initialize(cfg => cfg.CreateMap<CreateCommentViewModel, CommentViewModelBLL>()
                   .ForMember("Date", opt => opt.MapFrom(c => DateTime.Now))
                   .ForMember("CustomerId", opt => opt.MapFrom(c => User.Identity.GetUserId<int>()))
                   .ForMember("Rating", opt => opt.MapFrom(c => c.Rating.ToCharArray().Where(r => r == '★').Count()))
               );
            CommentViewModelBLL commentDto = Mapper.Map<CreateCommentViewModel, CommentViewModelBLL>(comment);

            if (ModelState.IsValid)
            {
                OperationDetails operationDetails = _commentService.Create(commentDto);
                _unitOfWork.Save();
                if (!operationDetails.Succedeed)
                    ModelState.AddModelError(operationDetails.Property, operationDetails.Message);

                var user = _userService.FindById(comment.PerformerId);
                if (user.CommentsBll != null)
                    user.Rating = (int)Math.Round((float)(user.CommentsBll.Select(c => c.Rating).Sum() / user.CommentsBll.Count));
                operationDetails = _userService.Update(user);
                if (!operationDetails.Succedeed)
                    ModelState.AddModelError(operationDetails.Property, operationDetails.Message);
            }
            return RedirectToAction("Details", "Performers", new { id = comment.PerformerId, emptyComment = commentIsEmpty });
        }
    }
}