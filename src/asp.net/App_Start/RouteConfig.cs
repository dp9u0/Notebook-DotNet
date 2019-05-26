using System.Web.Routing;

namespace asp.net
{
    public static class RouteConfig
    {
        public static void RegisterRoutes(RouteCollection routes)
        {

            RouteTable.Routes.RouteExistingFiles = true;

            var defaults = new RouteValueDictionary { { "areacode", "010" }, { "days", 2 } };
//            var constaints = new RouteValueDictionary { { "areacode", @"0\d{2,3}" }, { "days", @"[1-3]{1}" } };
            RouteValueDictionary constaints = null;
            var dataTokens = new RouteValueDictionary { { "defaultCity", "BeiJing" }, { "defaultDays", 2 } };
            routes.MapPageRoute("default", "{areacode}/{days}", "~/weather.aspx", false, defaults, constaints, dataTokens);


            var defaults2 = new RouteValueDictionary { { "name", "*" }, { "id", "*" } };
            RouteTable.Routes.MapPageRoute("employees", "employees/{name}/{id}", "~/Default.aspx", true, defaults2);
        }
    }
}
