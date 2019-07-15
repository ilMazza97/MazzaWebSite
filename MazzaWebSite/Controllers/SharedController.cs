using System.Web.Mvc;

namespace MazzaWebSite.Controllers
{
    public class SharedController : Controller
    {
        public ActionResult UnderCostruction()
        {
            return View();
        }
        public ActionResult success()
        {
            return View();
        }
        public ActionResult fail()
        {
            return View();
        }
        public ActionResult status()
        {
            return View();
        }
    }
}