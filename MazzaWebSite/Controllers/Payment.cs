using System.Web.Mvc;
namespace MazzaWebSite.Controllers
{
    public class PaymentController : Controller
    {
        public ActionResult Skrill()
        {
            return View();
        }
        public ActionResult AdvancedCash()
        {
            return View();
        }
    }
}