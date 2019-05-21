using System;

namespace asp.net
{

    public partial class Weather : System.Web.UI.Page
    {

        protected void Page_Load(object sender, EventArgs e)
        {
            //StringBuilder sb = new StringBuilder();
            //sb.Append(string.Format("Route: {0}" + "<br/>", this.RouteData.Route.GetType().FullName));
            //sb.Append(string.Format("RouteHandler: {0}" + "<br/>", this.RouteData.RouteHandler.GetType().FullName));
            //sb.Append("Variables" + "<br/>");
            //foreach (var variable in this.RouteData.Values)
            //{
            //    sb.Append(string.Format("{0}{1}: {2}" + "<br/>", "&nbsp;&nbsp;&nbsp;&nbsp;", variable.Key, variable.Value));
            //}
            //sb.Append("DataTokens" + "<br/>");
            //foreach (var variable in this.RouteData.DataTokens)
            //{
            //    sb.Append(string.Format("{0}{1}: {2}" + "<br/>", "&nbsp;&nbsp;&nbsp;&nbsp;", variable.Key, variable.Value));
            //}
            //this.Response.Write(sb.ToString());
        }

    }

}