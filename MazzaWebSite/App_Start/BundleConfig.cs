using System.Web.Optimization;

namespace MazzaWebSite
{
    public class BundleConfig
    {
        public static void RegisterBundles(BundleCollection bundles)
        {
            bundles.Add(new ScriptBundle("~/Scripts/bootstrap").Include(
                     "~/Scripts/bootstrap.min.js",
                     "~/Scripts/bootstrap-dialog.min.js",
                     "~/Scripts/bootstrap-notify.js"));
            bundles.Add(new ScriptBundle("~/Scripts/jquery").Include(
                     "~/Scripts/jquery-{version}.js"));
            bundles.Add(new ScriptBundle("~/Scripts/jqueryval").Include(
                     "~/Scripts/jquery.validate.min.js",
                     "~/Scripts/jquery.unobtrusive-ajax.min.js"));
            bundles.Add(new ScriptBundle("~/lib/mobile-nav").Include(
                      "~/lib/mobile-nav/mobile-nav.js"));
            bundles.Add(new ScriptBundle("~/lib/easing").Include(
                      "~/lib/easing/easing.min.js"));
            bundles.Add(new ScriptBundle("~/lib/wow").Include(
                      "~/lib/wow/wow.min.js"));
            bundles.Add(new ScriptBundle("~/lib/waypoints").Include(
                      "~/lib/waypoints/waypoints.min.js"));
            bundles.Add(new ScriptBundle("~/lib/counterup").Include(
                      "~/lib/counterup/counterup.min.js"));
            bundles.Add(new ScriptBundle("~/js").Include(
                      "~/js/main.js",
                      "~/js/wb-function.js"));

            bundles.Add(new StyleBundle("~/css/css").Include(
                      "~/css/style.css"));
            bundles.Add(new StyleBundle("~/lib/font-awesome/css").Include(
                      "~/lib/font-awesome/css/font-awesome.min.css"));
            bundles.Add(new StyleBundle("~/lib/animate").Include(
                      "~/lib/animate/animate.min.css"));
            bundles.Add(new StyleBundle("~/lib/ionicons/css").Include(
                      "~/lib/ionicons/css/ionicons.min.css"));
            bundles.Add(new StyleBundle("~/lib/bootstrap/css").Include(
                      "~/lib/bootstrap/css/bootstrap.css"));
            
        }
    }
}
