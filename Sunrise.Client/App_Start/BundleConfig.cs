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
                      "~/Scripts/libs/moment/moment.js",
                      "~/Scripts/libs/angular-ui/ui-bootstrap-tpls.js",
                      "~/Scripts/libs/respond.js"));

            bundles.Add(new ScriptBundle("~/bundles/angular").Include(
                      "~/Scripts/libs/angularjs/angular.js",
                      "~/Scripts/libs/angularjs/angular-route.js",
                      "~/Scripts/libs/angularjs/angular-animate.js",
                      "~/Scripts/libs/toaster/toaster.js",
                      "~/Scripts/libs/fileupload/ng-file-upload.js"));

            bundles.Add(new ScriptBundle("~/bundles/app").Include(
                "~/Scripts/app/helper/extension.js",
                "~/Scripts/app/app-bootstrap.js",
                "~/Scripts/app/directive/my-spinner.js",
                "~/Scripts/app/directive/my-inputset.js",
                "~/Scripts/app/directive/my-slider.js",
                "~/Scripts/app/directive/my-collapse.js",
                "~/Scripts/app/directive/my-utility.js",
                "~/Scripts/app/helper/dialog.js",
                "~/Scripts/app/helper/router.js"));


            bundles.Add(new StyleBundle("~/Content/css").Include(
                      "~/Content/vendor/font-awesome/font-awesome.css",
                      "~/Content/vendor/bootstrap/bootstrap.css",
                      "~/Content/vendor/toaster/toaster.css",
                      "~/Content/vendor/sb-admin/sb-admin2.css",
                      "~/Content/default.css"));

            bundles.Add(new StyleBundle("~/Content/login").Include(
                      "~/Content/login.css"));
        }
    }
}
