using System.Web;
using System.Web.Optimization;

namespace Standard
{
    public class BundleConfig
    {
        // For more information on bundling, visit http://go.microsoft.com/fwlink/?LinkId=301862
        public static void RegisterBundles(BundleCollection bundles)
        {
            bundles.Add(new ScriptBundle("~/bundles/jquery").Include(
                        "~/Scripts/jquery-{version}.js"));

            bundles.Add(new ScriptBundle("~/bundles/jqueryval").Include(
                        "~/Scripts/jquery.validate*"));

            // Use the development version of Modernizr to develop with and learn from. Then, when you're
            // ready for production, use the build tool at http://modernizr.com to pick only the tests you need.
            bundles.Add(new ScriptBundle("~/bundles/modernizr").Include(
                        "~/Scripts/modernizr-*"));

            bundles.Add(new StyleBundle("~/Content/css").Include(
                      "~/Content/lib/bootstrap/bootstrap.min.css",
                      "~/Content/lib/metis/metisMenu.css",
                      "~/Content/css/font-awesome.min.css",
                      "~/Content/css/common.css",
                      "~/Content/css/site.css"));

            bundles.Add(new StyleBundle("~/Content/js").Include(
                      "~/Content/lib/bootstrap/bootstrap.min.js",
                      "~/Content/lib/metis/metisMenu.js",
                      "~/Scripts/respond.js",
                      "~/Content/js/common.js",
                      "~/Content/js/layout.js"));
        }
    }
}
