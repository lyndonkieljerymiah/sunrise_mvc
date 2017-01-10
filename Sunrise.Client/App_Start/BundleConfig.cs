using System.Web;
using System.Web.Optimization;

namespace Sunrise.Client
{
    public class BundleConfig
    {
        // For more information on bundling, visit http://go.microsoft.com/fwlink/?LinkId=301862
        public static void RegisterBundles(BundleCollection bundles)
        {
            bundles.Add(new ScriptBundle("~/bundles/jquery").Include(
                        "~/Scripts/libs/jquery/jquery-{version}.js"));

            bundles.Add(new ScriptBundle("~/bundles/jqueryval").Include(
                        "~/Scripts/libs/jquery/jquery.validate*"));


            // Use the development version of Modernizr to develop with and learn from. Then, when you're
            // ready for production, use the build tool at http://modernizr.com to pick only the tests you need.
            bundles.Add(new ScriptBundle("~/bundles/modernizr").Include(
                        "~/Scripts/libs/other/modernizr-*"));

            bundles.Add(new ScriptBundle("~/bundles/ui-bootstrap").Include(
                      "~/Scripts/libs/angular-ui/ui-bootstrap-tpls.js",
                      "~/Scripts/libs/respond.js"));

            bundles.Add(new ScriptBundle("~/bundles/angular").Include(
                      "~/Scripts/libs/angularjs/angular.js",
                      "~/Scripts/libs/angularjs/angular-route.js",
                      "~/Scripts/libs/angularjs/angular-animate.js",
                      "~/Scripts/libs/toaster/toaster.js"));

            bundles.Add(new ScriptBundle("~/bundles/app").Include(
                "~/Scripts/app/helper/extension.js",
                "~/Scripts/app/app-bootstrap.js",
                "~/Scripts/app/helper/dialog.js",
                "~/Scripts/app/helper/router.js"));


            bundles.Add(new StyleBundle("~/Content/css").Include(
                      "~/Content/font-awesome.css",
                      "~/Content/bootstrap-slate.css",
                      "~/Content/toaster.css",
                      "~/Content/default.css"));

            bundles.Add(new StyleBundle("~/Content/login").Include(
                      "~/Content/login.css"));
        }
    }
}
