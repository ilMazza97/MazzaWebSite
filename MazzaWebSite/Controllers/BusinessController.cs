using System.Web.Mvc;

namespace MazzaWebSite.Controllers
{
    public class BusinessController : Controller
    {
        private ICookie _cookie = null;
        public BusinessController()
        {
            _cookie = new Cookie();
        }

        public ActionResult CryptoProject(string @ref)
        {
            var response = HttpContext.Response;
            _cookie.CheckReferentCookie(Request, response, @ref);
            return View();
        }
    }
}