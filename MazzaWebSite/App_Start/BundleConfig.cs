using System.Web.Optimization;

namespace MazzaWebSite
{
    public class BundleConfig
    {
        public static void RegisterBundles(BundleCollection bundles)
        {
            bundles.Add(new ScriptBundle("~/Scripts").Include(
                     "~/Scripts/bootstrap.min.js",
                     "~/Scripts/bootstrap-notify.min.js",
                     "~/Scripts/counterup.min.js",
                     "~/Scripts/wow.min.js",
                     "~/Scripts/waypoints.min.js",
                     "~/Scripts/easing.min.js",
                     "~/Scripts/main.min.js",
                     "~/Scripts/mobile-nav.js",
                     "~/Scripts/wb-function.js"
                     ));

            bundles.Add(new ScriptBundle("~/Scripts/jquery").Include(
                     "~/Scripts/jquery-3.3.1.min.js",
                     "~/Scripts/jquery.validate.min.js",
                     "~/Scripts/jquery.unobtrusive-ajax.min.js"
                     ));

            bundles.Add(new StyleBundle("~/css/css").Include(
                      "~/css/bootstrap.css",
                      "~/css/animate.min.css",
                      "~/css/font-awesome.min.css",
                      "~/css/ionicons.min.css",
                      "~/css/style.css"
                      ));

        }
    }
}
