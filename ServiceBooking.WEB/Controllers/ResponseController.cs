using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace ServiceBooking.WEB.Controllers
{
    public class ResponseController : Controller
    {
        // GET: Response
        public ActionResult Send(int orderId, int performerId)
        {
            return RedirectToAction("Details", "Orders", new {id = orderId});
        }
    }
}