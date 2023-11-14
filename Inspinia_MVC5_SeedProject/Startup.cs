using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(FARSIMAN.Startup))]
namespace FARSIMAN
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
