using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace ServiceBooking.WEB.Controllers
{
    public class ErrorController : Controller
    {
        public ActionResult NotFound()
        {
            Response.StatusCode = 404;
            return View();
        }

        public ActionResult InternalServerError()
        {
            Response.StatusCode = 500;
            return View();
        }

        public ActionResult Forbidden()
        {
            Response.StatusCode = 403;
            return View();
        }

        public ActionResult ServiceTemporarilyUnavailable()
        {
            Response.StatusCode = 503;
            return View();
        }

        public ActionResult Default()
        {
            Response.StatusCode = 500;
            return View();
        }
    }
}