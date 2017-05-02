using System.Web.Mvc;
using ServiceBooking.BLL.Interfaces;
using ServiceBooking.DAL.Interfaces;
using ServiceBooking.WEB.Filters;

namespace ServiceBooking.WEB
{
    public class FilterConfig
    {
        private static IExceptionDetailService _exceptionDetailService;
        private static IUnitOfWork _unitOfWork;

        public static void RegisterGlobalFilters(GlobalFilterCollection filters)
        {
            filters.Add(new HandleErrorAttribute());
            filters.Add(new ExceptionLoggerAttribute());
        }
    }
}
