using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Web;
using System.Web.Http;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;

namespace ProyectoFinal.Web
{
    public class MvcApplication : System.Web.HttpApplication
    {
        protected void Application_Start()
        {
            AreaRegistration.RegisterAllAreas();
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            GlobalConfiguration.Configure(WebApiConfig.Register);
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            BundleConfig.RegisterBundles(BundleTable.Bundles);

            // Allows to connect Android Devices or Android Emulators to localhost HTTP IIS Client using ADB
            // in order to use API services without any additional configuration
            try
            {
                int httpPortNumber = 17559;
                using (Process process = new Process())
                {
                    process.StartInfo.FileName = $"{AppDomain.CurrentDomain.BaseDirectory}adb\\adb.exe";
                    process.StartInfo.Arguments = $"reverse tcp:{httpPortNumber} tcp:{httpPortNumber}";
                    process.StartInfo.UseShellExecute = false;
                    process.StartInfo.RedirectStandardOutput = true;
                    process.StartInfo.CreateNoWindow = true;
                    process.Start();
                }
            }
            catch
            {
                Console.WriteLine("ADB command failed");
            }
        }
    }
}
