using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(ChatterExample.Startup))]
namespace ChatterExample
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
