using System.Web;
namespace MazzaWebSite.Controllers
{
    public interface ICookie
    {
        HttpCookie GetCookieLanguage(HttpRequestBase request, HttpResponseBase response);
        void CheckReferentCookie(HttpRequestBase request, HttpResponseBase Response,string key);

    }
}