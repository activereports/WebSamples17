using Microsoft.Owin;
using Owin;
using System.Web.Routing;
using GrapeCity.ActiveReports.Aspnet.Viewer;
using System.Reflection;

[assembly: OwinStartup(typeof(JSViewer_MVC.Startup))]

namespace JSViewer_MVC
{
    public class Startup
    {
        public static string EmbeddedReportsPrefix = "JSViewer_MVC.Reports";

        public void Configuration(IAppBuilder app)
        {
            app.UseErrorPage();

            app.UseReporting(settings =>
            {
                settings.UseEmbeddedTemplates(EmbeddedReportsPrefix, Assembly.GetAssembly(GetType()));
                settings.UseCompression = true;
            });

            RouteTable.Routes.RouteExistingFiles = true;
        }
    }
}