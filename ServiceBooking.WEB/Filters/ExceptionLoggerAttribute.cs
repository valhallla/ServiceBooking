using System;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using System.Web.Http;
using System.Web.Mvc;
using System.Web.Routing;
using System.Web.UI.WebControls;
using Microsoft.AspNet.Identity;
using Ninject;
using ServiceBooking.BLL.Interfaces;
using ServiceBooking.DAL.Interfaces;
using ServiceBooking.DAL.UnitOfWork.DTO;
using ServiceBooking.Util;

namespace ServiceBooking.WEB.Filters
{
    public class ExceptionLoggerAttribute : FilterAttribute, IExceptionFilter
    {
        private static IKernel _appKernel;
        private static IExceptionDetailService _exceptionDetailService;
        private static IUnitOfWork _unitOfWork;

        public void OnException(ExceptionContext filterContext)
        {
            string viewName = "Default";
            var statusCode = 0;
            Task<string> message = null;
            if (filterContext.Exception is HttpResponseException)
            {
                statusCode = (int) ((HttpResponseException) filterContext.Exception).Response.StatusCode;
                message = ((HttpResponseException) filterContext.Exception).Response.Content.ReadAsStringAsync();
            }
            switch (statusCode)
            {
                case 400:
                    viewName = "BadRequest";
                    break;
                case 403:
                    viewName = "Forbidden";
                    break;
                case 500:
                    viewName = "InternalServerError";
                    break;
                case 404:
                    viewName = "NotFound";
                    break;
                case 503:
                    viewName = "ServiceTemporarilyUnavailable";
                    break;
            }

            _appKernel = new StandardKernel(new ExceptionDetailNinjectModule());
            _exceptionDetailService = _appKernel.Get<IExceptionDetailService>();
            _unitOfWork = _appKernel.Get<IUnitOfWork>();

            ExceptionDetailViewModelBLL exceptionDetail = new ExceptionDetailViewModelBLL()
            {
                Guid = Guid.NewGuid(),
                Message = message?.Result ?? filterContext.Exception.Message,
                Url = filterContext.HttpContext.Request.RawUrl,
                UrlReferrere = filterContext.HttpContext.Request.UrlReferrer == null ? "" : filterContext.HttpContext.Request.UrlReferrer.AbsoluteUri,
                StackTrace = filterContext.Exception.StackTrace,
                Date = DateTime.Now,
                UserId = filterContext.HttpContext.User.Identity.GetUserId<int>()
            };

            _exceptionDetailService.Create(exceptionDetail);
            _unitOfWork.Save();

            var result = new ViewResult
            {
                ViewName = viewName,
                MasterName = string.Empty,
                ViewData = new ViewDataDictionary<Guid>(exceptionDetail.Guid)
            };
            filterContext.Result = result;
            filterContext.ExceptionHandled = true;
            filterContext.HttpContext.Response.Clear();
            filterContext.HttpContext.Response.TrySkipIisCustomErrors = true;
        }
    }
}