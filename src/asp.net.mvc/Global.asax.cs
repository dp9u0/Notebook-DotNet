using System.Threading;
using System.Web;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;

namespace asp.net.mvc
{

    public class MvcApplication : HttpApplication
    {

        private static readonly NLog.Logger Logger = NLog.LogManager.GetCurrentClassLogger();

        public MvcApplication()
        {
            Logger.Info("Create MvcApplication");
            HookEvent();
        }

        protected void Application_Start()
        {
            AreaRegistration.RegisterAllAreas();
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            BundleConfig.RegisterBundles(BundleTable.Bundles);
        }

        private void HookEvent()
        {
            // Thread.Sleep(30000);
            // throw new Exception();
        }

    }

}