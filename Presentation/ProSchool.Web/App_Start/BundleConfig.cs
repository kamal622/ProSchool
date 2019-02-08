using System.Web;
using System.Web.Optimization;

namespace ProSchool.Web
{
    public class BundleConfig
    {
        // For more information on bundling, visit http://go.microsoft.com/fwlink/?LinkId=301862
        public static void RegisterBundles(BundleCollection bundles)
        {
            bundles.Add(new ScriptBundle("~/bundles/jquery").Include(
                        "~/Scripts/jquery-{version}.js",
                        "~/Scripts/jquery.unobtrusive-ajax.min.js"));

            bundles.Add(new ScriptBundle("~/bundles/jqueryval").Include(
                        "~/Scripts/jquery.validate*"));

            // Use the development version of Modernizr to develop with and learn from. Then, when you're
            // ready for production, use the build tool at http://modernizr.com to pick only the tests you need.
            bundles.Add(new ScriptBundle("~/bundles/modernizr").Include(
                        "~/Scripts/modernizr-*"));

            bundles.Add(new ScriptBundle("~/bundles/bootstrap").Include(
                      "~/Scripts/bootstrap.js",
                      "~/Scripts/respond.js",
                      "~/Scripts/jquery.slimscroll.min.js",
                      "~/Scripts/app.min.js"));

            bundles.Add(new StyleBundle("~/Content/css").Include(
                      "~/Content/bootstrap.css",
                      "~/Content/font-awesome.min.css",
                      "~/Content/ionicons.min.css",
                      "~/Content/AdminLTE.min.css",
                      "~/Content/skins/_all-skins.min.css",
                      "~/Content/Site.css",
                      "~/Content/imagehover.min.css"
                      ));

            bundles.Add(new ScriptBundle("~/bundles/jqx").Include(
                "~/Scripts/angular.min.js",
                "~/jqwidgets/jqxangular.js",
                "~/jqwidgets/jqxcore.js",
                "~/jqwidgets/jqxdata.js",
                "~/jqwidgets/jqx-all.js",
                "~/jqwidgets/jqxcore.js",
                "~/js/BaseController.js",
                "~/Scripts/jsUtilities.js"
               ));

            bundles.Add(new StyleBundle("~/jqwidgets/styles/css")
                .Include("~/jqwidgets/styles/jqx.base.css"
                , "~/jqwidgets/styles/jqx.web.css"
                , "~/jqwidgets/styles/jqx.android.css"
                , "~/jqwidgets/styles/jqx.arctic.css"
                , "~/jqwidgets/styles/jqx.black.css"
                , "~/jqwidgets/styles/jqx.blackberry.css"
                , "~/jqwidgets/styles/jqx.bootstrap.css"
                , "~/jqwidgets/styles/jqx.classic.css"
                , "~/jqwidgets/styles/jqx.darkblue.css"
                , "~/jqwidgets/styles/jqx.energyblue.css"
                , "~/jqwidgets/styles/jqx.fresh.css"
                , "~/jqwidgets/styles/jqx.highcontrast.css"
                , "~/jqwidgets/styles/jqx.metro.css"
                , "~/jqwidgets/styles/jqx.mobile.css"
                , "~/jqwidgets/styles/jqx.office.css"
                , "~/jqwidgets/styles/jqx.orange.css"
                , "~/jqwidgets/styles/jqx.shinyblack.css"
                , "~/jqwidgets/styles/jqx.summer.css"
                , "~/jqwidgets/styles/jqx.ui-redmond.css"
                , "~/jqwidgets/styles/jqx.ui-sunny.css"
                ));
        }
    }
}
