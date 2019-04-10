using Microsoft.Owin;
using Owin;

[assembly: OwinStartup(typeof(MazzaWebSite.Startup))]
namespace MazzaWebSite
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
