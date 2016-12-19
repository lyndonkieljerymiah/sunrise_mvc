using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(Sunrise.Client.Startup))]
namespace Sunrise.Client
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
