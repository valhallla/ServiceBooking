using System.Linq;
using System.Web.Mvc;
using ServiceBooking.BLL.Interfaces;

namespace ServiceBooking.WEB.Controllers
{
    public class HomeController : Controller
    {
        private static IOrderService _orderService;
        private static IUserService _userService;

        public HomeController() : this(_orderService, _userService)
        { }

        public HomeController(IOrderService orderService, IUserService userService)
        {
            _orderService = orderService;
            _userService = userService;
        }
        
        public ActionResult Index()
        {
            ViewBag.NewOrdersAmountString = string.Empty;
            ViewBag.NewPerformersAmountString = string.Empty;

            if (Request.IsAuthenticated && User.IsInRole("admin"))
            {
                var orders = _orderService.GetAll();
                if (orders == null)
                    return HttpNotFound();

                var newOrdersAmount = orders.Count(model => !model.AdminStatus);
                if (newOrdersAmount > 0)
                    ViewBag.NewOrdersAmountString = " + " + newOrdersAmount;

                var users = _userService.GetAll();
                if (users == null)
                    return HttpNotFound();

                var newPerformersAmount = users.Count(model => !model.AdminStatus && model.IsPerformer);
                if (newPerformersAmount > 0)
                    ViewBag.NewPerformersAmountString = " + " + newPerformersAmount;
            }

            return View();
        }

        public ActionResult Contact()
        {
            return View();
        }
    }
}