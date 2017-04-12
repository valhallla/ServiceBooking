//using System.Web;
//using System.Web.Mvc;

//namespace ServiceBooking.WEB.Filters
//{
//    public class ForbiddenException : FilterAttribute, IExceptionFilter
//    {

//        public void OnException(ExceptionContext exceptionContext)
//        {
//            //if ((exceptionContext.Exception is System.Exception))
//            {
//                exceptionContext.Result = new RedirectResult("/Account/Forbidden.cshtml");
//                exceptionContext.ExceptionHandled = true;
//            }
//        }
//    }
//}