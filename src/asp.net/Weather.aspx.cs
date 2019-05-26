using System;
using System.Web;
using System.Web.Routing;

namespace asp.net
{

    public partial class Weather : System.Web.UI.Page
    {

        protected string VirtualPath1 = "";
        protected string VirtualPath2 = "";
        protected string VirtualPath3 = "";
        protected string VirtualPath4 = "";

        protected void Page_Load(object sender, EventArgs e)
        {
            var routeData = new RouteData();
            var requestContext = new RequestContext
            {
                HttpContext = new HttpContextWrapper(HttpContext.Current), RouteData = routeData
            };
            var values = new RouteValueDictionary();

            VirtualPath1 = RouteTable.Routes.GetVirtualPath(null, values)?.VirtualPath;
            VirtualPath2 = RouteTable.Routes.GetVirtualPath(requestContext, values)?.VirtualPath;
            routeData.Values.Add("areaCode", "0001");
            routeData.Values.Add("days", "1");
            VirtualPath3 = RouteTable.Routes.GetVirtualPath(requestContext, values)?.VirtualPath;

            values.Add("days", "2");
            values.Add("areaCode", "0002");
            VirtualPath4 = RouteTable.Routes.GetVirtualPath(requestContext, values)?.VirtualPath;
        }

    }

}