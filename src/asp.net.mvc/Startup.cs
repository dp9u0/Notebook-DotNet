using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(asp.net.mvc.Startup))]
namespace asp.net.mvc
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
