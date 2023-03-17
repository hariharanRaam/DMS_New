using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;
using System.Web.Optimization;
using System.IO;
using GleamTech.AspNet;
using GleamTech.DocumentUltimate;

namespace DMS.Web
{
    public class MvcApplication : System.Web.HttpApplication
    {
        protected void Application_Start()
        {
            AreaRegistration.RegisterAllAreas();
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            //BundleConfig.RegisterBundles(BundleTable.Bundles);

            BundleConfig.RegisterBundles(BundleTable.Bundles);

            //var licenseFile = Hosting.ResolvePhysicalPath("~/App_Data/License.dat");
            //if (File.Exists(licenseFile))
            //DocumentUltimateConfiguration.Current.LicenseKey = File.ReadAllText(licenseFile);
            log4net.Config.XmlConfigurator.Configure();
        }
    }
}
