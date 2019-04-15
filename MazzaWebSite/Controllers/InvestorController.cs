using MazzaWebSite.Models;
using Microsoft.AspNet.Identity;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
namespace MazzaWebSite.Controllers
{
    public class InvestorController : Controller
    {
        public ActionResult Dashboard()
        {
            using (var dbContext = new MazzaDbContext())
            {
                List<InvestmentReport> model = dbContext.Database
                .SqlQuery<InvestmentReport>(string.Format("CALL spREP_InvestmentReport ({0})", User.Identity.GetUserId<int>()))
                .ToList();
                return Request.IsAuthenticated && model != null ? View(model) : (ActionResult)RedirectToAction("Index", "Home");
            }
        }
        public ActionResult Prova()
        {
            return View();
        }
    }
}