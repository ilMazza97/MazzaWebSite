using System;
using System.Threading;
using System.Web;

namespace MazzaWebSite.Controllers
{
    public class Cookie : ICookie
    {
        public Cookie() { }

        public HttpCookie GetCookieLanguage(HttpRequestBase request, HttpResponseBase response)
        {
            string key = "OMP-Culture";
            var cookie = request.Cookies[key];
            if (cookie == null)
            {
                cookie = new HttpCookie(key)
                {
                    Expires = DateTime.Now.AddMonths(3),
                    Value = Thread.CurrentThread.CurrentCulture.ToString()
                };
                response.Cookies.Set(cookie);
            }
            return cookie;
        }
        public void CheckReferentCookie(HttpRequestBase request, HttpResponseBase response, string key)
        {
            HttpCookie cookie = request.Cookies["OMP-referentcode"];
            if (cookie == null || string.IsNullOrEmpty(cookie.Value))
            {
                cookie = new HttpCookie("OMP-referentcode")
                {
                    Expires = DateTime.Now.AddMonths(3),
                    Value = key ?? string.Empty
                };
                response.Cookies.Set(cookie);
            }
        }
    }
}