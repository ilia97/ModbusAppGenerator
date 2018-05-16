using System.Web.Optimization;

namespace ModbusAppGenerator
{
    public class BundleConfig
    {
        public static void RegisterBundles(BundleCollection bundles)
        {
            bundles.Add(new ScriptBundle("~/bundles/jquery").Include(
                        "~/Scripts/lib/jquery-{version}.js"));

            bundles.Add(new ScriptBundle("~/bundles/jqueryval").Include(
                        "~/Scripts/lib/jquery.validate*"));
            
            bundles.Add(new ScriptBundle("~/bundles/modernizr").Include(
                        "~/Scripts/lib/modernizr-*"));

            bundles.Add(new ScriptBundle("~/bundles/bootstrap").Include(
                      "~/Scripts/lib/bootstrap.js",
                      "~/Scripts/lib/respond.js"));

            bundles.Add(new ScriptBundle("~/bundles/material").Include(
                      "~/Scripts/lib/material.js",
                      "~/Scripts/lib/getmdl-select.min.js"));

            bundles.Add(new ScriptBundle("~/bundles/overrides").Include(
                      "~/Scripts/overrides/required-attribute.js"));

            bundles.Add(new ScriptBundle("~/bundles/common").Include(
                      "~/Scripts/common.js"));

            bundles.Add(new ScriptBundle("~/bundles/services").Include(
                      "~/Scripts/services/projectService.js"));

            bundles.Add(new StyleBundle("~/Content/css").Include(
                      "~/Content/css/bootstrap.css",
                      "~/Content/css/material.css",
                      "~/Content/css/site.css",
                      "~/Content/css/getmdl-select.min.css"));
        }
    }
}
