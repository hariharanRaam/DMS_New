using System.Web;
using System.Web.Optimization;

namespace DMS.Web
{
    public class BundleConfig
    {
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

            bundles.Add(new ScriptBundle("~/bundles/bootstrap").Include(
            "~/Scripts/bootstrap.js",
            "~/Scripts/respond.js"));

            bundles.Add(new StyleBundle("~/Content/css").Include(
            "~/Content/bootstrap.css",
            "~/Content/site.css"));


            //bundles.Add(new StyleBundle("~/Content/Bootstrap-4/css").Include(
            //          "~/Content/Bootstrap-4/css/bootstrap.css",
            //          "~/Content/Bootstrap-4/css/bootstrap.min.css",
            //          "~/Content/site.css"));

            //bundles.Add(new StyleBundle("~/Content/Bootstrap-4/js").Include(
            //        "~/Content/Bootstrap-4/js/bootstrap.js",
            //        "~/Content/Bootstrap-4/js/bootstrap.min.js",
            //        "~/Content/Bootstrap-4/js/bootstrap.bundle.js"));

            
            bundles.Add(new ScriptBundle("~/bundles/app").Include(
            "~/Scripts/App/CBmessagebox.js"
            ));
            //Kendo Reports
            bundles.Add(new ScriptBundle("~/bundles/kendoReport").Include("~/ReportViewer/js/telerikReportViewer-9.0.15.324.js"));
            bundles.Add(new StyleBundle("~/Content/kendoReport/css").Include(
            "~/Content/font-awesome.min.css",
            "~/ReportViewer/styles/telerikReportViewer-9.0.15.324.css"));    

            // Set EnableOptimizations to false for debugging. For more information,
            // visit http://go.microsoft.com/fwlink/?LinkId=301862
#if DEBUG
            BundleTable.EnableOptimizations = false;			//Set to TRUE to turn on js minification
#else
				BundleTable.EnableOptimizations = true;			//Set to TRUE to turn on js minification
#endif

        }
    }
}
