using System.Web.Mvc;

namespace asp.net.mvc.Controllers
{

    public class HomeController : Controller
    {

        private static readonly NLog.Logger Logger = NLog.LogManager.GetCurrentClassLogger();

        public HomeController()
        {
            Logger.Info("Home Controller Created");
        }

        protected override void ExecuteCore()
        {
            Logger.Info("Home Controller ExecuteCore");
            base.ExecuteCore();
        }

        public ActionResult Index()
        {
            Logger.Info("Home Controller Index");
            return View();
        }

        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";
            Logger.Info("Home Controller About");
            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }

    }

}