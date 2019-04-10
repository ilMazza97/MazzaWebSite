using MazzaWebSite.Models;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text.RegularExpressions;
using System.Web;
using System.Web.Mvc;

namespace MazzaWebSite.Controllers
{
    public class HomeController : Controller
    {
        MazzaDbContext db = new MazzaDbContext();
        private ICookie _cookie = null;
        public HomeController()
        {
            _cookie = new Cookie();
        }
        
        public ActionResult Index(string @ref)
        {
            //Paypal.SendMoney();

            //GetInstragramUrlImage(out List<string> instagramUrlImage);
            _cookie.CheckReferentCookie(Request, Response, @ref);
            UserEntityModel model = new UserEntityModel
            {
                Users = db.Users.ToList(),
                Deposits = db.Deposits.ToList(),
                Withdrawals = db.Withdrawals.ToList(),
                InstagramUrlImage = new List<string>
                {
                    "https://www.bucknell.edu/Images/Depts/Communication/Branding/ColorPalette-PaperWhite-200x200.jpg",
                    "https://www.bucknell.edu/Images/Depts/Communication/Branding/ColorPalette-PANTONE305C-200x200.jpg",
                    "https://www.bucknell.edu/Images/Depts/Communication/Branding/ColorPalette-PANTONE640C-200x200.jpg",
                    "https://www.bucknell.edu/Images/Depts/Communication/Branding/ColorPalette-BucknellBlue-200x200.jpg",
                    "https://www.bucknell.edu/Images/Depts/Communication/Branding/ColorPalette-PANTONE137C-200x200.jpg"
                }
            };
            return View(model);
        }


        public void SetCulture(string culture)
        {
            switch (Regex.Replace(culture, @"[^0-9a-zA-Zç\._]", string.Empty).ToLower())
            {
                case Language.Italiano:
                    culture = "it";
                    break;
                case Language.Inglese:
                    culture = "en";
                    break;
                case Language.Tedesco:
                    culture = "en";
                    break;
                case Language.Francese:
                    culture = "en";
                    break;
                default:
                    culture = "en";
                    break;
            }

            HttpCookie cookie = new HttpCookie("culture")
            {
                Expires = DateTime.Now.AddMonths(3),
                Value = culture
            };
            HttpContext.Response.Cookies.Add(cookie);    
        }


        private static void GetInstragramUrlImage(out List<string> instagramUrlImage)
        {
            instagramUrlImage = new List<string>();
            HttpWebRequest instaRequest = WebRequest.Create("https://api.instagram.com/v1/users/self/media/recent/?access_token=9976189344.dcdb0a1.f1df035ac2304a0bb593b83d544bbbdc") as HttpWebRequest;
            //optional
            HttpWebResponse instaResponse = instaRequest.GetResponse() as HttpWebResponse;
            string rawJson = new StreamReader(instaResponse.GetResponseStream()).ReadToEnd();

            JObject InstagramJsonFeeds = JObject.Parse(rawJson);
            // Loop through JObject and get the details
            foreach (var Property in InstagramJsonFeeds["data"])
                instagramUrlImage.Add(Property["images"]["thumbnail"]["url"].ToString());
        }

    }
}